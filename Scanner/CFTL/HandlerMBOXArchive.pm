#
# HandlerMBOXArchive.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#

package HandlerMBOXArchive;

use HTML::Entities qw(encode_entities);
use Date::Parse;
use Math::BigInt; 
use CFTL::TimeUtils;
use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(validate extract);


sub new {
	my $self = {
		
	};
	bless $self, "HandlerMBOXArchive";
	return $self;
}

sub extract {
	my $self = shift;
	my $source = shift;
	my @ret = ();

	my $data = $source->data(0, $source->size());
	$data =~ s/\r//g;
	
	my @messages = split(/[\n\r]+From /, $data);
	
	foreach(@messages) {
		my @timestamps;
		my $id;
		my $subject;
		my $from;
		my $to;
		
		if(/^\- (.+)/m) {
			push @timestamps, "<Timestamp type=\"Message stored at final destination\" value=\"".TimeUtils::UnixTime(str2time($1))."\" origin=\"MBOXArchive\" />";
		}
		if(/^From: (.+)/m) {
			$from = $1;	
		}
		if(/^To: (.+)/m) {
			$to = $1;	
		}
		if(/^Subject: (.+)/m) {
			$subject = $1;	
		}
		if(/^Message-ID: (.+)/m) {
			$id = $1;	
		}
		if(/^Date: (.+)/m) {
			push @timestamps, "<Timestamp type=\"Message was sent\" value=\"".TimeUtils::UnixTime(str2time($1))."\" origin=\"MBOXArchive\" />";	
		}
		while(/^Received: (.+)\n(\t.+\n)+/gm) {
			
			my @parts = split(/;/, $2);
			my $date = $parts[$#parts];
			$date =~ s/[\r\n]//g;
			$date =~ s/^[ \t]*(.+?)[ \t]*$/$1/;
			
			push @timestamps, "<Timestamp type=\"Message was received\" value=\"".TimeUtils::UnixTime(str2time($date))."\" origin=\"MBOXArchive\" />";			
		}
		my $title = "$id $subject from $from to $to";
		$title =~ s/[\x00-\x1f]/?/g;
    	$title = encode_entities($title);

		my $fulltext = $_;
		$fulltext =~ s/[\x00-\x1f]/?/g;
    	$fulltext = encode_entities($fulltext);
		push @timestamps, "<Data name=\"Full message text\" value=\"".$fulltext."\" />";		
			
		push @ret, Source::new($source, $title, "0-0", $source->{'_id'}, \@timestamps);		
	}
	print $#messages;

	die "Invalid data" if !$self->validate($source);
	
	return @ret;
}


sub validate {
	my $self = shift;
	my $source = shift;
	
	return 0 if $source->data(0, 5) !~ /^From /;

	return 1;
}

1;