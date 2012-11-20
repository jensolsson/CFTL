<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">


<xsl:template match="EvidenceCollection">
	<h1>EvideceCollection</h1>
    <xsl:for-each select="Evidence">
<table bgcolor="#eeeeee" style="Border: 1px solid #000000;">
	<tr>
		<th align="left" colspan="2" bgcolor="#dddddd"><xsl:value-of select="@title"/></th>
	</tr>
	<tr><td colspan="2"><small>Id <xsl:value-of select="@id"/></small></td></tr>
	<tr><td colspan="2"><small>Parent <xsl:value-of select="@parent"/></small></td></tr>

	    <xsl:for-each select="Chunk">
	<tr>
		<tr><td colspan="2"><small><b>CHUNK: </b> <xsl:value-of select="@from"/> - <xsl:value-of select="@to"/></small></td></tr>
	</tr>
	    </xsl:for-each>
    
	    <xsl:for-each select="Timestamp">
	<tr>
		<td width="200"><xsl:value-of select="@value"/></td>
		<td><xsl:value-of select="@type"/></td>
	</tr>
	    </xsl:for-each>
	    
	    <xsl:for-each select="Data">
	<tr>
		<td width="200"><font color="#ff0000"><b><xsl:value-of select="@name"/></b></font></td>
		<td><xsl:value-of select="@value"/></td>
	</tr>
	    </xsl:for-each>
</table><br />
    </xsl:for-each>
</xsl:template>





</xsl:stylesheet>



