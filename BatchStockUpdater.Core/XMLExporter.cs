using System;
using System.Linq;
using System.Xml.Linq;
using System.Data;
using System.IO;

namespace BatchStockUpdater.Core
{
    public class XMLExporter
    {
        // Export stock data as XML
        public OperationResult ExportXML(string savePath, DataTable dataTable, int styleIndex)
        {
            var opResult = new OperationResult();

            //Guard Clause
            var folderPath = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(folderPath))
            {
                opResult.Success = false;
                opResult.AddMessage("Invalid folder. Change save folder path in preferences.");
                Logging.GetInstance().LogExportXML(LogStatus.Failure, styleIndex);
                return opResult;
            }

            var styleSheet = ReturnXMLStyleSheet(styleIndex);

            var header = ReturnXMLHeader();

            var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), styleSheet);

            var table = new XElement("stockTable", header);

            AddDataTableItemsToXMLTable(dataTable, table);

            document.Add(table);

            try
            {
                document.Save(savePath);

                opResult.Success = true;
                opResult.AddMessage("Successfully exported XML.");

                Logging.GetInstance().LogExportXML(LogStatus.Success, styleIndex);
            }
            catch (UnauthorizedAccessException)
            {
                opResult.Success = false;
                opResult.AddMessage("Faild to exported XML file due to unauthorized access of destination folder. " +
                    "\nPlease check the save path in preferences and try again");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                opResult.Success = false;
                opResult.AddMessage("Faild to exported XML. Please contact systems administrator.");
                Logging.GetInstance().LogExportXML(LogStatus.Failure, styleIndex);
            }

            return opResult;
        }


        // Return selected Style Sheet Processing Instruction
        private XProcessingInstruction ReturnXMLStyleSheet(int styleIndex)
        {
            XProcessingInstruction styleSheet = null;

            switch (styleIndex)
            {
                case 1:
                    styleSheet = new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='stocklist1.xslt'");
                    break;
                case 2:
                    styleSheet = new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='stocklist2.xslt'");
                    break;
            }

            return styleSheet;
        }

        // Return XML Header
        private static XElement ReturnXMLHeader()
        {
            return new XElement("headers",
                new XElement("headerItem", "Item Code"),
                new XElement("headerItem", "Item Description"),
                new XElement("headerItem", "Current Count"),
                new XElement("headerItem", "On Order"));
        }

        // Add DataTable rows to XML XElement (table)
        private static void AddDataTableItemsToXMLTable(DataTable dataTable, XElement table)
        {
            var rows = dataTable.Rows;

            // Loop through DataTable rows
            for (var i = 0; i < rows.Count; i++)
            {
                var rowFields = rows[i].ItemArray.
                    Select(field => field.ToString()).ToArray();

                var row = new XElement("row",
                    new XElement("itemCode", rowFields[0]),
                    new XElement("itemDescription", rowFields[1]),
                    new XElement("currentCount", rowFields[2]),
                    new XElement("onOrder", rowFields[3]));

                table.Add(row);
            }
        }
    }
}
