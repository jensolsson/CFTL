#
# HandlerFAT.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#
package HandlerFAT;

use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(validate extract);

sub new {
	my $self = {
		
	};
	bless $self, "HandlerFAT";
	return $self;
}

sub extract {
	my $self = shift;
	my $source = shift;
	my @ret = ();

	die "Invalid data" if !$self->validate($source);

	my $data = $self->readSector($source, 0);
	
	(my $BS_OEMName, my $BPB_BytsPerSec, my $BPB_SecPerClus, my $BPB_RsvdSecCnt, my $BPB_NumFATs, my $BPB_RootEntCnt, my $BPB_TotSec16, my $BPB_Media, my $BPB_FATSz16, my $BPB_TotSec32) = unpack("x3 a8 v C v C v v C v x2 x2 x4 L", $data);

	my $rootDirSectors = int((($BPB_RootEntCnt * 32) + ($BPB_BytsPerSec - 1)) / $BPB_BytsPerSec);
	my $fatSize = ($BPB_FATSz16!=0) ? $BPB_FATSz16 : unpack("L", substr($data, 36, 4));
	my $totalSectors = ($BPB_TotSec16!=0) ? $BPB_TotSec16 : $BPB_TotSec32; 
	my $dataSectors = $totalSectors - ($BPB_RsvdSecCnt + ($BPB_NumFATs * $fatSize) + $rootDirSectors); 
	my $clusters = int($dataSectors / $BPB_SecPerClus);
	my $firstRootDirSector = $BPB_RsvdSecCnt + $BPB_NumFATs * $fatSize;
	my $firstDataSector = $BPB_RsvdSecCnt + $BPB_NumFATs * $fatSize + $rootDirSectors;

	my $fatType;
	if($clusters < 4085) { $fatType=12; }
	elsif($clusters < 65525) { $fatType=16; }
	else { $fatType=32; } 

	(my $BPB_FATSz32, my $BPB_ExtFlags, my $BPB_FSVer, my $BPB_RootClus, my $BPB_FSInfo, my $BPB_BkBootSec, my $BS_BootSig, my $BS_VolID, my $BS_VolLab, my $BS_FilSysType);
	
	if($fatType==32) {
		($BPB_FATSz32, $BPB_ExtFlags, $BPB_FSVer, $BPB_RootClus, $BPB_FSInfo, $BPB_BkBootSec, $BS_BootSig, $BS_VolID, $BS_VolLab, $BS_FilSysType) = unpack("x36 L v v L v v x12 x x C L a11 a8", $data);
	}
	if($fatType==12 || $fatType==16) {
		$BPB_RootClus = 2;
		(my $BS_BootSig, my $BS_VolID, my $BS_VolLab, my $BS_FilSysType) = unpack("x36 x x C L a11 a8", $data);
	}

	my $cluster = $BPB_RootClus;
		
	my $rootDirectory;

	if($fatType == 32 || $cluster != 2) { #or non-root cluster
		do {
			$rootDirectory .= $self->readCluster($source, $cluster, $firstDataSector, $BPB_BytsPerSec, $BPB_SecPerClus);
			$cluster = $self->fat($source, $fatType, $BPB_RsvdSecCnt, $BPB_BytsPerSec, $cluster);
		} until((($fatType==32) && ($cluster == 268435455)) || (($fatType==16) && ($cluster == 65535)));
	}
	else {
		$rootDirectory .= $self->readRootCluster16($source, $firstRootDirSector, $rootDirSectors, $BPB_BytsPerSec, $BPB_SecPerClus);
	}
	my @files = listFiles($rootDirectory, $BPB_RootEntCnt);

	my $path;

	my @dirs = ();
	my $testcount = 0;

	do {
		foreach(@files) {
			next if $_->[0] eq "." || $_->[0] eq "..";
			#print $_->[0];
			#print join ",\t", @$_;
			
			my $filename = "$path/".($_->[1]||$_->[0]);
			
			$cluster = $_->[2];
			my @clusterchain;
			do {
				push(@clusterchain, $cluster);
				$cluster = $self->fat($source, $fatType, $BPB_RsvdSecCnt, $BPB_BytsPerSec, $cluster);
			} until($cluster == 0 || (($fatType==32) && ($cluster == 268435455)) || (($fatType==16) && ($cluster == 65535)));
			
			if($_->[4] =~ /D/) {
				push @dirs, $path."/".($_->[1]||$_->[0]);
				push @dirs, $_->[2];
			}


			my $cchain;
			my $cstart;
			my $cend;
			foreach(@clusterchain) {
				if(!$cstart) {
					$cstart = $_;
					$cend = $_;
					next;	
				}
				if($_ == $cend+1) {
					$cend = $_;
					next;	
				}
				$cchain .= clusterToPosition($cstart, $firstDataSector, $BPB_BytsPerSec, $BPB_SecPerClus)."-".clusterToPosition($cend+1, $firstDataSector, $BPB_BytsPerSec, $BPB_SecPerClus).",";
				$cstart = 0;
		 		$cend = 0;
			}
			$cchain .= clusterToPosition($cstart, $firstDataSector, $BPB_BytsPerSec, $BPB_SecPerClus)."-".clusterToPosition($cend+1, $firstDataSector, $BPB_BytsPerSec, $BPB_SecPerClus);
			
			
			my $timestamps = [
				"<Timestamp type=\"File was created\" value=\"$_->[5]\" origin=\"FAT\" />",
				"<Timestamp type=\"File was last modified\" value=\"$_->[6]\" origin=\"FAT\" />",
				"<Timestamp type=\"File was last accessed\" value=\"$_->[7]\" origin=\"FAT\" />"
			];
			push @ret, Source::new($source, "$filename", $cchain, $source->{'_id'}, $timestamps);
			
			
			$testcount++;
		}
		
		$path = shift @dirs;
		$cluster = shift @dirs;
		my $directory = "";
		do {
			$directory .= $self->readCluster($source, $cluster, $firstDataSector, $BPB_BytsPerSec, $BPB_SecPerClus);
			$cluster = $self->fat($source, $fatType, $BPB_RsvdSecCnt, $BPB_BytsPerSec, $cluster);
		} until((($fatType==32) && ($cluster == 268435455)) || (($fatType==16) && ($cluster == 65535)));
				
		@files = listFiles($directory);
		
		
	} while(scalar(@dirs));


	return @ret;
}


