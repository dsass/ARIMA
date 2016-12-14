using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARIMA.dataprocessing;
using ARIMA.timeseries.stats;
using MathNet.Numerics.LinearAlgebra;
//using ARIMA.cronos-ARMA.ABMath.ModelFramework;
using ARIMA.timeseries.math.linalg;
using ABMath.ModelFramework;
using ABMath.ModelFramework.Transforms;
using ABMath.ModelFramework.Data;
using ABMath.ModelFramework.Models;
//using cronos-ARMA.ABMath.ModelFramework.Transforms;


namespace ARIMA.timeseries
{
    public class TimeSeriesWrapper
    {
        private string xattr;
        private string yattr;
        private string file;

        public TimeSeriesWrapper(string xattribute, string yattribute, string filename)
        {
            xattr = xattribute;
            yattr = yattribute;
            file = filename;
        }

        public string XAttr
        {
            get
            {
                return String.Copy(xattr);
            }
        }

        public string YAttr
        {
            get
            {
                return String.Copy(yattr);
            }
        }

        public void beginARMAProcess(char delimiter, double siglevel, int len=50)
        {
            CSVReader reader = new CSVReader();
            string[] headers = reader.getHeaders(file, delimiter);
            int xindex = -1;
            int yindex = -1;
            for (int i = 0; i < headers.Length; i++)
            {
                if (String.Compare(headers[i], xattr) == 0)
                {
                    xindex = i;
                } else if (String.Compare(headers[i], yattr) == 0)
                {
                    yindex = i;
                }
            }
            List<String[]> data = reader.getData(xindex, yindex, 0, 1, file, delimiter);
            DataObject[,] series = getSeries(data, len);
            Console.WriteLine(testStationarity(series, siglevel));

            int d = 0;
            if (testStationarity(series, siglevel))
            {
                d = 1;
            }

            var tSeries = series;
            
            //Difference the series
            if (d != 0)
            {
                DataObject[] col = GetCol(tSeries, 1);
                double[] dcol = new double[col.Length];
                for (int i = 0; i < col.Length; i++)
                {
                    dcol[i] = Double.Parse(col[i].Value);
                }
                double[] xdiff = ArrayManipulation.diff(dcol);
                for (int i = 0; i < xdiff.Length; i++)
                {
                    tSeries[i, 1].Value = xdiff[i].ToString();
                }
                //tSeries = ArrayManipulation.diff(dcol);
            }

            var ts = new TimeSeries 
                                {
                                    Title = "Time Series",
                                    Description = "TS Description"
                                };
            for (int i = 0; i < series.GetLength(0); i++)
            {
                ts.Add(tSeries[i,0].Date, Double.Parse(tSeries[i,1].Value), false); //datetime, value, false
            }

            //TODO log transform the time series
        
            //Use ACF and PACF to find ARMA parameters
            var highInterval = 1.96/Math.Sqrt(series.Length);
            var acf = ts.ComputeACF(20, false);
            Console.WriteLine("confidence interval: " + highInterval.ToString());
            Console.WriteLine(acf);
            //bool crossed = acf[0] > highInterval;
            int p = 0;
            var low = acf[0];
            for (int i = 0; i < acf.Count; i++) 
            {
                Console.WriteLine("interval: " + highInterval.ToString() + " acf: " + acf[i].ToString());
                //if ((acf[i] > highInterval) != crossed)
                if (Math.Min(low, acf[i]) <= highInterval && highInterval < Math.Max(acf[i], low))
                {
                    p = i;
                    if (p != 0)
                    {
                        p--;
                    }
                    break;
                }
                low = acf[i];
            }
            var pacf = TimeSeries.GetPACFFrom(acf);
            //crossed = pacf[0] > highInterval;
            int q = 0;
            low = pacf[0];
            for (int i = 0; i < pacf.Count; i++)
            {
                Console.WriteLine("interval: " + highInterval.ToString() + " pacf: " + pacf[i].ToString());
                //if ((pacf[i] > highInterval) != crossed)
                if (Math.Min(low, pacf[i]) <= highInterval && highInterval < Math.Max(pacf[i], low))
                {
                    q = i;
                    if (q != 0)
                    {
                        q--;
                    }
                    break;
                }
                low = pacf[i];
            }
            Console.WriteLine("p: " + p.ToString() + " q: " + q.ToString());
            var model = new ARMAModel(p, q);
            model.TheData = ts;
            model.FitByMLE(200, 100, 0, null);

            var forecaster = new ForecastTransform();
            var futureTimes = new List<DateTime>();
            var nextTime = ts.GetLastTime();
            var daysProjected = 8;
            for (int t = 0; t < daysProjected; ++t )
            {
                nextTime = nextTime.AddDays(1);
                futureTimes.Add(nextTime);
            }
            forecaster.FutureTimes = futureTimes.ToArray();

            forecaster.SetInput(0, model, null); 
            forecaster.SetInput(1, model.theData, null);

            var predictors = forecaster.GetOutput(0) as TimeSeries;

            Console.WriteLine("results: ");
            Console.WriteLine(predictors.GetDescription());
            Console.WriteLine(predictors[0]);
            Console.WriteLine(predictors[1]);

            // now predictors is a time series of the forecast values for the next 8 days
            // that is, predictors[0] is the predictive mean of Y_{101} given Y_1,...,Y_100,
            //          predictors[1] is the predictive mean of Y_{102} given Y_1,...,Y_100, etc.
        }

        public static T[] GetCol<T>(T[,] matrix, int col)
        {
            var colLength = matrix.GetLength(0);
            var colVector = new T[colLength];

            for (var i = 0; i < colLength; i++)
            colVector[i] = matrix[i, col];

            return colVector;
        }

        public DataObject[,] getSeries(List<string[]> data, int len)
        {
            // returns 2D array of the time series, with X being the first column and Y being the second column
            DataObject[,] series = new DataObject[len,2];
            for (int i = 0; i < len; i++)
            {
                series[i,0] = new DataObject(data[i][0], data[i][1], data[i][2]);
                series[i,1] = new DataObject(data[i][0], data[i][1], data[i][3]);
            }
            return series;
        }

        public bool testStationarity(DataObject[,] series, double siglevel)
        {
            Vector<double> X = Vector<double>.Build.Dense(series.GetLength(0));
            Vector<double> Y = Vector<double>.Build.Dense(series.GetLength(0));
            for (int i = 0; i < X.Count; i++)
            {
                X[i] = Double.Parse(series[i, 0].Value);
                Y[i] = Double.Parse(series[i, 1].Value);
            }
            ADF adftest = new stats.ADF(X, Y);
            return adftest.adfuller(siglevel);
        }

    }
}
