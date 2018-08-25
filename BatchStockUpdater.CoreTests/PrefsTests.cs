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
        [TestMethod()]
        public void LoadPrefsTest()
        {
            var didPrefsJsonLoadSuccessfully = Prefs.GetInstance().LoadPrefs();

            Assert.AreEqual(didPrefsJsonLoadSuccessfully, true);
        }
    }
}