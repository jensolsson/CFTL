#
# Source.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#

package Source;

use HTML::Entities qw(encode_entities);
use List::Util qw[min max];
use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(data writeXml read seek);

my $id = 0;

sub new {
	my $self = {
		_title		=> "Untitled",
		_id			=> ++$id,
		_desc		=> "",
		_pos		=> 0
	};
	
	$self->{'_source'} = shift || die "No source specified";
	$self->{'_title'} = shift || "Unknown";
	
	#$self->{'_title'} =~ s/[^a-z0-9 \.,_\-\\\/]/?/gi;
	
	$self->{'_title'} =~ s/[\x00-\x1f]/?/g;
    $self->{'_title'} = encode_entities($self->{'_title'});

	my $max = (ref $self->{'_source'} eq "") ? (-s $self->{'_source'}) : $self->{'_source'}->size();
	$self->{'_chunks'} = shift || "0-".$max; 

	$self->{'_parent'} = shift || "";

	$self->{'_timestamps'} = shift || 0;


	if(ref $self->{'_source'} eq "") {
		open $self->{'_file'}, "<".$self->{'_source'} or die "Could not open file $self->{'_source'}";
		binmode $self->{'_file'};
	}
		
	bless $self, 'Source';
	return $self;
}

sub DESTROY {
	my ($self) = shift;	
	if(ref $self->{'_source'} eq "") {	
		close $self->{'_file'};
	}
}

sub writeXml {
	my $self = shift;
	my $xml = shift;
	
	print $xml "\t<Evidence title =\"$self->{'_title'}\" type=\"$self->{'_type'}\" id=\"$self->{'_id'}\" parent=\"$self->{'_parent'}\">\n";
	foreach(split(/,/, $self->{'_chunks'})) {
		my ($from, $to) = split /\-/;
		print $xml "\t\t<Chunk from=\"$from\" to=\"$to\" />\n";		
	}
	if(ref $self->{'_timestamps'} eq "ARRAY") { 
		my $timestamps = $self->{'_timestamps'};
		foreach(@$timestamps) {
			print $xml "\t\t$_\n";		
		}
	}
	print $xml "\t</Evidence>\n";
}

sub data {
	my $self = shift;
	my $start = shift;
	my $length = shift;
	my $data = "";

	while($self->{'_chunks'} =~ /([0-9]+).+?([0-9]+)/g) {
		my ($chunkStart, $chunkEnd, $chunkLength) = ($1, $2, $2-$1);
		if($start >= $chunkLength) {
			$start -= $chunkLength;
		}
		else {
			my $amount = min($length, $chunkLength - $start);
			my $buffer;
			if(ref $self->{'_source'} eq "") {	
				seek $self->{'_file'}, $chunkStart + $start, 0;
				read $self->{'_file'}, $buffer, $amount;
			}
			elsif(ref $self->{'_source'} eq "Source") {
				$buffer = $self->{'_source'}->data($chunkStart + $start, $amount);
			}
			$data .= $buffer;
			$length -= $amount;
		}
	}
	
	return $data;
}

sub size {
	my $self = shift;
	my $size = 0;
	while($self->{'_chunks'} =~ /([0-9]+).+?([0-9]+)/g) {
		my $chunkLength = $2-$1;
		$size += $chunkLength;
	}
	return $size;	
}

sub seek {
	my $self = shift;
	$self->{'pos'} = shift;
}

sub read {
	my $self = shift;
	my $size = shift;
	my $buffer = $self->data($self->{'pos'}, $size);
	$self->{'pos'}+=$size;
	return $buffer;
}



1;