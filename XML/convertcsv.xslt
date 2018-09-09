<?xml version="1.0"?>

<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <!-- Matches Item Code -->
	<xsl:template match="/">
		<html>
		<head>
			<title>Wood Stocks - Stock Invintory</title>
		</head>
		<body>
			<h1>
				<!--<xsl.value-of select="text()">-->
			</h1>

			<table border="1">
				
				<xsl:for-each select="/root/row">
					<xsl:variable name ="count" select="position()"/>
					<tr>
						<td><xsl:value-of select="/root/row[$count]/itemCode"/></td>
						<td><xsl:value-of select="/root/row[$count]/itemDescription"/></td>
						<td><xsl:value-of select="/root/row[$count]/currentCount"/></td>
						

						<xsl:variable name ="onOrder" select="/root/row[$count]/onOrder"/>

						<xsl:when test="1=1">
							<td bgcolor="#ff00ff">
							
						</xsl:when>
						<xsl:otherwise>
							<td bgcolor="#ffffff">
						</xsl:otherwise>
						
						<td><xsl:value-of select="$onOrder"/></td>
					</tr>

				</xsl:for-each>
				
			</table>
		</body>
		</html>
	</xsl:template>

</xsl:stylesheet>
