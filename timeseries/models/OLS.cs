using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ARIMA.timeseries.models
{
    // ordinary least squares regression model
    class OLS : RegressionModel
    {
        public OLS(Matrix<double> endog, Matrix<double> exog, string m) : base(endog, exog, m) {}

        public Matrix<double> logmatrix(Matrix<double> m)
        {
            for (int i = 0; i < m.RowCount; i++)
            {
                for (int j = 0; j < m.ColumnCount; j++)
                {
                    double log = Math.Log(m.At(i, j));
                    m[i,j] = log;
                }
            }
            return m;
        }

        public Matrix<double> loglike(Matrix<double> param)
        {
            //        The likelihood function for the clasical OLS model.

            //        Parameters
            //        ----------
            //        params : array-like
            //            The coefficients with which to estimate the log-likelihood.

            //        Returns
            //        -------
            //        The concentrated likelihood function evaluated at params.
            double nobs2 = nobs / 2.0;
           Matrix<double> m1 = enddata - exogdata.PointwiseMultiply(param);
           Matrix<double> m2 = m1.Transpose().PointwiseMultiply(m1);
           m1.TransposeAndMultiply(m1, m2);
           return -nobs2 * Math.Log(2 * Math.PI) - nobs2 * logmatrix(1 / (2 * nobs2) * m2) - nobs2;
        }

        public override Matrix<double> whiten(Matrix<double> Y)
        {
            // OLS model whitener does nothing: returns Y.   
            return Y;
        }
    }
}
