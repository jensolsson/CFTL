#
# HandlerWindowsEventLog.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#

package HandlerWindowsEventLog;

use HTML::Entities qw(encode_entities);
use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(validate extract);

sub new {
	my $self = {
		
	};
	bless $self, "HandlerWindowsEventLog";
	return $self;
}

sub extract {
	my $self = shift;
	my $source = shift;
	my @ret = ();

	die "Invalid data" if !$self->validate($source);

	
	my $eventLogHeader = $source->data(0, 12*4);
	my ($headerSize, $signature, $majorVersion, $minorVersion, $startOffset, $endOffset, $currentRecordNumber, $oldestRecordNumber, $maxSize, $flags, $retention, $endHeaderSize) = unpack("V12", $eventLogHeader);

	my $pos = $startOffset;
	$source->seek($pos);
	
	for(;;) {
		my $eventLogRecord = $source->read(6*4+4*2+6*4);
		my ($length, $reserved, $recordNumber, $timeGenerated, $timeWritten, $eventId, $eventType, $numStrings, $eventCategory, $reservedFlags, $closingRecordNumber, $stringOffset, $userSidLength, $userSidOffset, $dataLength, $dataOffset) = unpack("V6 v4 V6", $eventLogRecord);
  
  		my $sourceName = readString($source);
		my $computerName = readString($source);

		my @strings;
		$source->seek($pos+$stringOffset);
		for(my $stringId=0;$stringId<$numStrings;$stringId++) {
			push @strings, readString($source);
		}
		
		my $userSid;
		if($userSidLength!=0) {
			$source->seek($pos+$userSidOffset);			
			$userSid=$source->read($userSidLength);
			$userSid =~ s/(.)/unpack("H4", $1)/ges;		
		}
		
		my $data;
		if($dataLength!=0) {
			$source->seek($pos+$dataOffset);		
			$data = $source->read($dataLength);
			$data =~ s/(.)/unpack("H4", $1)/ges;
		}


		$recordNumber =~ s/[\x00-\x1f]/?/g;
    	$recordNumber = encode_entities($recordNumber);

		$eventId =~ s/[\x00-\x1f]/?/g;
    	$eventId = encode_entities($eventId);
    	
		$eventType =~ s/[\x00-\x1f]/?/g;
    	$eventType = encode_entities($eventType);
    	
		$numStrings =~ s/[\x00-\x1f]/?/g;
    	$numStrings = encode_entities($numStrings);
    	
		$eventCategory =~ s/[\x00-\x1f]/?/g;
    	$eventCategory = encode_entities($eventCategory);
    	
		$reservedFlags =~ s/[\x00-\x1f]/?/g;
    	$reservedFlags = encode_entities($reservedFlags);
    	
		$closingRecordNumber =~ s/[\x00-\x1f]/?/g;
    	$closingRecordNumber = encode_entities($closingRecordNumber);
    	
		$stringOffset =~ s/[\x00-\x1f]/?/g;
    	$stringOffset = encode_entities($stringOffset);
    	
		$userSidLength =~ s/[\x00-\x1f]/?/g;
    	$userSidLength = encode_entities($userSidLength);
    	
		$userSidOffset =~ s/[\x00-\x1f]/?/g;
    	$userSidOffset = encode_entities($userSidOffset);
    	
		$dataLength =~ s/[\x00-\x1f]/?/g;
    	$dataLength = encode_entities($dataLength);
    	
		$dataOffset =~ s/[\x00-\x1f]/?/g;
    	$dataOffset = encode_entities($dataOffset);   
    	 	
		$sourceName =~ s/[\x00-\x1f]/?/g;
    	$sourceName = encode_entities($sourceName); 
    	   	
		$computerName =~ s/[\x00-\x1f]/?/g;
    	$computerName = encode_entities($computerName); 
    	   	
		$userSid =~ s/[\x00-\x1f]/?/g;
    	$userSid = encode_entities($userSid); 
    	   	
		$data =~ s/[\x00-\x1f]/?/g;
    	$data = encode_entities($data); 
    	   	    	
		my $xml = [
			"<Timestamp type=\"Entry was generated\" value=\"".gmtimetostd($timeGenerated)."\" origin=\"WindowsEventLog\" />",
			"<Timestamp type=\"Entry was written\" value=\"".gmtimetostd($timeWritten)."\" origin=\"WindowsEventLog\" />",
			"<Data name=\"RecordNumber\" value=\"$recordNumber\" />",
			"<Data name=\"EventId\" value=\"$eventId\" />",
			"<Data name=\"EventType\" value=\"$eventType\" />",
			"<Data name=\"NumStrings\" value=\"$numStrings\" />",
			"<Data name=\"EventCategory\" value=\"$eventCategory\" />",
			"<Data name=\"ReservedFlags\" value=\"$reservedFlags\" />",
			"<Data name=\"ClosingRecordNumber\" value=\"$closingRecordNumber\" />",
			"<Data name=\"StringOffset\" value=\"$stringOffset\" />",
			"<Data name=\"UserSidLength\" value=\"$userSidLength\" />",
			"<Data name=\"UserSidOffset\" value=\"$userSidOffset\" />",
			"<Data name=\"DataLength\" value=\"$dataLength\" />",
			"<Data name=\"DataOffset\" value=\"$dataOffset\" />",
			"<Data name=\"SourceName\" value=\"$sourceName\" />",
			"<Data name=\"ComputerName\" value=\"$computerName\" />",
			"<Data name=\"UserSid\" value=\"$userSid\" />",
			"<Data name=\"Data\" value=\"$data\" />"
		];

		
		foreach(0..$#strings) {
			my $value = $strings[$_];
			$value =~ s/[\x00-\x1f]/?/g;
    		$value = encode_entities($value); 			
			push(@$xml, "<Data name=\"String$_\" value=\"$value\" />");
		}
		
		#todo fix the followin data area
		push @ret, Source::new($source, "ID $eventId", "0-0", $source->{'_id'}, $xml);
		#print "$pos-".($pos+$length)." $endOffset\n";
		
		$pos+=$length;
		last if $pos>=$endOffset;
		$source->seek($pos);
	}

	return @ret;
}

sub gmtimetostd {
	my ($second, $minute, $hour, $day, $month, $year) = gmtime(shift);
	return sprintf("%04d-%02d-%02d %02d:%02d:%02d", $year+1900, $month+1, $day, $hour, $minute, $second);
}

sub readString {
	my $source = shift;
	my $char, my $string;
	for(;;) {
		my $char = $source->read(2);
		last if $char eq "\0\0";
		$string.=$char;
	};
	return $string;	
}

sub validate {
	my $self = shift;
	my $source = shift;
	
	return 0 if $source->data(4, 4) ne "LfLe";
		
	return 1;
}

1;