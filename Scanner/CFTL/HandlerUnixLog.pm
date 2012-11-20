#
# HandlerUnixLog.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#

package HandlerUnixLog;

use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(validate extract);

sub new {
	my $self = {
		
	};
	bless $self, "HandlerUnixLog";
	return $self;
}

sub extract {
	my $self = shift;
	my $source = shift;
	my @ret = ();

	die "Invalid data" if !$self->validate($source);

	$_ = $source->data(0, $source->size());
	@_ = split /[\r\n]+/;
	
	foreach(@_) {
		if(/^(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Nov|Dec) ( [0-9]{1}|[0-9]{2}) ([0-9]{2}:[0-9]{2}:[0-9]{2}) (.+)/) {
			print "\n$1 $2 $3";
			print "\n$4\n";
		}
	}

	return @ret;
}

sub validate {
	my $self = shift;
	my $source = shift;

	$_ = $source->data(0, (1024<$source->size())?1024:$source->size());
	@_ = split /[\r\n]+/;

	my $lineCount = 0;
	foreach(@_) {
		$lineCount++;
		return 0 unless /^(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Nov|Dec) ( [0-9]{1}|[0-9]{2}) [0-9]{2}:[0-9]{2}:[0-9]{2}/;	
	}

	return 1 if $lineCount > 0;
	
	return 0;
}

1;