#
# HandlerNTFS.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#
package HandlerNTFS;

use Digest::MD5 qw(md5_base64);
use CFTL::TimeUtils;
use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(validate extract);

#temp
my $debug = 0;

sub new {
	my $self = {
		
	};
	bless $self, "HandlerNTFS";
	return $self;
}

sub extract {
	my $self = shift;
	my $source = shift;
	my @ret = ();

	die "Invalid data" if !$self->validate($source);

	my $data = $self->readSector($source, 0);

	my $bpbData = substr($data, 11, 25+48);
	my %bpb;
	($bpb{'BytsPerSec'}, $bpb{'SecPerClus'}, $bpb{'RsvdSecCnt'}, $bpb{'Media'}, $bpb{'HiddenSectors'}, $bpb{'TotalSectors_LO'}, $bpb{'TotalSectors_HI'}, $bpb{'MFT_LO'}, $bpb{'MFT_HI'}, $bpb{'MFTMirror_LO'}, $bpb{'MFTMirror_HI'}, $bpb{'ClustersPerFileRecordSegment'}, $bpb{'ClustersPerIndexBlock'}, $bpb{'SerialNumber'}, $bpb{'Checksum'}) = unpack("v C v x3 x2 C x2 x2 x2 L x4 x4 L2 L2 L2 c x3 c x3 a8 L", $bpbData);

	if($bpb{'ClustersPerFileRecordSegment'} < 0) {
		$bpb{'FileRecordSegmentSize'} = 2 ** (-1 * $bpb{'ClustersPerFileRecordSegment'});
	}
	else {
		$bpb{'FileRecordSegmentSize'} = $bpb{'ClustersPerFileRecordSegment'}*$bpb{'BytsPerSec'}*$bpb{'SecPerClus'};
	}

	if($bpb{'ClustersPerIndexBlock'} < 0) {
		$bpb{'IndexBlockSize'} = 2 ** (-1 * $bpb{'ClustersPerIndexBlock'});
	}
	else {
		$bpb{'IndexBlockSize'} = $bpb{'ClustersPerIndexBlock'}*$bpb{'BytsPerSec'}*$bpb{'SecPerClus'};
	}

	die "Cannot yet handle 64bit values" if $bpb{'TotalSectors_HI'} || $bpb{'MFT_HI'} || $bpb{'MFTMirror_HI'};

	my $list = $self->listDirectory($source, \%bpb, "", 5);
	
	return @$list;
}


