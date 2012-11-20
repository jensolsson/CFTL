#
# HandlerWindowsRegistryHive.pm
# CyberForensics TimeLab
# Copyright(C) 2008 Jens Olsson <jens@rby.se>
#

package HandlerWindowsRegistryHive;

use CFTL::TimeUtils;
use Math::BigInt;
use Exporter;
use strict;
our @ISA = qw(Exporter);
our @EXPORT = qw(validate extract);


sub new {
	my $self = {
		
	};
	bless $self, "HandlerWindowsRegistryHive";
	return $self;
}

sub extract {
	my $self = shift;
	my $source = shift;
	my @ret = ();

	print $source->{'_title'}."\n";
	#<STDIN>;

	die "Invalid data" if !$self->validate($source);

	my $index = 1;
	$index++ while -e "debug/registry$index.dat";
	
	open(my $fh, ">debug/registry$index.dat");
	print $fh $source->data(0, $source->size());
	close($fh);

	
	@ret = dir($source, unpack("l", $source->data(0x24, 4)), "/");

	return @ret;
}


sub dir {
	my $source = shift;
	my $pos = shift;
	my $dir = shift;
	
	my @ret = ();
	
	my $size = unpack("l", $source->data(4096+$pos, 4));
	my $blockType = $source->data(4096+$pos+4, 2);
	my $time = TimeUtils::WinTime64($source->data(4096+$pos+8, 8));
	
	#print " ".$time." ($size b)\n";
	


	my ($subkeysCount, $subkeysOffset, $valuesCount, $valuesOffset, $classNameOffset, $nameLength, $classNameLength) = unpack("x24 l x4 l x4 l l x4 l x20 v v", $source->data(4096+$pos, 1000));

	my $name = $source->data(4096+$pos+0x50, $nameLength);

	my $linkstype = $source->data(4096+$subkeysOffset+4, 2);
	#print "  <BEGIN $dir$name subkeys=$subkeysCount subkeysOffset=$subkeysOffset followingLinksType=$linkstype>\n";

	my $timestamps = [
		"<Timestamp type=\"Key was last changed\" value=\"$time\" origin=\"WindowsRegistryHive\" />"
	];
	push @ret, Source::new($source, "$dir$name", "0-0", $source->{'_id'}, $timestamps);
	



	if($linkstype eq "ri") {
		my $subChunks = unpack("v", $source->data(4096+$subkeysOffset+6, 2));

		for(0..($subChunks-1)) {

			my $subchunk = unpack("L", $source->data(4096+$subkeysOffset+8+$_*8, 4));

			my $subkeysRealCount = unpack("v", $source->data(4096+$subchunk+6, 2));


			for(0..($subkeysRealCount-1)) {
				#$_=0; #todo debug?????
				my $key = unpack("L", $source->data(4096+$subchunk+8+$_*8, 4));
	
				#print "\n\n[key $key $dir$name]\n";
	
				push @ret, dir($source, $key, $dir.$name."/");
			}

		}

	}
	elsif($linkstype eq "lh" || $linkstype eq "n\x00") {
		for(0..($subkeysCount-1)) {
			my $key = unpack("L", $source->data(4096+$subkeysOffset+8+$_*8, 4));

			#print "\n\n[key $key $dir$name]\n";

			push @ret, dir($source, $key, $dir.$name."/");	
		}
	}
	else {
		#print "\n>>>$linkstype\n";
		#<STDIN>;		
	}

	#print "  <END $dir$name>\n";
	return @ret;
}


sub validate {
	my $self = shift;
	my $source = shift;
	
	return 0 if $source->data(0, 4) ne "regf";

	return 0 if $source->data(4096, 4) ne "hbin";
	
	return 0 if $source->{'_title'} =~ /userdiff$/i;

	return 1;
}

1;