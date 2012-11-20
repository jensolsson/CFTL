#
# HandlerJPEG.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#

package HandlerJPEG;

use Date::Parse;
use Math::BigInt; 
use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(validate extract);


sub new {
	my $self = {
		
	};
	bless $self, "HandlerJPEG";
	return $self;
}

sub extract {
	my $self = shift;
	my $source = shift;
	my @ret = ();

	my $data = $source->data(0, $source->size());
	
	$source->seek(0);
	
	die "Invalid beginning of jpeg file, expected FFD8" unless $source->read(2) eq "\xFF\xD8";

	my $exif;
	my $exifPos;
	my $marker;
	my $pos = 2;
	do {
		($marker, my $length) = unpack("x C n", $source->read(4));
		print(unpack("H*", chr($marker))."-$length\n");
		my $chunk = $source->read($length-2);
		if($marker == 0xe1) {
			$exif = $chunk;
			$exifPos = $pos + 4;	
		}

		$pos += 2 + $length;
	} while($marker != 0xDA);
	
	
	
	my $pos = 0;
	if(substr($exif, $pos, 6) ne "Exif\x00\x00") {
		warn "Exif signature is invalid";
		return;
	}
	$pos+=6;
	
	my $i16;
	my $i32;
	if(substr($exif, $pos, 2) eq "II") {
		$i16 = "v";
		$i32 = "V";
	}
	else {
		$i16 = "n";
		$i32 = "N";
	}
	$pos+=2;
	
	die "Invalid integer according to architecture" if(unpack("$i16", substr($exif, $pos, 2)) != 0x2A);
	$pos+=2;
	
	die "Invalid offset" if(unpack("$i32", substr($exif, $pos, 4)) != 0x08);
	$pos+=4;

	my $offsetNext;
	do {
		print STDERR "J";
		
		my $files = unpack("$i16", substr($exif, $pos, 2));
		$pos+=2;
		print "files=".$files."\n";
		last unless $files;
		
		
		for(1..($files)) {
			print STDERR "P";

			my $entry = substr($exif, $pos, 12);
			my $tagnr = unpack("$i16", substr($entry, 0, 2));
			my $format = unpack("$i16", substr($entry, 2, 2));
			my $components = unpack("$i32", substr($entry, 4, 4));
			my $value = unpack("$i32", substr($entry, 8, 4));


			$offsetNext = $value if $tagnr==0x8769;

			print "entry:  ".unpack("H*", substr($entry, 0, 2))."\n";
			print "format: ".unpack("H*", substr($entry, 2, 2))."\n";
			print "components: ".unpack("H*", substr($entry, 4, 4))."\n";
			print "data: ".unpack("H*", substr($entry, 8, 4))."\n";
			print "\n";
			$pos+=12;
		}
		
		
		my $next = unpack("$i32", substr($exif, $pos, 4));
		$pos = $offsetNext - $exifPos;
		die $pos;
	} while(1);
	
	
	#print "\nnext: ".unpack("H*", substr($exif, $pos, 0, 16))."\n";
	

	
	return @ret;
}


sub validate {
	my $self = shift;
	my $source = shift;
	
	return 0 if $source->data(0, 2) ne "\xFF\xD8";

	return 1;
}

1;