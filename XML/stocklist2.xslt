<?xml version="1.0"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

    <!-- Matches Item Code -->
	<xsl:template match="/">
		<html>
			<head>
				<title>Wood Stocks - Invintory (Sorted)</title>
				<link rel="stylesheet" href="stocklist2.css"/>
				
			</head>
		<body>
      <!--Set font and background variables -->
			<xsl:variable name="backgroundRed" select="'#FFBBBB'"/>
			<xsl:variable name="backgroundOrange" select="'#ffd7ad'"/>
      		<xsl:variable name="backgroundYellow" select="'#fcf7bd'"/>
			<xsl:variable name="fontRed" select="'#DD0000'"/>
			<table>
			<tr>
			<td>Wood Stocks - Invintory (Sorted)</td>
			</tr>
			</table>
			<stockTable>
			<!--Setup Table Header-->
			<row>
				<headerItem><xsl:value-of select="/stockTable/headers/headerItem[1]"/></headerItem>
				<headerItem><xsl:value-of select="/stockTable/headers/headerItem[2]"/></headerItem>
				<headerItem><xsl:value-of select="/stockTable/headers/headerItem[3]"/></headerItem>
				<headerItem><xsl:value-of select="/stockTable/headers/headerItem[4]"/></headerItem>
			</row>

			<!--Loop Through XML Stock Items-->
			<xsl:for-each select="/stockTable/row">
			
			<xsl:sort select="currentCount" data-type="number" order="ascending"/>
			<xsl:sort select="onOrder" order="ascending"/>
				<row>

					<!--Colour background orange based on if currentCount is 0 and onOrder is 'Yes'-->
					<xsl:choose>
						<xsl:when test="number(currentCount) = 0 and onOrder='Yes'">

							<itemCode style="background-color: {$backgroundOrange}"><xsl:value-of select="itemCode"/></itemCode>
							<itemDescription style="background-color: {$backgroundOrange}"><xsl:value-of select="itemDescription"/></itemDescription>
							<currentCount style="background-color: {$backgroundOrange}"><xsl:value-of select="currentCount"/></currentCount>
							<onOrder style="background-color: {$backgroundOrange}"><xsl:value-of select="onOrder"/></onOrder>
						</xsl:when>
						<xsl:otherwise>

							<!--Colour background and text red based on if currentCount is 0 and onOrder is 'No'-->
							<xsl:choose>
								<xsl:when test="number(currentCount) = 0 and onOrder='No'">

									<itemCode style="background-color: {$backgroundRed}; color: {$fontRed}; font-weight: bold"><xsl:value-of select="itemCode"/></itemCode>
									<itemDescription style="background-color: {$backgroundRed}; color: {$fontRed}; font-weight: bold"><xsl:value-of select="itemDescription"/></itemDescription>
									<currentCount style="background-color: {$backgroundRed}; color: {$fontRed}; font-weight: bold"><xsl:value-of select="currentCount"/></currentCount>
									<onOrder style="background-color: {$backgroundRed}; color: {$fontRed}; font-weight: bold"><xsl:value-of select="onOrder"/></onOrder>
								</xsl:when>
								<xsl:otherwise>

                  <!--Colour background yellow based on if currentCount is > 0 and <4 and onOrder is 'Yes'-->
									<xsl:choose>
									<xsl:when test="number(currentCount) &gt; 0 and number(currentCount) &lt; 4  and onOrder='No'">
										<itemCode style="background-color: {$backgroundYellow};"><xsl:value-of select="itemCode"/></itemCode>
										<itemDescription style="background-color: {$backgroundYellow};"><xsl:value-of select="itemDescription"/></itemDescription>
										<currentCount style="background-color: {$backgroundYellow};"><xsl:value-of select="currentCount"/></currentCount>
										<onOrder style="background-color: {$backgroundYellow}"><xsl:value-of select="onOrder"/></onOrder>
									</xsl:when>
                    
                  <!--Colour background grey for standard display-->
									<xsl:otherwise>
										<itemCode><xsl:value-of select="itemCode"/></itemCode>
										<itemDescription><xsl:value-of select="itemDescription"/></itemDescription>
										<currentCount><xsl:value-of select="currentCount"/></currentCount>
										<onOrder><xsl:value-of select="onOrder"/></onOrder>

										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
				</row>
			</xsl:for-each>
			</stockTable>
		</body>
		</html>
	</xsl:template>

</xsl:stylesheet>