using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BatchStockUpdater.Core.Tests
{
    [TestClass()]
    public class XMLExporterTests
    {
        [TestMethod()]
        public void ExportXMLTestPass()
        {

            var dataTable = new ReturnADataTable_ForTesting().TestDataTable();
            var result = new XMLExporter().ExportXML(@"C:\Cert IV\stocklistText.xml", dataTable, 2);
            
            var successString1 = ("Successfully exported XML.");

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.MessageList[0], successString1);

        }

        [TestMethod()]
        public void ExportXMLTestFail1()
        {

            var dataTable = new ReturnADataTable_ForTesting().TestDataTable();
            var result = new XMLExporter().ExportXML(@"F:\stocklistText.xml", dataTable, 2);


            var failedString1 = ("Invalid folder. Change save folder path in preferences.");

            //var failedString3 = ("Faild to exported XML. Please contact systems administrator.");

            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.MessageList[0], failedString1);

        }

        [TestMethod()]
        public void ExportXMLTestFail2()
        {

            var dataTable = new ReturnADataTable_ForTesting().TestDataTable();
            var result = new XMLExporter().ExportXML(@"C:\stocklistText.xml", dataTable, 2);

            var failedString2 = ("Faild to exported XML file due to unauthorized access of destination folder. " +
                    "\nPlease check the save path in preferences and try again");

            //var failedString3 = ("Faild to exported XML. Please contact systems administrator.");

            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.MessageList[0], failedString2);

        }
    }
}