sub listDirectory {
	(my $self, my $source, my $bpb, my $path, my $mftIndex, my $currentFile) = @_;
	my @ret = ();	
	
	
	#temp
	$debug = ($path =~ /All Users\/Desktop$/);
	#$debug = 1;
	if($debug) {
		print STDERR "\n\n\nSTART DEBUG $path\n";
		<STDIN>;
	}
	
	my $mftRecord = $self->getMftRecord($source, $bpb, $mftIndex);
	my $attributes = $self->getAttributes($source, $mftRecord->{'AttributesData'}, $bpb);
	
	
	my $fileData;
	
	foreach(@$attributes) {
		if($_->{'TypeString'} eq "\$DATA") {
			print STDERR "\$DATA\n" if $debug;
			
			
			
			my $timestamps = [
				"<Timestamp type=\"File's Master Record was modified\" value=\"$currentFile->{MFTChanged}\" origin=\"NTFS\" />",
				"<Timestamp type=\"File was created\" value=\"$currentFile->{Created}\" origin=\"NTFS\" />",
				"<Timestamp type=\"File was last modified\" value=\"$currentFile->{Altered}\" origin=\"NTFS\" />",
				"<Timestamp type=\"File was last accessed\" value=\"$currentFile->{Read}\" origin=\"NTFS\" />"
			];
			

			
			if($_->{'NonResidentFlag'}) {
				unless(dataRunsToPositions($bpb, $_->{'Data'}, $_->{'RealSize_LO'}) eq "") {	
					push @ret, Source::new($source, "$path", dataRunsToPositions($bpb, $_->{'Data'}, $_->{'RealSize_LO'}), $source->{'_id'}, $timestamps);
				}
			}
			else {
				my $pos = $mftRecord->{'MftPosition'}+$mftRecord->{'AttributesDataOffset'} + $_->{'DataOffset'};
				push @ret, Source::new($source, "$path", "$pos-".($pos+length($_->{'Data'})), $source->{'_id'}, $timestamps);
			}

		}
		elsif($_->{'TypeString'} eq "\$FILE_NAME") {
			print STDERR "\$FILE_NAME\n" if $debug;
			
			parseFileName($_->{'Data'});	
		}
		elsif($_->{'TypeString'} eq "\$INDEX_ALLOCATION") {
			print STDERR "\$INDEX_ALLOCATION\n";
			
			printHex($self->readCluster($source, vcnToLcn($_->{'Data'}, 0), $bpb->{'RsvdSecCnt'}, $bpb->{'BytsPerSec'}, $bpb->{'SecPerClus'}));
			
			
			my @clusters;
			@clusters = $self->parseIndexAllocation($source, $bpb, $_->{'Data'});

			
			foreach(@clusters) {
				my $files = $_->{'Entries'};
				
				foreach(@$files) {
					my $file = parseFileName($_->{'Stream'});
					next unless $file->{'NameSpace'} & 0x1; #not windows Namespace
					my $type = ($file->{'Flags'} & 0x10000000) ? "DIR" : "FILE";
					
										
					#print $path."/".$file->{'FileName'}.", ".$_->{'FileReference_LO'}.", $type, $file->{Created}, $file->{Altered}, $file->{Read}\n";
					
					sub conv {
						my $data = shift;
						my $dataout = "";
						for(my $c=0;$c<length($data);$c+=2) {
							$dataout .= substr($data, $c, 1);
						}
						return $dataout;
					}
					
					if($_->{'FileReference_LO'}>=16) {
						my $fileName = conv($file->{'FileName'});

						my $list = $self->listDirectory($source, $bpb, "$path/".$fileName, $_->{'FileReference_LO'}, $file); # if $type eq "DIR";
						push @ret, @$list;
					}
				}
			}
		}
		elsif($_->{'TypeString'} eq "\$INDEX_ROOT") {
			print STDERR "\$INDEX_ROOT\n";

			printHex($_->{'Data'});

			my @clusters;
			@clusters = $self->parseIndexRoot($source, $bpb, $_->{'Data'});
			
			foreach(@clusters) {
				my $files = $_->{'Entries'};
				
				foreach(@$files) {
					my $file = parseFileName($_->{'Stream'});
					my $fileName = conv($file->{'FileName'});
					print($fileName."\n");
				}
			}
			
			
			print STDERR "back\n";

			
		}
		else {
			print STDERR "Unhandled: $_->{'TypeString'}\n" if $debug;	
		}	
	}
	
	
	if($debug) {
		print STDERR "END DEBUG\n";
		<STDIN>;
	}

	return \@ret;
}


