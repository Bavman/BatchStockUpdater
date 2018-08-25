using Microsoft.VisualStudio.TestTools.UnitTesting;
using BatchStockUpdater.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace BatchStockUpdater.Core.Tests
{
    [TestClass()]
    public class FileIOTests
    {


        [TestMethod()]
        public void ReturnCSVDataTest()
        {
            var csvFilePath = @"C:\Cert IV\stocklist.CSV";

            var fileIO = new FileIO();

            var textFieldParser = fileIO.ReturnCSVData(csvFilePath);

            Assert.AreNotEqual(textFieldParser, null);
        }

        [TestMethod()]
        public void ChechHearderRowTest()
        {
            var csvHeader = new string[]
            {
                "Item Code",
                "Item Description",
                "Current Count",
                "On Order"
            };

            var csvFilePath = @"C:\Cert IV\stocklist.CSV";

            var fileIO = new FileIO();

            var testTextFieldParser = fileIO.LoadCSV(csvFilePath, false);

            var isHeaderCorrect = fileIO.ChechHearderRow(testTextFieldParser, csvHeader);

            Assert.AreEqual(isHeaderCorrect, true);
        }

        // Method returns 'true' if the checkDateTime is less than the 
        // stocklist.csv modified datetime.
        [TestMethod()]
        public void DoesFileDateCheckPass()
        {
            // Get stocklist.csv filepath
            var csvFilePath = @"C:\Cert IV\stocklist.CSV";

            // Setup Date time to check against file date time
            var checkDateTime = DateTime.Today;

            // Add dats and a new time span -- modifiy these variables to test method.
            checkDateTime = checkDateTime.AddDays(0);
            var checkTime = new TimeSpan(8, 2, 0);

            checkDateTime = checkDateTime.Add(checkTime);

            // Add timespan to checkDateTime
            var fileInfo = new FileInfo(csvFilePath);
            var fileDateModified = fileInfo.LastWriteTime;

            Console.WriteLine("Path: {0}", csvFilePath);
            Console.WriteLine("File Modified: {0}", fileDateModified);
            Console.WriteLine("Set Date Time Check {0}", checkDateTime);

            var fileIO = new FileIO();
            var isOK = fileIO.IsFileDateModifiedCurrent(csvFilePath, checkDateTime);

            Assert.AreEqual(true, isOK);
        }

        [TestMethod()]
        public void SaveCSVTest()
        {
            var saveCSVFilePath = @"C:\Cert IV\TestSave.CSV";

            var fileIO = new FileIO();

            var returnADataTable = new ReturnADataTable();

            var testDataTable = returnADataTable.TestDataTable();

            var isSaved = fileIO.SaveCSV(saveCSVFilePath, testDataTable);

            Assert.AreEqual(true, isSaved);
        }

        [TestMethod()]
        public void LoadCSVTest()
        {
            Assert.Fail();
        }
    }
}