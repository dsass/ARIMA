using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARIMA.dataprocessing;
using ARIMA.timeseries.stats;
using MathNet.Numerics.LinearAlgebra;
using ARIMA.cronos-ARMA.ABMath.ModelFramework;
using ARIMA.timeseries.math.linalg;
using cronos-ARMA.ABMath.ModelFramework.Transforms;


namespace ARIMA.timeseries
{
    class TimeSeries
    {
        private string xattr;
        private string yattr;
        private string file;

        public TimeSeries(string xattribute, string yattribute, string filename)
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
            //for (int i = 0; i < 50; i++)


            int d = 0;
            if (testStationarity(series, siglevel))
            {
                d = 1;
            }

            var tSeries = series;
            
            //Difference the series
            if (d != 0)
            {
                tSeries = ArrayManipulation.diff(GetCol(tSeries, 1));
            }

            var ts = new Data.TimeSeries 
                                {
                                    Title = "Time Series",
                                    Description = "TS Description"
                                };
            for (int i = 0, i < series.Count; i++)
            {
                ts.Add(tSeries[i,0], tSeries[i,1], false); //datetime, value, false
            }

            //TODO log transform the time series
        
            //Use ACF and PACF to find ARMA parameters
            var highInterval = 1.96/Math.Sqrt(series.Count);
            var acf = ts.ComputeACF(20, false);
            bool crossed = acf[0] > highInterval;
            int p = 0;
            for (int i; i < acf.Count; i++) 
            {
                if ((acf[i] > highInterval) != crossed)
                {
                    p = i;
                }
            }
            var pacf = ts.GetPACFFrom(acf);
            crossed = pacf[0] > highInterval;
            int q = 0;
            for (int i; i < pacf.Count; i++);
            {
                if ((pacf[i] > highInterval) != crossed)
                {
                    q = i;
                }
            }

            var model = ARMAModel(p, q);
            model.theData = ts;
            model.FitByMLE(200, 100, 0, null);

            var forecaster = new ForecastTransform();
            var futureTimes = new List<DateTime>();
            var nextTime = ts.GetLastTime();
            var daysProjected = 8
            for (int t = 0; t < daysProjected; ++t )
            {
                nextTime = nextTime.AddDays(1);
                futureTimes.Add(nextTime);
            }
            forecaster.FutureTimes = futureTimes.ToArray();

            forecaster.SetInput(0, model, null); 
            forecaster.SetInput(1, simulatedData, null);

            var predictors = forecaster.GetOutput(0) as TimeSeries;

            // now predictors is a time series of the forecast values for the next 8 days
            // that is, predictors[0] is the predictive mean of X_{101} given X_1,...,X_100,
            //          predictors[1] is the predictive mean of X_{102} given X_1,...,X_100, etc.
        }

        public static T[] GetCol<T>(this T[,] matrix, int col)
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
