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
    public class PrefsTests
    {

        // Checks successful load of the prefs.json file.
        [TestMethod()]
        public void LoadPrefsTest()
        {
            var isSuccessfulPrefsLoad = Prefs.GetInstance().LoadPrefs();

            Assert.AreEqual(isSuccessfulPrefsLoad, true);
        }
    }
}