sub parseIndexRoot {
	(my $self, my $source, my $bpb, my $data) = @_;
	
	my @clusters;
	
	my $clusterData = $data;

	my %index;	
	
	#($index{'Signature'}, $index{'UpdateSequenceOffset'}, $index{'UpdateSequenceSize'}, $index{'LogFileSequenceNumber_LO'}, $index{'LogFileSequenceNumber_HI'}, $index{'VCNThisIndex_LO'}, $index{'VCNThisIndex_HI'}, $index{'IndexEntriesOffset'}, $index{'IndexEntriesSize'}, $index{'IndexEntriesAllocatedSize'}, $index{'NotLeafNode'}, $index{'UpdateSequence'}) = unpack("a4 v v L2 L2 L L L C x3 v", $clusterData);	
	
	($index{'IndexEntriesOffset'}, $index{'IndexEntriesSize'}, $index{'IndexEntriesAllocatedSize'}, $index{'NotLeafNode'}) = unpack("x16 L L L C", $clusterData);
	
	
	print STDERR "parseIndexRoot signature: $index{'Signature'}\n";
	
	my $indexData = substr($clusterData, $index{'IndexEntriesOffset'}+0x10, $index{'IndexEntriesSize'});
	
	my @entries;
	my $offset = 0;
	for(;;) {
		my %indexEntry;
		($indexEntry{'FileReference_LO'}, $indexEntry{'FileReference_HI'}, $indexEntry{'Length'}, $indexEntry{'StreamLength'}, $indexEntry{'Flags'}) = unpack("L2 v v L", substr($indexData, $offset));

		if($indexEntry{'Flags'} & 0x2) { #last entry
			last;
		}
		else {
			$indexEntry{'Stream'} = substr($indexData, $offset+16, $indexEntry{'StreamLength'});
		}
		
		if($indexEntry{'Flags'} & 0x1) { #subnode is set
			$indexEntry{'Entry'} = substr($indexData, $indexEntry{'Length'}-8, 8);
		}
		
		push(@entries, \%indexEntry);
		
		$offset += $indexEntry{'Length'};
	}
	
	$index{'Entries'} = \@entries;
	
	push @clusters, \%index;

	return @clusters;
}


sub parseIndexAllocation {
	(my $self, my $source, my $bpb, my $data) = @_;
	
	my @clusters;
	
	for(my $c=0;$c<vcnCount($data);$c++) {
		my $clusterData = $self->readCluster($source, vcnToLcn($data, $c), $bpb->{'RsvdSecCnt'}, $bpb->{'BytsPerSec'}, $bpb->{'SecPerClus'});


		my %index;
		($index{'Signature'}, $index{'UpdateSequenceOffset'}, $index{'UpdateSequenceSize'}, $index{'LogFileSequenceNumber_LO'}, $index{'LogFileSequenceNumber_HI'}, $index{'VCNThisIndex_LO'}, $index{'VCNThisIndex_HI'}, $index{'IndexEntriesOffset'}, $index{'IndexEntriesSize'}, $index{'IndexEntriesAllocatedSize'}, $index{'NotLeafNode'}, $index{'UpdateSequence'}) = unpack("a4 v v L2 L2 L L L C x3 v", $clusterData);	

		print STDERR "parseIndexAllocation signature: $index{'Signature'}\n";

		
		my $indexData = substr($clusterData, $index{'IndexEntriesOffset'}+0x18, $index{'IndexEntriesSize'});
		
		my @entries;
		my $offset = 0;
		for(;;) {
			my %indexEntry;
			($indexEntry{'FileReference_LO'}, $indexEntry{'FileReference_HI'}, $indexEntry{'Length'}, $indexEntry{'StreamLength'}, $indexEntry{'Flags'}) = unpack("L2 v v L", substr($indexData, $offset));
	
			if($indexEntry{'Flags'} & 0x2) { #last entry
				last;
			}
			else {
				$indexEntry{'Stream'} = substr($indexData, $offset+16, $indexEntry{'StreamLength'});
			}
			
			if($indexEntry{'Flags'} & 0x1) { #subnode is set
				$indexEntry{'Entry'} = substr($indexData, $indexEntry{'Length'}-8, 8);
			}
			
			push(@entries, \%indexEntry);
			
			$offset += $indexEntry{'Length'};
		}
		
		$index{'Entries'} = \@entries;
		
		push @clusters, \%index;

	}

	return @clusters;
}


sub vcnCount {
	(my $data) = @_;
	
	my $offset = 0;
	my $clusterCount = 0;
	for(;;) {
		my $sizes = ord(substr($data, $offset, 1));
		last unless $sizes;
		
		my $sizeOffset = $sizes >> 4;
		my $sizeLength = $sizes & 0xF;

		unless($sizeOffset) {
			warn "Sparse files are currently cut of where the first zero area begin.";
			last;
		}

		my $lengthData = substr($data, $offset+1, $sizeLength).("\x00" x (4-$sizeLength));		
		my $clusterLength = unpack("L", $lengthData);
		
		$clusterCount += $clusterLength;

		$offset += 1 + $sizeOffset + $sizeLength;
	}	
	return $clusterCount;
}

