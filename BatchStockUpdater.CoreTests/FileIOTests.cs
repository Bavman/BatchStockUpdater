using Microsoft.VisualStudio.TestTools.UnitTesting;
using BatchStockUpdater.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchStockUpdater.Core.Tests
{
    [TestClass()]
    public class FileIOTests
    {
        // Method returns 'true' if the checkDateTime is less than the 
        // stocklist.csv modified datetime.
        [TestMethod()]
        public void DoesFileDateCheckPass()
        {
            // Get stocklist.csv filepath
            var prefs = Prefs.GetInstance();
            var csvFilePath = prefs.FilePath;

            // Setup Date time to check against file date time
            var checkDateTime = DateTime.Today;

            // Add dats and a new time span -- modifiy these variables to test method.
            checkDateTime = checkDateTime.AddDays(0);
            var checkTime = new TimeSpan(8, 2, 0);

            // Add timespan to checkDateTime
            checkDateTime = checkDateTime.Add(checkTime);
            Console.WriteLine("Set Date Time Check " + checkDateTime);

            var fileIO = new FileIO();
            var isOK = fileIO.DoesFileDateCheckPass(csvFilePath, checkDateTime);

            Assert.AreEqual(true, isOK);

        }
    }
}