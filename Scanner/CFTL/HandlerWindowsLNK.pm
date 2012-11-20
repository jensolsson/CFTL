#
# HandlerWindowsLNK.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#

package HandlerWindowsLNK;

use CFTL::TimeUtils;
use Math::BigInt;
use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(validate extract);

sub new {
	my $self = {
		
	};
	bless $self, "HandlerWindowsLNK";
	return $self;
}

sub extract {
	my $self = shift;
	my $source = shift;
	my @ret = ();

	die "Invalid data" if !$self->validate($source);

	my $flags = unpack("V", $source->data(20, 4));	
	my $create = TimeUtils::WinTime64($source->data(28, 8));
	my $modify = TimeUtils::WinTime64($source->data(36, 8));
	my $access = TimeUtils::WinTime64($source->data(44, 8));
	my $targetFileSize = unpack("V", $source->data(52, 4));

	my $skipLength = ($flags & 0x1) ? unpack("v", $source->data(76, 2)) : 0;
	#$skipLength += 2-($skipLength % 2);
	$skipLength+=2;
	
	(my $locationLength, my $locationFirstOffset, my $locationFlags, my $locationOffsetLocalVolumeInfo, my $locationOffsetLocalBasePath, my $locationNetworkVolumeInfo, my $locationRemainingPathnameOffset) = unpack("V*", $source->data(76+$skipLength, 7*4));
	
	
	my $path = unpack("Z*", $source->data(76+$skipLength+$locationOffsetLocalBasePath, $locationLength-$locationOffsetLocalBasePath));
	my $fileName = unpack("Z*", $source->data(76+$skipLength+$locationRemainingPathnameOffset, $locationLength-$locationRemainingPathnameOffset));
	
	my $xml = $source->{'_timestamps'};
	push(@$xml, "<Timestamp type=\"Target file was created\" value=\"$create\" origin=\"WindowsLNK\" />");
	push(@$xml, "<Timestamp type=\"Target file was last modified\" value=\"$modify\" origin=\"WindowsLNK\" />");
	push(@$xml, "<Timestamp type=\"Target file was last accessed\" value=\"$access\" origin=\"WindowsLNK\" />");
	push(@$xml, "<Data name=\"TargetFileSize\" value=\"$targetFileSize\" />");
	push(@$xml, "<Data name=\"TargetFile\" value=\"$path\" />");
	#todo more data should be output here, there are a lot

	return @ret;
}


sub validate {
	my $self = shift;
	my $source = shift;
	
	return 0 if unpack("V", $source->data(0, 4)) != 0x4C;
			
	return 1;
}


1;