sub vcnToLcn {
	(my $data, my $cluster) = @_;
	
	my $offset = 0;
	my $clusterOffset = 0;
	for(;;) {
		my $sizes = ord(substr($data, $offset, 1));
		last unless $sizes;
		
		my $sizeOffset = $sizes >> 4;
		my $sizeLength = $sizes & 0xF;


		die "Cannot yet handle sparse files" if $sizeOffset == 0;

		my $lengthData = substr($data, $offset+1, $sizeLength).("\x00" x (4-$sizeLength));
		
		my $sign = 0x80 & ord(substr($data, $offset+1+$sizeLength+$sizeOffset-1, 1));		
		if($sign) {
			#print "!!!SIGN!!!\n";
			#<STDIN>;
		}


		my $offsetData;
		if($sign) {
			$offsetData = substr($data, $offset+1+$sizeLength, $sizeOffset).("\xff" x (4-$sizeOffset));
			#$offsetData = substr($offsetData, 0, 3).  chr(ord(substr($offsetData, 3, 1)) | $sign);
			#die "!!!Cannot yet handle negative cluster offsets!!!\n";
			#print unpack("l", $offsetData);
			#<STDIN>;
		}
		else {
			$offsetData = substr($data, $offset+1+$sizeLength, $sizeOffset).("\x00" x (4-$sizeOffset));
		}
		
		my $clusterLength = unpack("L", $lengthData);
		$clusterOffset += unpack("l", $offsetData);


		if($cluster < $clusterLength) {
			my $lcn = $clusterOffset + $cluster;
			return $lcn;
		}
		$cluster -= $clusterLength;

		$offset += 1 + $sizeOffset + $sizeLength;
	}
	die "Cluster out of bounds, not part of data run!";
}


sub dataRunsToPositions {
	(my $bpb, my $data, my $size) = @_;
		
	my $positions;
	my $offset = 0;
	my $clusterOffset = 0;
	for(;;) {
		my $sizes = ord(substr($data, $offset, 1));
		last unless $sizes;
		
		my $sizeOffset = $sizes >> 4;
		my $sizeLength = $sizes & 0xF;

		last if $sizeOffset == 0;

		my $lengthData = substr($data, $offset+1, $sizeLength).("\x00" x (4-$sizeLength));
		
		my $sign = 0x80 & ord(substr($data, $offset+1+$sizeLength+$sizeOffset-1, 1));
		
		my $offsetData;

		if($sign) {
			$offsetData = substr($data, $offset+1+$sizeLength, $sizeOffset).("\xff" x (4-$sizeOffset));
			#$offsetData = substr($offsetData, 0, 3).  chr(ord(substr($offsetData, 3, 1)) | $sign);
		}
		else {
			$offsetData = substr($data, $offset+1+$sizeLength, $sizeOffset).("\x00" x (4-$sizeOffset));
			
		}

		my $clusterLength = unpack("L", $lengthData);
		$clusterOffset += unpack("l", $offsetData);

		my $byteOffset = $clusterOffset * $bpb->{'BytsPerSec'} * $bpb->{'SecPerClus'};
		my $byteLength = $clusterLength * $bpb->{'BytsPerSec'} * $bpb->{'SecPerClus'};

		if(defined($size)) {
			$byteLength = $size if $size < $byteLength;
			$size -= $byteLength;
		}

		my $start = $bpb->{'RsvdSecCnt'} * $bpb->{'BytsPerSec'};

		$positions .= ($start + $byteOffset) . "-" . ($start + $byteOffset + $byteLength) . ",";

		$offset += 1 + $sizeOffset + $sizeLength;
	}
	return substr($positions, 0, length($positions) - 1);
}

