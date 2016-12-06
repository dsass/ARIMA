using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ARIMA.timeseries.models;
using MathNet.Numerics.LinearAlgebra;

namespace ARIMATest.timeseries.models
{
    [TestClass]
    public class OLSTest
    {
        [TestMethod]
        public void OLSinitTest()
        {
            Matrix<double> xdshort = Matrix<double>.Build.Dense(1, 6, 1.0);
            Matrix<double> RHS = Matrix<double>.Build.Dense(6, 3, 1.0);
            for (int i = 0; i < 6; i++)
            {
                RHS[i, 0] = i + 3;
            }
            //int startlag = 1;
            OLS OLS_instance = new OLS(xdshort, RHS.SubMatrix(0, RHS.RowCount, 0, 1), "OLS");
        }

        [TestMethod]
        public void OLSFitTest()
        {
            Matrix<double> xdshort = Matrix<double>.Build.Dense(1, 6, 1.0);
            Matrix<double> RHS = Matrix<double>.Build.Dense(6, 3, 1.0);
            for (int i = 0; i < 6; i++)
            {
                RHS[i, 0] = i + 3;
            }
            //int startlag = 1;
            OLS OLS_instance = new OLS(xdshort, RHS.SubMatrix(0, RHS.RowCount, 0, 1), "OLS");
            RegressionResults res = OLS_instance.fit();
            Console.WriteLine(res.Adfstat);
        }

    }
}
