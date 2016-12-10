using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ARIMA.timeseries.models
{
    public class RegressionModel : Model
    {
        string[] data_attr;
        //Matrix<double> normalized_cov_params;
        static double thisrank;
        static double df_model;
        //Matrix<double> pinv_wexog;
        Matrix<double> wexog;
        protected double nobs;
        Matrix<double> wendog;
        string thismodel;
        static int k_constant;

        public RegressionModel(Matrix<double> Xdata, Matrix<double> Ydata, string m) : base(Xdata, Ydata)
        {
            nobs = (double)X.RowCount;
            thismodel = m;
        }

        public double Rsquared(Matrix<double> data, Vector<double> coef)
        {
            // 'coefficient of determination'
            int rows = data.RowCount;
            int cols = data.ColumnCount;

            // 1. compute mean of y
            double ySum = 0.0;
            for (int i = 0; i < rows; ++i)
                ySum += data[i,cols - 1]; // last column
            double yMean = ySum / rows;

            // 2. sum of squared residuals & tot sum squares
            double ssr = 0.0;
            double sst = 0.0;
            double y; // actual y value
            double predictedY; // using the coef[] 
            for (int i = 0; i < rows; ++i)
            {
                y = data[i,cols - 1]; // get actual y

                predictedY = coef[0]; // start w/ intercept constant
                for (int j = 0; j < cols - 1; ++j) // j is col of data
                    predictedY += coef[j + 1] * data[i,j]; // careful

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
            //double[][] result = MatrixCreate(rows, cols + 1);
            for (int i = 0; i < rows; ++i)
                result[i,0] = 1.0;

            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < cols; ++j)
                    result[i,j + 1] = data[i,j];

            return result;
        }

        public Vector<double> fit(Matrix<double> design)
        {
            // find linear regression coefficients
            // 1. peel off X matrix and Y vector
            int rows = design.RowCount;
            int cols = design.ColumnCount;
            Matrix<double> X = Matrix<double>.Build.Dense(rows, cols - 1);
            Vector<double> Y = Vector<double>.Build.Dense(rows, 1);//MatrixCreate(rows, 1); // a column vector

            int j;
            for (int i = 0; i < rows; ++i)
            {
                for (j = 0; j < cols - 1; ++j)
                {
                    X[i,j] = design[i,j];
                }
                Y[i] = design[i,j]; // last column
            }

            Matrix<double> Xt = X.Transpose();
            Matrix<double> XtX = Xt * X;
            Matrix<double> inv = XtX.Inverse();
            Matrix<double> invXt = inv * Xt;

            Vector<double> result = invXt * Y;

            // 2. B = inv(Xt * X) * Xt * y
           // double[][] Xt = MatrixTranspose(X);
           // double[][] XtX = MatrixProduct(Xt, X);
           // double[][] inv = MatrixInverse(XtX);
           // double[][] invXt = MatrixProduct(inv, Xt);

           // double[][] mResult = MatrixProduct(invXt, Y);
           // double[] result = MatrixToVector(mResult);
            return result;
        } // Solve

        public Matrix<double> DummyData(int rows, int seed)
        {
            // generate dummy data for linear regression problem
            double b0 = 15.0;
            double b1 = 0.8; // education years
            double b2 = 0.5; // work years
            double b3 = -3.0; // sex = 0 male, 1 female
            Random rnd = new Random(seed);

            Matrix<double> result = Matrix<double>.Build.Dense(rows, 4);

            for (int i = 0; i < rows; ++i)
            {
                int ed = rnd.Next(12, 17); // 12, 16]
                int work = rnd.Next(10, 31); // [10, 30]
                int sex = rnd.Next(0, 2); // 0 or 1
                double y = b0 + (b1 * ed) + (b2 * work) + (b3 * sex);
                y += 10.0 * rnd.NextDouble() - 5.0; // random [-5 +5]

                result[i,0] = ed;
                result[i,1] = work;
                result[i,2] = sex;
                result[i,3] = y; // income
            }
            return result;
        }

        public double testValue(Matrix<double> data, Vector<double> coeff)
        {
            double rsquared = Rsquared(data, coeff);
            double numerator = nobs - 2;
            double denominator = 1 - rsquared;
            return Math.Sqrt(rsquared) * Math.Sqrt(numerator / denominator);
        }

    }

}