sub fat {
	my $self = shift;
	my $source = shift;
	my $fatType = shift;
	
	my $reservedSectors = shift;
	my $bytesPerSector = shift;
	my $fatIndex = shift;
	
	my $size = ($fatType==32)?4:2;
	my $pos = $reservedSectors * $bytesPerSector + $fatIndex * $size;

	my $sector = int($pos / $bytesPerSector);
	$pos %= $bytesPerSector;
	
	my $data = $self->readSector($source, $sector);
	my $fatData = substr($data, $pos, $size);
	
	if($fatType==32) {
		return unpack("L", $fatData) & 0x0FFFFFFF;
	}
	elsif($fatType==16) {
		return unpack("v", $fatData);
	}
	else {
		die "Could not handle this FAT type";	
	}
}

sub listFiles {
	my $data = shift;
	my $entries = shift;
	
	my @ret = ();
	
	for(my $c=0;$c<length($data);$c+=32) {
		
		my $record = substr($data, $c, 32);

		(my $name, my $ext, my $attr, my $timeCreateTenth, my $timeCreate, my $dateCreate, my $dateAccess, my $clusterHigh, my $timeWrite, my $dateWrite, my $clusterLow, my $fileSize) = unpack("A8 A3 C x C v v v v v v v L", $record);
		my $cluster = (($clusterHigh<<16) | $clusterLow);
		
		if(ord(substr($name, 0, 1)) ne hex("e5") && ord(substr($name, 0, 1)) ne hex("0") && (($attr&0x8)==0)) {
			my $fullname = $ext ne "" ? "$name.$ext" : "$name";
			my $strattr;
			$strattr .= "R" if ($attr&0x1)!=0;
			$strattr .= "H" if ($attr&0x2)!=0;
			$strattr .= "S" if ($attr&0x4)!=0;
			$strattr .= "V" if ($attr&0x8)!=0;
			$strattr .= "D" if ($attr&0x10)!=0;
			$strattr .= "A" if ($attr&0x20)!=0;
			my $create = formatDate($dateCreate)." ".formatTime($timeCreate, $timeCreateTenth);
			my $modify = formatDate($dateWrite)." ".formatTime($timeWrite);
			my $access = formatDate($dateAccess);

			my $longName = "";
			for(my $b=$c-32;$b>0;$b-=32) {
				my $longRecord = substr($data, $b, 32);
				(my $type, my $longName1, my $what1, my $checksum, my $longName2, my $longName3) = unpack("C A10 C x C A12 x2 A4", $longRecord);
				last if ($type!=1 && $type != 0x41) && ($b==$c-32);
				$longName .= pack("A10 A12 A4", $longName1, $longName2, $longName3);
				last if ($type & 0x40);
			}
			
			my $longName8 = "";
			for(my $i=0;$i<length($longName);$i+=2) {
				my $char = substr($longName, $i, 1);
				last if $char eq "\xFF" || $char eq "\x00";
				$longName8 .= $char;
			}
			$longName8 =~ s/^[ \x00]*(.+?)[ \x00]*$/$1/; #trim, bad, temporary
						
			my @retEntry = ($fullname, $longName8, $cluster, $fileSize, $strattr, $create, $modify, $access);
			push @ret, [ @retEntry ];
		}
		
		last if --$entries == 0;
	}	
	return @ret;
}

