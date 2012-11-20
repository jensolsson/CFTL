#
# HandlerMBR.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#

package HandlerMBR;

use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(validate extract);

sub new {
	my $self = {
		
	};
	bless $self, "HandlerMBR";
	return $self;
}

sub extract {
	my $self = shift;
	my $source = shift;
	my @ret = ();

	die "Invalid data" if !$self->validate($source);


	for(0..3) {
		my $pos = 446 + $_ * 16;
		my ($status, $type, $start, $length) = unpack("C x3 C x3 L L", $source->data($pos, 16));
		next if $type == 0x0 || $length == 0 || $start == 0;
		push @ret, Source::new($source, "Partition($_)", ($start*512)."-".(($start+$length)*512), $source->{'_id'});
	}


	return @ret;
}

sub validate {
	my $self = shift;
	my $source = shift;
	
	return 0 if unpack("v", $source->data(510, 2)) != 0xAA55;

	for(my $pos = 446; $pos < 510; $pos +=16) {
		my ($status, $type, $start, $length) = unpack("C x3 C x3 L L", $source->data($pos, 16));
		return 0 if $status != 0x80 && $status != 0x0;
	}
	
	return 1;
}

1;