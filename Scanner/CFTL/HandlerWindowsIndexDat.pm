#
# HandlerWindowsIndexDat.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#

package HandlerWindowsIndexDat;

use HTML::Entities qw(encode_entities);
use CFTL::TimeUtils;
use Math::BigInt; 
use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(validate extract);


sub new {
	my $self = {
		
	};
	bless $self, "HandlerWindowsIndexDat";
	return $self;
}

sub extract {
	my $self = shift;
	my $source = shift;
	my @ret = ();
	
	die "Invalid data" if !$self->validate($source);
	
	$source->seek(0x5000);

	my $blockData = 1;
	
	while($blockData) {
		my $blockType = $source->read(4);	
		my $blockSize = unpack("L", $source->read(4));

		last if $blockSize == 0xBADF00D;
		warn "Negative length" and last if $blockSize*0x80-8 < 0;
		$blockData = $source->read($blockSize*0x80-8);
		next if $blockType ne "URL\ " && $blockType ne "LEAK";


		my $modify = TimeUtils::WinTime64(substr($blockData, 0, 8));
		my $visited = TimeUtils::WinTime64(substr($blockData, 8, 8));
		
		#my $urlOffset = unpack("L", substr($blockData, 52, 4));
		#my $url = unpack("Z*", substr($blockData, $urlOffset-8));

		my $url = unpack("Z*", substr($blockData, 96));
		$url =~ s/[\x00-\x1f]/?/g;
    	$url = encode_entities($url);
		
		my $timestamps = [
			"<Timestamp type=\"Page was modified\" value=\"$modify\" origin=\"WindowsIndexDat\" />",
			"<Timestamp type=\"Page was visited\" value=\"$visited\" origin=\"WindowsIndexDat\" />",
			"<Data name=\"URL\" value=\"$url\" />"
		];
		push @ret, Source::new($source, "$url", "0-0", $source->{'_id'}, $timestamps);
	}
	
	
	return @ret;
}



sub validate {
	my $self = shift;
	my $source = shift;
	
	return 0 if $source->data(0, 28) ne "Client UrlCache MMF Ver 5.2\x00";

	return 1;
}

1;