<?xml version="1.0"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	
    <!-- Matches Item Code -->
	<xsl:template match="/">
		
		<html>

			<link rel="stylesheet" href="stocklist1.css"/>

			<head>
				<title>Wood Stocks - Invintory</title>
			</head>

			<body>

				<h1 style="font-family: Helvetica; text-indent: 150px; font-size: 160%">Wood Stocks - Invintory</h1>

				<!--Set font and background variables -->
				<xsl:variable name="backgroundRed" select="'#FFBBBB'"/>
				<xsl:variable name="backgroundOrange" select="'#F2B87B'"/>
				<xsl:variable name="backgroundYellow" select="'#F7EF94'"/>
				<xsl:variable name="fontRed" select="'#DD0000'"/>
				
				<table>
				<!--Setup Table Header-->
				<tr>
					<th width="15%">Item Code</th>
					<th width="53%">Item Description</th>
					<th width="17%">Stock Count</th>
					<th width="15%">On Order</th>
				</tr>

				<!--Loop Through XML Stock Items-->
				<xsl:for-each select="/stockTable/row">

					<tr>
						<!--Colour background orange based on if currentCount is 0 and onOrder is 'Yes'-->
						<xsl:choose>
							<xsl:when test="number(currentCount) = 0 and onOrder='No'">

								<td style="background-color: {$backgroundRed}; color: {$fontRed}; font-weight: bold"><xsl:value-of select="itemCode"/></td>
								<td style="background-color: {$backgroundRed}; color: {$fontRed}; font-weight: bold"><xsl:value-of select="itemDescription"/></td>
								<td style="background-color: {$backgroundRed}; color: {$fontRed}; font-weight: bold"><xsl:value-of select="currentCount"/></td>
								<td style="background-color: {$backgroundRed}; color: {$fontRed}; font-weight: bold"><xsl:value-of select="onOrder"/></td>
							</xsl:when>
							<xsl:otherwise>
								<td><xsl:value-of select="itemCode"/></td>
								<td><xsl:value-of select="itemDescription"/></td>
								<td><xsl:value-of select="currentCount"/></td>
								<td><xsl:value-of select="onOrder"/></td>

								</xsl:otherwise>
							</xsl:choose>
					</tr>
				</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>
