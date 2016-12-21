using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using MathNet.Numerics.Statistics;

namespace ARIMA.timeseries.models
{
    public class RegressionModel : Model
    {
        protected double nobs;
        string thismodel;

        public RegressionModel(Matrix<double> Xdata, Vector<double> Ydata, string m) : base(Xdata, Ydata)
        {
            nobs = (double)X.RowCount;
            thismodel = m;
        }

        public double Rsquared(Matrix<double> X, Vector<double> Y, Vector<double> coef)
        {
            // 'coefficient of determination'
            int rows = X.RowCount;
            int cols = X.ColumnCount;

            // 1. compute mean of y
            double ySum = 0.0;
            for (int i = 0; i < rows; ++i)
                ySum += Y[i]; // last column
            double yMean = ySum / rows;

            // 2. sum of squared residuals & tot sum squares
            double ssr = 0.0;
            double sst = 0.0;
            double y; // actual y value
            double predictedY; // using the coef[] 
            for (int i = 0; i < rows; ++i)
            {
                y = Y[i]; // get actual y

                predictedY = coef[0]; // start w/ intercept constant
                for (int j = 0; j < cols; ++j) // j is col of data
                    predictedY += coef[j + 1] * X[i,j]; // careful

                ssr += (y - predictedY) * (y - predictedY);
                sst += (y - yMean) * (y - yMean);
            }

            if (sst == 0.0)
                throw new Exception("All y values equal");
            else
                return 1.0 - (ssr / sst);
        }

        public Matrix<double> designMatrix(Matrix<double> data)
        {
            // add a leading col of 1.0 values
            int rows = data.RowCount;
            int cols = data.ColumnCount;
            Matrix<double> result = Matrix<double>.Build.Dense(rows, cols + 1);
            for (int i = 0; i < rows; ++i)
                result[i,0] = 1.0;

            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < cols; ++j)
                    result[i,j + 1] = data[i,j];

            return result;
        }

        public Vector<double> fit(Matrix<double> design, Vector<double> Y)
        {
            // find linear regression coefficients
            // 1. peel off X matrix and Y vector
            int rows = design.RowCount;
            int cols = design.ColumnCount;
            Matrix<double> X = Matrix<double>.Build.Dense(rows, cols);

            int j;
            for (int i = 0; i < rows; i++)
            {
                for (j = 0; j < cols; j++)
                {
                    X[i,j] = design[i,j];
                }
            }

            Matrix<double> Xt = X.Transpose();
            Matrix<double> XtX = Xt * X;
            Matrix<double> inv = XtX.Inverse();
            Matrix<double> invXt = inv * Xt;

            Vector<double> result = invXt * Y;
            return result;
        }

        public double testValue(Matrix<double> X, Vector<double> Y, int attr, Vector<double> coeff)
        {
            // calculates the test statistic for a certain variable for the linear regression model
            // parameter estimate / parameter standard deviation
            int index = 0;
            if (attr != 0)
            {
                index = attr - 1;
            }
            double stdev = Statistics.StandardDeviation(X.Column(index));
            return coeff[attr] / (stdev / Math.Sqrt(X.RowCount));
        }


        /// <summary> 
        /// Moore–Penrose pseudoinverse 
        /// If A = U • Σ • VT is the singular value decomposition of A, then A† = V • Σ† • UT. 
        /// For a diagonal matrix such as Σ, we get the pseudoinverse by taking the reciprocal of each non-zero element 
        /// on the diagonal, leaving the zeros in place, and transposing the resulting matrix. 
        /// In numerical computation, only elements larger than some small tolerance are taken to be nonzero,
        /// and the others are replaced by zeros. For example, in the MATLAB or NumPy function pinv, 
        /// the tolerance is taken to be t = ε • max(m,n) • max(Σ), where ε is the machine epsilon. (Wikipedia) 
        /// </summary> 
        /// <param name="M">The matrix to pseudoinverse</param> 
        /// <returns>The pseudoinverse of this Matrix</returns> 
        public Matrix<double> PseudoInverse(Matrix<double> M)
        {
            MathNet.Numerics.LinearAlgebra.Factorization.Svd<double> D = M.Svd(true);
            Matrix<double> W = (Matrix<double>)D.W;
            Vector<double> s = (Vector<double>)D.S;

            // The first element of W has the maximum value. 
            double tolerance = MathNet.Numerics.Precision.EpsilonOf(2) * Math.Max(M.RowCount, M.ColumnCount) * W[0, 0];

            for (int i = 0; i < s.Count; i++)
            {
                if (s[i] < tolerance)
                    s[i] = 0;
                else
                    s[i] = 1 / s[i];
            }
            W.SetDiagonal(s);

            // (U * W * VT)T is equivalent with V * WT * UT 
            return (Matrix<double>)(D.U * W * D.VT).Transpose();
        }

    }

}
