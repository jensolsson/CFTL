#
# TimeUtils.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#
package TimeUtils;

use Exporter;
use Math::BigInt; 
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw();

sub WinTime64 {
	sub daysOfYear {
		my $year = shift;
		my $days = 365;
		$days++ if $year%4==0 && ($year%100!=0 || $year%400==0);
		return $days;
	}
	my $timestamp = Math::BigInt->new("0x".rev(unpack("H*", shift)));

	my @monthdays = (31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
	($timestamp, my $msec) = $timestamp->bdiv(10000000);
	my $usec = $msec->bstr();
	($timestamp, my $sec) = $timestamp->bdiv(60);
	my $second = $sec->bstr();
	($timestamp, my $min) = $timestamp->bdiv(60);
	my $minute = $min->bstr();
	($timestamp, my $hour) = $timestamp->bdiv(24);
	$hour = $hour->bstr();
	my $day = $timestamp->bstr();
	my $year = 1601;
	while($day>=daysOfYear($year)) {
		$day -= daysOfYear($year); 
		$year++;
	}
	$monthdays[1]++ if daysOfYear($year)==366;
	my $month = 1;
	$day++;
	while($day > $monthdays[$month-1]) {
		$day-=$monthdays[$month-1];
		$month++;	
	}
	my $date = sprintf("%04d-%02d-%02d %02d:%02d:%02d.%d", $year, $month, $day, $hour, $minute, $second, $usec);
	return $date;
}

sub UnixTime {
	(my $sec, my $min, my $hour, my $mday, my $mon, my $year, my $wday, my $yday, my $isdst) = localtime(shift);
	return sprintf("%4d-%02d-%02d %02d:%02d:%02d", $year+1900, $mon+1, $mday, $hour, $min, $sec);
}



sub rev {
	my $str = shift;
	my $ret = "";
	for(my $c=length($str)-2;$c>=0;$c-=2) {
		$ret.=substr($str, $c, 2);
	}
	return $ret;
}

1;