using Microsoft.VisualStudio.TestTools.UnitTesting;
using BatchStockUpdater.UI;
using BatchStockUpdater.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BatchStockUpdater.UI.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        [TestMethod()]
        public void ReturnColumnCollectionTest()
        {
            MainWindow mainWindow = new MainWindow();

            TestDataTable testDataTable = new TestDataTable();

            DataTable dataTable = testDataTable.ReturnTestDataTable();

            mainWindow.ReturnColumnCollection<int>(dataTable, "");

            Assert.Fail();
        }
    }
}