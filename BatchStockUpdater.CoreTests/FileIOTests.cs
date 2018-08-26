using Microsoft.VisualStudio.TestTools.UnitTesting;
using BatchStockUpdater.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Data;

namespace BatchStockUpdater.Core.Tests
{
    [TestClass()]
    public class FileIOTests
    {
        // Test the LoadCSV method and confirm that the first row of data is begin read in.
        // Compare the first line if csv data with the comparisonCSVLine variable within the test function.
        [TestMethod()]
        public void LoadCSVTest()
        {

            var csvFilePath = @"C:\stocklist.CSV";

            var comparisonCSVLine = new string[] { "Item Code", "Item Description", "Current Count", "On Order" };
            var firstCSVLine = new string[0];

            var fileIO = new FileIO();

            var textFieldParser = fileIO.LoadCSV(csvFilePath, false);

            var i = 0;

            while (!textFieldParser.EndOfData)
            {

                firstCSVLine = textFieldParser.ReadFields();

                i++;
                Console.WriteLine("Line {0}",i);

                if (i > 0)
                {
                    break;
                }
            }

            Console.WriteLine("{0}, {1}, {2}, {3}", firstCSVLine[0], firstCSVLine[1], firstCSVLine[2], firstCSVLine[3]);

            CollectionAssert.AreEqual(comparisonCSVLine, firstCSVLine);
        }


        // The ReturnCSVData method returns a TextFieldParser object or null of an incorrect path is entered.
        [TestMethod()]
        public void ReturnCSVDataTest()
        {
            var csvFilePath = @"C:\stocklist.CSV";

            var fileIO = new FileIO();

            var textFieldParser = fileIO.ReturnCSVData(csvFilePath);

            Assert.AreNotEqual(textFieldParser, null);
        }

        // The ChechHearderRow checks the stocklist.csv header against a local string array
        // containg the expected values.
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

            var csvFilePath = @"C:\stocklist.CSV";

            var fileIO = new FileIO();

            var testTextFieldParser = fileIO.LoadCSV(csvFilePath, false);

            var isHeaderCorrect = fileIO.ChechHearderRow(testTextFieldParser, csvHeader);

            Assert.AreEqual(isHeaderCorrect, true);
        }

        // Method returns 'true' if the checkDateTime is less than the 
        // stocklist.csv modified datetime.
        [TestMethod()]
        public void IsFileDateModifiedCurrentTest()
        {
            // Get stocklist.csv filepath
            var csvFilePath = @"C:\stocklist.CSV";

            // Setup Date time to check against file date time
            var checkDateTime = DateTime.Today;

            // Add dats and a new time span -- modifiy these variables to test method.
            checkDateTime = checkDateTime.AddDays(-8);
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

        // Checks if the SaveCSV method successfully saves the updated stocklist data.
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

    }
}