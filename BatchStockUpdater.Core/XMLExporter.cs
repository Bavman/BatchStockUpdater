using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualBasic.FileIO;
using System.Data;

namespace BatchStockUpdater.Core
{
    public class XMLExporter
    {

        public void ExportXML(string savePath, DataTable dataTable, int styleIndex)
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

            var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                styleSheet);

            var header = new XElement("header",
                new XElement("th", "Item Code"),
                new XElement("th", "Item Description"),
                new XElement("th", "Current Count"),
                new XElement("th", "On Order"));

            var table = new XElement("table", header);

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

            document.Add(table);

            document.Save(savePath);
        }
 
    }
}
