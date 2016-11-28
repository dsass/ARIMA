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

        public static double[] lagmat(double[] array, int maxlag)
        {
            return null;
        }

        public static double[] add_trend(double[] x, string trend = "c", bool prepend= false)
        {
            return null;
        }
    }
}
