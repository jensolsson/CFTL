#!/usr/bin/perl
#
# CreateHTML.pl
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#

use strict;

my $fileName = shift @ARGV or die "Usage: CreateHTML.pl fileName\n";

open(my $fh, "<$fileName");
read($fh, local $_, -s "$fileName");
close($fh);

if(/<EvidenceCollection>(.+?)<\/EvidenceCollection>/s) {
	my $evidenceCollection = $1;
	while($evidenceCollection =~ /<Evidence[^>]*>(.+?)<\/Evidence>/sg) {
		my $evidenceData = $1;
		while($evidenceData =~ /<Timestamp type="([^"]+)" value="([^"]+)" \/>/sg) {
			print "<tr><td>$2</td><td>$1</td></tr>\n";
		}
	}
	
}