sub formatDate {
	my $datestamp = shift;
	my $day = 0x1f & $datestamp;
	$day = "0$day" if $day < 10;
	my $month = 0x0f & ($datestamp >> 5);
	$month = "0$month" if $month < 10;
	my $year = ($datestamp >> 9) + 1980;
	
	return "$year-$month-$day";
}

sub formatTime {
	my $tenthActive = @_ == 2;
	my $timestamp = shift;
	my $tenth = shift;
	
	my $second = (0x1f & ($timestamp << 1)) + (($tenth>100)?1:0);
	$second = "0$second" if $second < 10;
	my $minute = 0x3f & ($timestamp >> 5);
	$minute = "0$minute" if $minute < 10;
	my $hour = ($timestamp >> 11);
	$hour = "0$hour" if $hour < 10;
	$tenth%=100;
	$tenth = "0$tenth" if $tenth < 10;
	
	return "$hour:$minute:$second.".$tenth if $tenthActive;
	return "$hour:$minute:$second";
}


sub validate {
	my $self = shift;
	my $source = shift;

	return 0 if unpack("v", $source->data(510, 2)) != 0xAA55;

	my @jmpBoot = unpack("C3", $source->data(0, 3));
	return 0 if !($jmpBoot[0] == 0xE9 || ($jmpBoot[0] == 0xEB && $jmpBoot[2] == 0x90));

	my $fatType = $source->data(54, 8);
	return 0 if substr($fatType, 0, 3) ne "FAT";
	
	return 1;
}

sub readSector {
	my $self = shift;
	my $source = shift;
	my $sector = shift;
	
	return $source->data(512 * $sector, 512);	
}

sub clusterToPosition() {
	my $cluster = shift;

	my $firstDataSector = shift;
	my $bytesPerSector = shift;
	my $sectorsPerCluster = shift;	
	
	return $firstDataSector * $bytesPerSector + ($cluster - 2) * $bytesPerSector * $sectorsPerCluster;
}

sub readCluster {
	my $self = shift;
	my $source = shift;
	my $cluster = shift;

	my $firstDataSector = shift;
	my $bytesPerSector = shift;
	my $sectorsPerCluster = shift;
	
	#First data cluster is cluster 2
	my $pos = $firstDataSector * $bytesPerSector + ($cluster - 2) * $bytesPerSector * $sectorsPerCluster;

	return $source->data($pos, $bytesPerSector * $sectorsPerCluster);
}

sub readRootCluster16 {
	my $self = shift;
	my $source = shift;
	
	my $firstRootDirSector = shift;
	my $rootDirSectors = shift;
	my $bytesPerSector = shift;
	my $sectorsPerCluster = shift;	
	
	return $source->data($firstRootDirSector * $bytesPerSector, $bytesPerSector * $rootDirSectors);
}


sub printHex {
	my $data = shift;
	for(my $c=0;$c<length($data);$c+=16) {
		print unpack("H*", pack("C C", $c>>8, $c))."   ";
		my $text = "";
		for(my $i=$c;$i<$c+16;$i++) {
			if($i>=length($data)) { print "   "; }
			else { print unpack("H*", substr($data, $i, 1)).($i%4==3&&$i%16!=15?"|":" "); }
			my $char=substr($data, $i, 1);
			if($i<length($data)) { $text.=(ord($char)<32||ord($char)>127)?".":$char; }
		}
		print "   $text\n";
	}
}


1;