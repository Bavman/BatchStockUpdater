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
			<h1>
				<!--<xsl.value-of select="text()">-->
			</h1>

			<!-- VARIABLES -->
			<xsl:variable name="rowPath" select="/table/row"></xsl:variable>
      
			<!-- FUNCTION -->
			<fn:function name="fn:displayStock">
				<table>
				<xsl:for-each select="/table/row">
				<xsl:sort select="currentCount" data-type="number" order="ascending"/>
				<xsl:variable name ="count" select="position()"/>
					<tr>
						<itemCode><xsl:value-of select="/table/row[$count]/itemCode"/></itemCode>
						<itemDescription><xsl:value-of select="/table/row[$count]/itemDescription"/></itemDescription>

            <!-- Assign Variables-->
						<xsl:variable name ="currentCount" select="/table/row[$count]/currentCount"/>
						<xsl:variable name ="onOrder" select="/table/row[$count]/onOrder"/>
						
						<!--Colour onOrder background green based on if currentCount is 0 and onOder is 'Yes'-->
						<xsl:choose>
							<xsl:when test="currentCount='0' and onOrder='Yes'">
                <currentCount style="background-color: #BBFFBB"><xsl:value-of select="currentCount"/></currentCount>
								<onOrder style="background-color: #BBFFBB"><xsl:value-of select="$onOrder"/></onOrder>
							</xsl:when>
							<xsl:otherwise>
									<!--Colour onOrder background red based on if currentCount is 0 and onOder is 'Yes'-->
								<xsl:choose>
									<xsl:when test="currentCount='0' and onOrder='No'">
                    <currentCount style="background-color: #FFBBBB; color: #FF0000"><xsl:value-of select="currentCount"/></currentCount>
										<onOrder style="background-color: #FFBBBB; color: #FF0000"><xsl:value-of select="$onOrder"/></onOrder>
									</xsl:when>
									<xsl:otherwise>
                    <currentCount><xsl:value-of select="currentCount"/></currentCount>
                    <onOrder><xsl:value-of select="$onOrder"/></onOrder>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</tr>
				</xsl:for-each>
				</table>
			</fn:function>

			<h1 xPath="fn:displayStock()">
				<!--<xsl.value-of select="text()">-->
			</h1>
		</body>
		</html>
	</xsl:template>

	

</xsl:stylesheet>
