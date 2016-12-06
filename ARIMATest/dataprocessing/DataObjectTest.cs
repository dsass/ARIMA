using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ARIMA.dataprocessing;

namespace ARIMATest.dataprocessing
{
    [TestClass]
    public class DataObjectTest
    {
        [TestMethod]
        public void DateObjectInitTest()
        {
            string date = "20/07/1990";
            string time = "14:13:00";
            string val = "Value!";
            DateTime d = new DateTime(1990, 07, 20, 14, 13, 0);
            ARIMA.dataprocessing.DataObject dobj = new ARIMA.dataprocessing.DataObject(date, time, val);
            Assert.AreEqual(DateTime.Compare(d, dobj.Date), 0);
        }
    }
}
