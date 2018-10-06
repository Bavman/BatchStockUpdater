<?xml version="1.0"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

    <!-- Matches Item Code -->
	<xsl:template match="/">

		<html>

			<link rel="stylesheet" href="stocklist2.css"/>

			<head>
				<title>Wood Stocks - Invintory (Sorted)</title>
			</head>

			<body>

				<!--Set font and background variables -->
				<xsl:variable name="backgroundRed" select="'#FFBBBB'"/>
				<xsl:variable name="backgroundOrange" select="'#ffd7ad'"/>
				<xsl:variable name="backgroundYellow" select="'#fcf7bd'"/>
				<xsl:variable name="fontRed" select="'#DD0000'"/>
				
				<table>
					<tr>
						<th>Wood Stocks - Invintory (Sorted)</th>
					</tr>
				</table>

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
					
					<xsl:sort select="currentCount" data-type="number" order="ascending"/>
					<xsl:sort select="onOrder" order="ascending"/>
						<tr>

							<!--Colour background orange based on if currentCount is 0 and onOrder is 'Yes'-->
							<xsl:choose>
								<xsl:when test="number(currentCount) = 0 and onOrder='Yes'">

									<td style="background-color: {$backgroundOrange}"><xsl:value-of select="itemCode"/></td>
									<td style="background-color: {$backgroundOrange}"><xsl:value-of select="itemDescription"/></td>
									<td style="background-color: {$backgroundOrange}"><xsl:value-of select="currentCount"/></td>
									<td style="background-color: {$backgroundOrange}"><xsl:value-of select="onOrder"/></td>
								</xsl:when>
								<xsl:otherwise>

									<!--Colour background and text red based on if currentCount is 0 and onOrder is 'No'-->
									<xsl:choose>
										<xsl:when test="number(currentCount) = 0 and onOrder='No'">

											<td style="background-color: {$backgroundRed}; color: {$fontRed}; font-weight: bold"><xsl:value-of select="itemCode"/></td>
											<td style="background-color: {$backgroundRed}; color: {$fontRed}; font-weight: bold"><xsl:value-of select="itemDescription"/></td>
											<td style="background-color: {$backgroundRed}; color: {$fontRed}; font-weight: bold"><xsl:value-of select="currentCount"/></td>
											<td style="background-color: {$backgroundRed}; color: {$fontRed}; font-weight: bold"><xsl:value-of select="onOrder"/></td>

										</xsl:when>
										<xsl:otherwise>

						<!--Colour background yellow based on if currentCount is > 0 and <4 and onOrder is 'Yes'-->
											<xsl:choose>
											<xsl:when test="number(currentCount) &gt; 0 and number(currentCount) &lt; 4  and onOrder='No'">
												<td style="background-color: {$backgroundYellow};"><xsl:value-of select="itemCode"/></td>
												<td style="background-color: {$backgroundYellow};"><xsl:value-of select="itemDescription"/></td>
												<td style="background-color: {$backgroundYellow};"><xsl:value-of select="currentCount"/></td>
												<td style="background-color: {$backgroundYellow}"><xsl:value-of select="onOrder"/></td>
											</xsl:when>
							
						<!--Colour background grey for standard display-->
											<xsl:otherwise>
												<td><xsl:value-of select="itemCode"/></td>
												<td><xsl:value-of select="itemDescription"/></td>
												<td><xsl:value-of select="currentCount"/></td>
												<td><xsl:value-of select="onOrder"/></td>

												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>
