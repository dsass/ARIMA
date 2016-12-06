using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ARIMA.timeseries.models
{
    public class RegressionResults
    {
        static Matrix<double> cov_params;
        static Matrix<double> param;
        double adfstat;
        double aic;
        double llf;
        double k_constant;

        public RegressionResults(string model, RegressionModel m, Matrix<double> p, Matrix<double> normalized_cov_params, double scale=1, string cov_type= "nonrobust")
        {
            cov_params = normalized_cov_params;
            param = p;
            adfstat = (param.At(0,0) - 1.0) / bse()[0];
            aic = -2 * llf + 2 * (m.Df_model + m.k);
        }
        
    
        public double Adfstat
        {
            get
            {
                return adfstat;
            }
        }        

        public double Aic
        {
            get
            {
                return aic;
            }
        }

        public double Llf
        {
            get
            {
                return llf;
            }
        }

        private static Matrix<double> computellf(Model m)
        {
            return m.loglike(param);
        }

        private static Vector<double> bse()
        {
            return cov_params.Diagonal().PointwisePower(0.5);
        }
    }
}
