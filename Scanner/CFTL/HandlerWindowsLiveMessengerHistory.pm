#
# HandlerWindowsLiveMessengerHistory.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#

package HandlerWindowsLiveMessengerHistory;

use HTML::Entities qw(encode_entities);
use Math::BigInt; 
use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(validate extract);


sub new {
	my $self = {
		
	};
	bless $self, "HandlerWindowsLiveMessengerHistory";
	return $self;
}

sub extract {
	my $self = shift;
	my $source = shift;
	my @ret = ();
	
	die "Invalid data" if !$self->validate($source);
	
	
	$_ = $source->data(0, $source->size());
	
	while(/<Message Date="[^"]+" Time="[^"]+" DateTime="([^"]+)" SessionID="1"><From><User FriendlyName="([^"]+)"\/><\/From><To><User FriendlyName="([^"]+)"\/><\/To><Text Style="[^"]+">([^<]+)<\/Text><\/Message>/g) {
		
		my $timestamp = $1;
		{
			$timestamp =~ s/T/ /;
			$timestamp =~ s/Z//;
		}
		
		
		my $from = $2;
		my $to = $3;
		my $message = $4;
		
		$from =~ s/[\x00-\x1f]/?/g;
    	$from = encode_entities($from);

		$to =~ s/[\x00-\x1f]/?/g;
    	$to = encode_entities($to);

		$message =~ s/[\x00-\x1f]/?/g;
    	$message = encode_entities($message);

		
		my $timestamps = [
			"<Timestamp type=\"Message was sent\" value=\"$timestamp\" origin=\"WindowsLiveMessengerHistory\" />",
			"<Data name=\"From\" value=\"$from\" />",
			"<Data name=\"To\" value=\"$to\" />",
			"<Data name=\"Message\" value=\"$message\" />"
		];
		push @ret, Source::new($source, "MSN Message from $2 to $3 on $timestamp", "0-0", $source->{'_id'}, $timestamps);
	}

	
	
	return @ret;
}


sub validate {
	my $self = shift;
	my $source = shift;
	
	my $idString = "<?xml version=\"1.0\"?>\r\n<?xml-stylesheet type='text/xsl' href='MessageLog.xsl'?>\r\n";
	return 0 if $source->data(0, length($idString)) ne $idString;

	return 1;
}

1;