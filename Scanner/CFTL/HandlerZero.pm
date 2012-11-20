#
# HandlerZero.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#

package HandlerZero;

use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(validate extract);

sub new {
	my $self = {
		
	};
	bless $self, "HandlerZero";
	return $self;
}

sub extract {
	my $self = shift;
	my $source = shift;
	my @ret = ();

	die "Invalid data" if !$self->validate($source);

	return @ret;
}

sub validate {
	my $self = shift;
	my $source = shift;

	return 1;
}

1;