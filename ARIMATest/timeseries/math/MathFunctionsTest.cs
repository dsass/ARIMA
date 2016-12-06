using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ARIMATest.timeseries.math
{
    [TestClass]
    public class MathFunctionsTest
    {
        [TestMethod]
        public void diffTest()
        {
            double[] x = { 1, 2, 4, 7, 0 };
            double[] ans1 = ARIMA.timeseries.math.MathFunctions.diff(x);
            Assert.AreEqual(System.Linq.Enumerable.SequenceEqual(ans1, new double[] { 1, 2, 3, -7 }), true);
            Assert.AreEqual(System.Linq.Enumerable.SequenceEqual(ARIMA.timeseries.math.MathFunctions.diff(x, 2), new double[] { 1, 1, -10 }), true);
        }
    }
}
