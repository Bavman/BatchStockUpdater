<?xml version="1.0"?>

<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:fn="http://xmlns.opentechnology.org/xslt-extensions/common">
    <!-- Matches Item Code -->
	<xsl:template match="/">
		<html>
		<head>
			<title>Wood Stocks - Stock Invintory</title>
			<link rel="stylesheet" href="stocklist.css"/>
		</head>
		<body>
			<h1 style="font-family: Helvetica, sans-serif; text-indent: 90px;">
				Wood Stocks - Stock Invintory
			</h1>
				<table>

					<!--Setup Table Header-->
					<tr>
						<th><xsl:value-of select="/table/header/th[1]"/></th>
						<th><xsl:value-of select="/table/header/th[2]"/></th>
						<th><xsl:value-of select="/table/header/th[3]"/></th>
						<th><xsl:value-of select="/table/header/th[4]"/></th>
					</tr>

					<!--Loop Through XML Stock Items-->
					<xsl:for-each select="/table/row">
					<xsl:sort select="currentCount" data-type="number" order="ascending"/>
						<tr>

							<!--Colour background orange based on if currentCount is 0 and onOder is 'Yes'-->
							<xsl:choose>
								<xsl:when test="currentCount='0' and onOrder='Yes'">
									<itemCode style="background-color: #f2b87b"><xsl:value-of select="itemCode"/></itemCode>
									<itemDescription style="background-color: #f2b87b"><xsl:value-of select="itemDescription"/></itemDescription>
									<currentCount style="background-color: #f2b87b"><xsl:value-of select="currentCount"/></currentCount>
									<onOrder style="background-color: #f2b87b"><xsl:value-of select="onOrder"/></onOrder>
								</xsl:when>
								<xsl:otherwise>

									<!--Colour background and text red based on if currentCount is 0 and onOder is 'No'-->
									<xsl:choose>
										<xsl:when test="currentCount='0' and onOrder='No'">
											<itemCode style="background-color: #FFBBBB; color: #DD0000"><xsl:value-of select="itemCode"/></itemCode>
											<itemDescription style="background-color: #FFBBBB; color: #DD0000"><xsl:value-of select="itemDescription"/></itemDescription>
											<currentCount style="background-color: #FFBBBB; color: #DD0000"><xsl:value-of select="currentCount"/></currentCount>
											<onOrder style="background-color: #FFBBBB; color: #DD0000"><xsl:value-of select="onOrder"/></onOrder>
										</xsl:when>
										<xsl:otherwise>

											<!--Colour background grey for standard display-->
											<itemCode><xsl:value-of select="itemCode"/></itemCode>
											<itemDescription><xsl:value-of select="itemDescription"/></itemDescription>
											<currentCount><xsl:value-of select="currentCount"/></currentCount>
											<onOrder><xsl:value-of select="onOrder"/></onOrder>
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
