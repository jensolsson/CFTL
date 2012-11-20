#!/usr/bin/perl
#
# Scan.pl
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#


use CFTL::Source;
use strict;


my @sourceHandlers;
my @evidenceSources;

my %flags;
for(0..$#ARGV) {
	$_ = shift @ARGV;
	if(/^\-(v.+)/) {
		for(1..length($1)) {
			$flags{"v" x $_}=1;
		}
	}
	elsif(/^\-(.+)/) {
		$flags{$1}=1;
	}
	else {
		push @ARGV, $_;	
	}
}


print STDERR "CyberForensics TimeLab\n";
print STDERR  "Copyright(C) 2008 Jens Olsson <jens\@rby.se>\n";
print STDERR  "\n";


if(@ARGV < 2) {
	print STDERR  "Usage: Scan.pl [-v[v...]] evidence-path [evidence-path [...]] xml-output-path.xml\n";
	print STDERR  "\n";
	print STDERR  "  -v[v...]	Verbose output\n";
	print STDERR  "\n";
	exit;
}



my $xmlOutputFile = pop @ARGV;

#die "'$xmlOutputFile' already exist, cannot replace existing files as a security precaution." if -e $xmlOutputFile;
die "Filename '$xmlOutputFile' is invalid, must end with .xml." if $xmlOutputFile !~ /\.xml$/i;

foreach(@ARGV) {
	die "'$_' does not exist." unless -e $_;
	push @evidenceSources, Source::new($_, "$_");
}

open(my $xml, ">$xmlOutputFile");

print $xml <<END;
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE EvidenceCollection SYSTEM "CyberForensicsTimeLab.dtd">
<!--
Created by CyberForensics TimeLab.
Copyright(C) 2008 Jens Olsson.
-->
<EvidenceCollection>
END

opendir(my $dh, "CFTL");
	while($_ = readdir($dh)) {
		if(/^(Handler(.+))\.pm$/) {
			print STDERR "Loading handler module for $2...\n" if $flags{'v'};
			eval "use CFTL::$1;";
			die $@ if $@;
			push @sourceHandlers, eval "$1::new();";
			die $@ if $@;
		}
	}
	print STDERR "\n" if $flags{'v'};
closedir($dh);


my $evidenceCount = 0;
my %summary;

foreach my $evidence (@evidenceSources) {
	print STDERR "Evidence $evidence->{'_title'}\n" if $flags{'vv'};
	
		
	foreach my $handler (@sourceHandlers) {

		if($handler->validate($evidence)) {
			$summary{ref($handler)}++;
			print STDERR "\t".(ref $handler)."\n" if $flags{'vvv'};
			
			my $type = ref $handler;
			$type =~ s/^Handler//;
			$evidence->{'_type'} = $type;

			my @subEvidence = $handler->extract($evidence);
			
			$evidence->writeXml($xml);
			
			push @evidenceSources, @subEvidence if scalar(@subEvidence) > 0;
			last;
		}
		elsif($flags{'vvvv'}) {
			print STDERR "\t".(ref $handler)." (Cant handle)\n";
		}

	}

	print STDERR "." if !$flags{'vv'} && !$flags{'q'};
	
	print STDERR "\n" if $flags{'vvv'};
	$evidenceCount++;
}

print $xml "</EvidenceCollection>\n";
close $xml;

print("\n\n*** Summary ***\n");
foreach(keys %summary) {
	print("$_: $summary{$_}\n");	
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