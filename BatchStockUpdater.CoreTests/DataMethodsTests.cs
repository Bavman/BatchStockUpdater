using Microsoft.VisualStudio.TestTools.UnitTesting;
using BatchStockUpdater.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatchStockUpdater.Users;
using System.Data;

namespace BatchStockUpdater.Core.Tests
{
    [TestClass()]
    public class DataMethodsTests
    {
        string[] _csvHeader = new string[]
        {
            "Item Code",
            "Item Description",
            "Current Count",
            "On Order"
        };


        string[] _colArray1 = new string[] { "A0001", "A0002", "A0003", "A0004" };
        string[] _colArray2 = new string[] { "Horse on Wheels", "Elephant on Wheels", "Dog on Wheels", "Seal on Wheels" };
        int[] _colArray3 = new int[] { 4, 20, 10, 3 };
        string[] _colArray4 = new string[] { "No", "No", "Yes", "No" };

        // Tests collection returned by the method ReturnColumnCollection against the above local array variables.
        // This method can be used to store imported columns from the stocklist.csv file for later comparison.
        [TestMethod()]
        public void ReturnColumnCollectionTest()
        {

            var dataMethods = DataMethods.GetInstance();
            var returnADataTable = new ReturnADataTable_ForTesting();

            var testDataTable = returnADataTable.TestDataTable();
            Console.WriteLine("DataTable Row Count " + testDataTable.Rows.Count);

            //var testCollection = dataMethods.ReturnColumnCollection<string>(testDataTable, _csvHeader[0]);
            //var testCollection = dataMethods.ReturnColumnCollection<string>(testDataTable, _csvHeader[1]);
            //var testCollection = dataMethods.ReturnColumnCollection<int>(testDataTable, _csvHeader[2]);
            var testCollection = dataMethods.ReturnColumnCollection<string>(testDataTable, _csvHeader[3]);

            Console.WriteLine("Column from dataTable");
            foreach (var cell in testCollection.ToArray())
            {
                Console.WriteLine(cell);
            }

            Console.WriteLine("TestArray");
            foreach (var cell in _colArray4)
            {
                Console.WriteLine(cell);
            }


            //CollectionAssert.AreEqual(testCollection.ToArray(), colArray1);
            //CollectionAssert.AreEqual(testCollection.ToArray(), colArray2);
            //CollectionAssert.AreEqual(testCollection.ToArray(), colArray3);
            CollectionAssert.AreEqual(testCollection.ToArray(), _colArray4);

            //Assert.AreEqual(4, testCollection.Count);
        }

        // Checks reference against primitive variables
        [TestMethod()]
        public void CompareCurrentCountsTablesTest()
        {
            // Class Lists for testing
            var user1 = new AppUser { UserName = "Frank", Password = "Hey1" };
            var user2 = new AppUser { UserName = "Bob", Password = "Hey2" };

            var userList1 = new List<AppUser> { user1, user2 };
            var userList2 = new List<AppUser> { user1, user2 };

            var userList3 = new List<AppUser> { new AppUser { UserName = "Frank", Password = "Hey1" },
                new AppUser { UserName = "Bob", Password = "Hey2" } };

            // Primitive Lists for testing
            var testColArray1 = new string[] { "A0001", "A0002", "A0003", "A0004" };


            var dataMethods = DataMethods.GetInstance();
            var isEqual = dataMethods.CompareCurrentCountsTables<string>(_colArray1, testColArray1);
            //var isEqual = dataMethods.CompareCurrentCountsTables<AppUser>(userList1, userList3);

            Assert.AreEqual(isEqual, true);
        }


        [TestMethod()]
        public void PopulateDataTableTest()
        {
            var csvFilePath = @"C:\stocklist.CSV";

            var fileIO = new FileIO();

            var textFieldParser = fileIO.ReturnCSVData(csvFilePath);

            var dataTabe = DataMethods.GetInstance().PopulateDataTable(textFieldParser, _csvHeader);

            var rowCount = dataTabe.Rows.Count;

            var columnCount = dataTabe.Columns.Count;

            var rowColumnIntArray = new int[] { rowCount, columnCount };

            var compareIntArray = new int[] { 40, 4 };

            Console.WriteLine("Row count {0}",rowCount);
            Console.WriteLine("Column count {0}", columnCount);

            CollectionAssert.AreEqual(rowColumnIntArray, compareIntArray);
        }
    }
}