sub getMftRecord {
	(my $self, my $source, my $bpb, my $mftIndex) = @_;


	my $data;

	my $cluster = 0;

	if($mftIndex<16) { #First 16 records are placed right after $bpb->{'MFT_LO'}
		$cluster = $bpb->{'MFT_LO'}+int($mftIndex/4);
		$data = $self->readCluster($source, $cluster, $bpb->{'RsvdSecCnt'}, $bpb->{'BytsPerSec'}, $bpb->{'SecPerClus'});	
	}
	else {
		my $mft = $self->getMftRecord($source, $bpb, 0);
		my $attributes = $self->getAttributes($source, $mft->{'AttributesData'}, $bpb);
		
		foreach(@$attributes) {
			next unless $_->{'TypeString'} eq "\$DATA";
			
			$cluster = vcnToLcn($_->{'Data'}, int($mftIndex/4));
			$data = $self->readCluster($source, $cluster, $bpb->{'RsvdSecCnt'}, $bpb->{'BytsPerSec'}, $bpb->{'SecPerClus'});
			last;
		}
	}

	my $recordOffset = 1024 * ($mftIndex%4);
	
	
	my %mftRecord;
	
	$mftRecord{'MftPosition'} = $bpb->{'RsvdSecCnt'} * $bpb->{'BytsPerSec'} + $cluster * $bpb->{'BytsPerSec'} * $bpb->{'SecPerClus'};
	
	($mftRecord{'Signature'}, $mftRecord{'UpdateSequenceOffset'}, $mftRecord{'UpdateSequenceSize'}, $mftRecord{'LogSequenceNumber_LO'}, $mftRecord{'LogSequenceNumber_HI'}, $mftRecord{'SequenceNumber'}, $mftRecord{'HardLinkCount'}, $mftRecord{'AttributesOffset'}, $mftRecord{'Flags'}, $mftRecord{'RecordSize'}, $mftRecord{'RecordAllocatedSize'}, $mftRecord{'BaseFileReference_LO'}, $mftRecord{'BaseFileReference_HI'}, $mftRecord{'NextAttributeId'}, $mftRecord{'AlignTo4Bytes'}, $mftRecord{'RecordNumber'}) = unpack("a4 v v L2 v v v v L L L2 v v L", substr($data, $recordOffset));

	$mftRecord{'AttributesDataOffset'} = $recordOffset + $mftRecord{'AttributesOffset'};
	$mftRecord{'AttributesData'} = substr($data, $recordOffset + $mftRecord{'AttributesOffset'}, $mftRecord{'RecordSize'} - $mftRecord{'AttributesOffset'});
	
	return \%mftRecord;
}


