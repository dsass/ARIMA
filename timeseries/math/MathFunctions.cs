using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARIMA.timeseries.math
{
    // contains math functions necessary for model computation such as differencing an array list
    public class MathFunctions
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

        public static double[,] lagmat(double[] x, int n)
        {
            int length = x.Length - n;
            double[,] res = new double[length, n + 1];
            int start = 0;
            for (int i = 0; i < length; i++)
            {
                int innerindex = 0;
                for (int j = start + n; j >= start; j--)
                {
                    res[i, innerindex] = x[j];
                    innerindex++;
                }
                start++;
            }
            return res;
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

        public static double variance(double[,] x, int axis)
        {
            int dim1 = x.GetLength(0);
            int dim2 = x.GetLength(1);
            if (axis == dim1)
            {
                double[] row = new double[dim2];
                for (int j = 0; j < dim2; j++)
                {
                    row[j] = x[0, j];
                }
                double avg = row.Average();
                double squaressumdiff = row.Select(val => (val - avg) * (val - avg)).Sum();
                return squaressumdiff / row.Length;
            } else if (axis == dim2)
            {
                double[] row = new double[dim1];
                for (int i = 0; i < dim1; i++)
                {
                    row[i] = x[1, i];
                }
                double avg = row.Average();
                double squaressumdiff = row.Select(val => (val - avg) * (val - avg)).Sum();
                return squaressumdiff / row.Length;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static double[,] add_constant(double[,] X, bool prepend)
        {
            if (variance(X, 0) == 0)
            {
                return X;
            }
            else
            {
                double[,] ones = new double[X.GetLength(0), 1];
                for (int i = 0; i < X.GetLength(0); i++)
                {
                    ones[i, 0] = 1;
                }
                X = column_stack(X, ones);
                if (prepend)
                {
                    return roll(X, 1, 1);
                } else
                {
                   return X;
                }
            }
        }

        public static double[,] add_trend(double[,] X, string trend = "c", bool prepend = false)
        {
            trend = trend.ToLower();
            int trendorder = 0;
            if (String.Equals(trend, "c"))
            {
                return add_constant(X, prepend);
            }
            else if (String.Equals(trend, "ct") || String.Equals(trend, "t"))
            {
                trendorder = 1;
            }
            else if (trend.Equals("ctt"))
            {
                trendorder = 2;
            }
            else
            {
                throw new ArgumentException();
            }

            int nobs = X.GetLength(0);
            double[] a = new double[nobs];
            for (int i = 1; i < nobs + 1; i++)
            {
                a[i - 1] = i;
            }
            double[,] trendarr = vander(a, trendorder + 1);
            // put in order ctt
            trendarr = fliplr(trendarr);
            //if (String.Equals(trend,"t"))
            //{
            //    trendarr = trendarr[:,1];
            //}
            if (prepend == false)
            {
                return column_stack(X, trendarr);
            } else
            {
                return column_stack(trendarr, X);
            }
            //if not X.dtype.names:
            //    if not prepend:
            //        X = np.column_stack((X, trendarr))
            //    else:
            //        X = np.column_stack((trendarr, X))
            //else:
            //    return_rec = data.__clas__ is np.recarray
            //    if trendorder == 1:
            //        if trend == "ct":
            //            dt = [('const',float),('trend',float)]
            //        else:
            //            dt = [('trend', float)]
            //    elif trendorder == 2:
            //        dt = [('const',float),('trend',float),('trend_squared', float)]
            //    trendarr = trendarr.view(dt)
            //    if prepend:
            //        X = nprf.append_fields(trendarr, X.dtype.names, [X[i] for i
            //            in data.dtype.names], usemask=False, asrecarray=return_rec)
            //    else:
            //        X = nprf.append_fields(X, trendarr.dtype.names, [trendarr[i] for i
            //            in trendarr.dtype.names], usemask=false, asrecarray=return_rec)
            //return X
        }
    }
}
