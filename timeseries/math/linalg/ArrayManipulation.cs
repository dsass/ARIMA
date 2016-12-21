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
        // difference an array
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

        //compute the vandermonte matrix of an array
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

        // flip a matrix from left to right
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

        // Stack 2-D arrays as columns into a 2-D array.
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

        // Roll array elements along a given axis.
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


    }
}