sub getAttributes {
	(my $self, my $source, my $data, my $bpb) = @_;

	my @attributes;

	my $offset = 0;

	while(1) {	
		my %attr;
		
		($attr{'Type'}, $attr{'Length'}, $attr{'NonResidentFlag'}, $attr{'NameLength'}, $attr{'NameOffset'}, $attr{'Flags'}, $attr{'AttributeId'}) = unpack("L L C C v v v", substr($data, $offset, 16));
	
		my $attrtypes = {
			"10"=>"\$STANDARD_INFORMATION",
			"20"=>"\$ATTRIBUTE_LIST",
			"30"=>"\$FILE_NAME",
			#"40"=>"\$VOLUME_VERSION", #Old and never actually used
			"40"=>"\$OBJECT_ID",
			"50"=>"\$SECURITY_DESCRIPTOR",
			"60"=>"\$VOLUME_NAME",
			"70"=>"\$VOLUME_INFORMATION",
			"80"=>"\$DATA",
			"90"=>"\$INDEX_ROOT",
			"a0"=>"\$INDEX_ALLOCATION",
			"b0"=>"\$BITMAP",
			#"c0"=>"\$SYMBOLIC_LINK", #Old and never actually used
			"C0"=>"\$REPARSE_POINT",
			"d0"=>"\$EA_INFORMATION",
			"e0"=>"\$EA",
			"f0"=>"\$PROPERTY_SET",
			"100"=>"\$LOGGED_UTILITY_STREAM"
		};
	
		$attr{'Type'} = sprintf("%x", $attr{'Type'});

		last if $attr{'Type'} eq "ffffffff" || $attr{'Type'} eq '0'; #0 seem to be an undocumented end of list (or could it be end of block as well?)

		$attr{'TypeString'} = $attrtypes->{$attr{'Type'}};
		$offset += 16;
				
		if($attr{'NonResidentFlag'} == 0) {
			($attr{'AttributeLength'}, $attr{'AttributeOffset'}, $attr{'Indexed'}) = unpack("L v C x", substr($data, $offset, 8));
			
			$offset += 8;
			
			$attr{'Name'} = substr($data, $offset, $attr{'NameLength'}*2);
			$offset += $attr{'NameLength'}*2;
			
			$attr{'DataOffset'} = $offset;

			$attr{'Data'} = substr($data, $offset, $attr{'AttributeLength'});
			$offset += $attr{'Length'} - 24 - $attr{'NameLength'}*2;
		}
		else {
			($attr{'StartingVCN_LO'}, $attr{'StartingVCN_HI'}, $attr{'ATTR_LastVCN_LO'}, $attr{'LastVCN_HI'}, $attr{'DataRunOffset'}, $attr{'CompressionUnitSize'}, $attr{'AllocatedSize_LO'}, $attr{'AllocatedSize_HI'}, $attr{'RealSize_LO'}, $attr{'RealSize_HI'}, $attr{'InitSize_LO'}, $attr{'InitSize_HI'}) = unpack("L2 L2 v v x4 L2 L2 L2", substr($data, $offset, 48));
			$offset += 48;
			
			$attr{'Name'} = substr($data, $offset, $attr{'NameLength'}*2);
			$offset += $attr{'NameLength'}*2;		
	
			$attr{'Data'} = substr($data, $offset, $attr{'Length'} - 48 - $attr{'NameLength'}*2);
			$offset += $attr{'Length'} - 48 - 16 - $attr{'NameLength'}*2;
		}

		push(@attributes, \%attr);
	}	
	
	return \@attributes;
}


sub parseFileName {
	my $data = shift;
	my %fileName;
	($fileName{'Parent_LO'}, $fileName{'Parent_HI'}, $fileName{'Created'}, $fileName{'Altered'}, $fileName{'MFTChanged'}, $fileName{'Read'}, $fileName{'AllocatedSize_LO'}, $fileName{'AllocatedSize_HI'}, $fileName{'RealSize_LO'}, $fileName{'RealSize_HI'}, $fileName{'Flags'}, $fileName{'UnknownEAsReparse'}, $fileName{'Length'}, $fileName{'NameSpace'}) = unpack("L2 a8 a8 a8 a8 L2 L2 L L C C", $data);
	
	$fileName{'Created'} = TimeUtils::WinTime64($fileName{'Created'});
	$fileName{'Altered'} = TimeUtils::WinTime64($fileName{'Altered'});
	$fileName{'MFTChanged'} = TimeUtils::WinTime64($fileName{'MFTChanged'});
	$fileName{'Read'} = TimeUtils::WinTime64($fileName{'Read'});
	$fileName{'FileName'} = substr($data, 66, $fileName{'Length'}*2);

	return \%fileName;
}

sub validate {
	my $self = shift;
	my $source = shift;

	return 0 if unpack("v", $source->data(510, 2)) != 0xAA55;

	my @jmpBoot = unpack("C3", $source->data(0, 3));
	return 0 if !($jmpBoot[0] == 0xE9 || ($jmpBoot[0] == 0xEB && $jmpBoot[2] == 0x90));
	
	return 0 if $source->data(3, 8) ne "NTFS    ";
	
	return 1;
}

sub readCluster {
	my $self = shift;
	my $source = shift;
	my $cluster = shift;

	my $firstDataSector = shift;
	my $bytesPerSector = shift;
	my $sectorsPerCluster = shift;
	
	#First data cluster is cluster 2
	my $pos = $firstDataSector * $bytesPerSector + ($cluster) * $bytesPerSector * $sectorsPerCluster;

	return $source->data($pos, $bytesPerSector * $sectorsPerCluster);
}

sub readSector {
	my $self = shift;
	my $source = shift;
	my $sector = shift;
	
	return $source->data(512 * $sector, 512);	
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