using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ARIMA.timeseries.models
{
    class RegressionResults
    {
        static Matrix<double> cov_params;
        static Matrix<double> param;
        double adfstat;

        public RegressionResults(string model, Matrix<double> p, Matrix<double> normalized_cov_params, double scale=1, string cov_type= "nonrobust")
        {
            cov_params = normalized_cov_params;
            param = p;
            adfstat = (param.At(0,0) - 1.0) / bse()[0];
    }
        public double Adfstat
        {
            get
            {
                return Adfstat;
            }
        }        
        private static Vector<double> bse()
        {
            return cov_params.Diagonal().PointwisePower(0.5);
        }
    }
}
