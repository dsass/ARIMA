using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ARIMA.timeseries.math.linalg
{
    public class ArrayManipulation
    {
        public static double[] diff(double[] array, int n = 1)
        {
            if (n == 0)
            {
                return array;
            }
            if (n >= array.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            double[] diffarray = new double[array.Length - 1];
            for (int i = 0; i < array.Length - 1; i++)
            {
                diffarray[i] = array[i + 1] - array[i];
            }
            return diff(diffarray, n - 1);
        }

        public static double[,] vander(double[] x, int n)
        {
            double[,] res = new double[x.Length, n];
            int pow = n - 1;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < x.Length; j++)
                {
                    res[j, i] = Math.Pow(x[j], pow);
                }
                pow--;
            }
            return res;
        }

        public static double[,] fliplr(double[,] x)
        {
            int dim1 = x.GetLength(0);
            int dim2 = x.GetLength(1);
            for (int i = 0; i < dim1; i++)
            {
                double[] row = new double[dim2];
                for (int j = 0; j < dim2; j++)
                {
                    row[j] = x[i, j];
                }
                Array.Reverse(row);
                for (int j = 0; j < dim2; j++)
                {
                    x[i, j] = row[j];
                }
            }
            return x;
        }

        public static double[,] column_stack(double[,] array1, double[,] array2)
        {
            int length = array1.GetLength(1) + array2.GetLength(2);
            double[,] res = new double[array1.GetLength(0), length];
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    res[i, j] = array1[i, j];
                }
            }
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = array1.GetLength(1); j < length; j++)
                {
                    res[i, j] = array2[i, j - array1.GetLength(1)];
                }
            }
            return res;
        }

        public static double[,] roll(double[,] X, int shift, int axis)
        {
            double[,] res = new double[X.GetLength(0), X.GetLength(1)];
            int otherdim = (axis + 1) % 2;
            if (otherdim == 0)
            {
                for (int i = 0; i < X.GetLength(otherdim); i++)
                {
                    for (int j = 0; j < X.GetLength(axis); j++)
                    {
                        res[i, j] = X[i, (j + X.GetLength(axis) - 1) % X.GetLength(axis)];
                    }
                }
            }
            else
            {
                for (int i = 0; i < X.GetLength(axis); i++)
                {
                    for (int j = 0; j < X.GetLength(otherdim); j++)
                    {
                        res[i, j] = X[(i + X.GetLength(axis) - 1) % X.GetLength(axis), j];
                    }
                }
            }
            return res;
        }

        public static Matrix<double> pinv_extend(Matrix<double> X, double rcond = 1e-15)
        {
            X = X.Conjugate();
            MathNet.Numerics.LinearAlgebra.Factorization.Svd<double> svd = X.Svd(true);
            double m = svd.U.RowCount;
            double n = svd.VT.ColumnCount;
            Vector<double> s = svd.S;
            double cutoff = rcond * s.Maximum();
            for (int j = 0; j < Math.Min(m, n); j++)
            {
                if (s[j] > cutoff)
                {
                    s[j] = 1.0 / s[j];
                }
                else
                {
                    s[j] = 0.0;
                }
            }
            Matrix<double> vttranspose = svd.VT.Transpose();
            //Console.WriteLine(vttranspose.ToString());
            Console.WriteLine(m);
            Console.WriteLine(n);
            Console.WriteLine(Math.Min(m, n));
            Console.WriteLine(svd.VT.ToString());
            Console.WriteLine(svd.U.ToString());

            // THIS WAS A QUICK FIX TO DEAL WITH THE FACT THE U AND VT ARE SQUARE MATRICES AND NOT M x K
            // MUST DEAL WITH THIS BEFORE CONTINUING OTHERWISE IT MESSES UP EVERYTHING

            //Vector<double> scopy = Vector<double>.Build.Dense((int)m);
            //int i = 0;
            //int pointer = 0;
            //while (i < m)
            //{
            //    scopy[i] = s[pointer];
            //    //s = (Vector<double>)s.Concat(scopy);
            //    i++;
            //    pointer = (pointer + 1) % s.Count;
            //}
            Matrix<double> scopy = Matrix<double>.Build.Dense((int)m, (int)m, s[0]);
            Matrix<double> vtcopy = Matrix<double>.Build.Dense((int)m, (int)m, svd.VT[0,0]);
            Matrix<double> sbyutrans = scopy.TransposeAndMultiply(svd.U);
            //Matrix<double> sbyutrans = scopy.ToColumnMatrix().TransposeAndMultiply(svd.U);
            //return sbyutrans.PointwiseMultiply(vttranspose);
            return sbyutrans.PointwiseMultiply(vtcopy.Transpose());

            //    u, s, vt = np.linalg.svd(X, 0)
            //    s_orig = np.copy(s)
            //    m = u.shape[0]
            //    n = vt.shape[1]
            // cutoff = rcond * np.maximum.reduce(s)
            //for i in range(min(n, m)):
            //    if s[i] > cutoff:
            //        s[i] = 1./ s[i]
            //    else:
            //        s[i] = 0.
            //res = np.dot(np.transpose(vt), np.multiply(s[:, np.core.newaxis],
            //                                 np.transpose(u)))
            //return res;
        }

    }
}
