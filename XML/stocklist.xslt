<?xml version="1.0"?>

<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:func="http://exslt.org/functions">
    <!-- Matches Item Code -->
	<xsl:template match="/">
		<html>
		<head>
			<title>Wood Stocks - Stock Invintory</title>
			<link rel="stylesheet" href="stocklist.css"/>
		</head>
		<body>
			<h1>
				<!--<xsl.value-of select="text()">-->
			</h1>

			<!-- VARIABLES -->
			<xsl:variable name="rowPath" select="/table/row">
			</xsl:variable>

			<!-- FUNCTION -->

			<func:function name="func:displayStock">
				<!-- <xsl:param name="test"/>-->
				<table>
				<xsl:for-each select="/table/row">
				<xsl:variable name ="count" select="position()"/>
					<tr>
						<itemCode><xsl:value-of select="/table/row[$count]/itemCode"/></itemCode>
						<itemDescription><xsl:value-of select="/table/row[$count]/itemDescription"/></itemDescription>

						<xsl:variable name ="currentCount" select="/table/row[$count]/currentCount"/>
						<currentCount><xsl:value-of select="currentCount"/></currentCount>
						
						<xsl:variable name ="onOrder" select="/table/row[$count]/onOrder"/>
						
						<!--Colour onOrder background green based on if currentCount is 0 and onOder is 'Yes'-->
						<xsl:choose>
							<xsl:when test="currentCount='0' and onOrder='Yes'">
								<onOrder style="background-color: #BBFFBB"><xsl:value-of select="$onOrder"/></onOrder>
							</xsl:when>
							<xsl:otherwise>
									<!--Colour onOrder background red based on if currentCount is 0 and onOder is 'Yes'-->
								<xsl:choose>
									<xsl:when test="currentCount='0' and onOrder='No'">
										<onOrder style="background-color: #FFBBBB; color: #FF0000"><xsl:value-of select="$onOrder"/></onOrder>
									</xsl:when>
									<xsl:otherwise>
										<onOrder bgcolor="#ffffff"><xsl:value-of select="$onOrder"/></onOrder>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</tr>
				</xsl:for-each>
				</table>
			</func:function>

			<h1 xPath="func:displayStock()">
				<!--<xsl.value-of select="text()">-->
			</h1>
		</body>
		</html>
	</xsl:template>

	

</xsl:stylesheet>
