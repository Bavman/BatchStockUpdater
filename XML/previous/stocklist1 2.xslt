<?xml version="1.0"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

    <!-- Matches Item Code -->
	<xsl:template match="/">
		<html>
			<head>
				<title>Wood Stocks - Stock Invintory (Sorted)</title>
				<link rel="stylesheet" href="stocklist.css"/>
				<style>
					
					itemCode,itemDescription,currentCount,onOrder {display: table-cell; text-align: center}
					<!--itemCode,itemDescription,currentCount,onOrder {display: table-row; background-color:#FFFF00}-->
					table {font-family: Arial}
					now {display: table-row}
				</style>
			</head>
		<body>

			<h1 style="font-family: Helvetica; text-indent: 60px; font-size: 160%">Wood Stocks - Stock Invintory (Sorted)</h1>

      <!--Set font and background variables -->
			<xsl:variable name="backgroundRed" select="'#FFBBBB'"/>
			<xsl:variable name="backgroundOrange" select="'#f2b87b'"/>
			<xsl:variable name="fontRed" select="'#DD0000'"/>

			<table>

				<!--Setup Table Heade
				<tr>
					<td  style="color: #FF0000"><xsl:value-of select="/table/headers/th[1]"/></td>
					<td><xsl:value-of select="/table/headers/th[2]"/></td>
					<td><xsl:value-of select="/table/headers/th[3]"/></td>
					<td><xsl:value-of select="/table/headers/th[4]"/></td>
				</tr>r-->

				<!--Loop Through XML Stock Items-->
				<xsl:for-each select="/table/row">
				<xsl:sort select="currentCount" data-type="number" order="ascending"/>
					<row>
						<itemCode><xsl:value-of select="itemCode"/></itemCode>
						<itemDescription><xsl:value-of select="itemDescription"/></itemDescription>
						<currentCount><xsl:value-of select="currentCount"/></currentCount>
						<onOrder><xsl:value-of select="onOrder"/></onOrder>
					</row>
				</xsl:for-each>
			</table>
		</body>
		</html>
	</xsl:template>

</xsl:stylesheet>
