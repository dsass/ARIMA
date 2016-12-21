using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARIMA.dataprocessing;
using ARIMA.timeseries.stats;
using MathNet.Numerics.LinearAlgebra;
using ARIMA.timeseries.math.linalg;
using ABMath.ModelFramework;
using ABMath.ModelFramework.Transforms;
using ABMath.ModelFramework.Data;
using ABMath.ModelFramework.Models;
using System.Diagnostics;
using System.IO;

namespace ARIMA.timeseries
{
    public class TimeSeriesWrapper
    {

        // this method fits an ARMA model to the data and returns it, in the process testing for non-stationarity and calculating the orders of the
        // MA and AR terms of the model
        public ARMAModel beginARMAProcess(DataObject[] series, double siglevel, int len=50, bool print=true, bool log=false)
        {
            bool nonstationarity = testStationarity(series, siglevel);
            if (print)
            {
                Console.WriteLine("non-stationarity: " + nonstationarity.ToString());
            }

            int d = 0;
            if (nonstationarity)
            {
                d = 1;
            }

            var tSeries = series;
            
            //Difference the series
            if (d != 0)
            {
                double[] dcol = new double[tSeries.Length];
                for (int i = 0; i < dcol.Length; i++)
                {
                    dcol[i] = (tSeries[i].Value);
                }
                double[] xdiff = ArrayManipulation.diff(dcol);
                for (int i = dcol.Length - xdiff.Length; i < xdiff.Length; i++)
                {
                    tSeries[i].Value = xdiff[i];
                }
            }

            var ts = new TimeSeries 
                                {
                                    Title = "Time Series",
                                    Description = "TS Description"
                                };
            for (int i = 0; i < series.GetLength(0); i++)
            {
                ts.Add(tSeries[i].Date, (tSeries[i].Value), false); //datetime, value, false
            }

            if (log)
            {
                //log transform the time series
                var logTransform = new LogTransform();
                logTransform.SetInput(0, ts, null);
                logTransform.Recompute();
                ts = logTransform.GetOutput(0) as TimeSeries;
            }

            //Use ACF and PACF to find ARMA parameters
            var highInterval = 1.96/Math.Sqrt(series.Length);
            var acf = ts.ComputeACF(20, false);
            int p = 1;
            var low = acf[0];
            for (int i = 0; i < acf.Count; i++) 
            {
                if (Math.Min(low, acf[i]) <= highInterval && highInterval < Math.Max(acf[i], low))
                {
                    p = i;
                    if (p != 0)
                    {
                        p--;
                    }
                    if (p == 0)
                    {
                        p = 1;
                    }
                    break;
                }
                low = acf[i];
            }
            var pacf = TimeSeries.GetPACFFrom(acf);
            int q = 1;
            low = pacf[0];
            for (int i = 0; i < pacf.Count; i++)
            {
                if (Math.Min(low, pacf[i]) <= highInterval && highInterval < Math.Max(pacf[i], low))
                {
                    q = i;
                    if (q != 0)
                    {
                        q--;
                    } 
                    if (q == 0)
                    {
                        q = 1;
                    }
                    break;
                }
                low = pacf[i];
            }
            if (p > 4)
            {
                p = 4;
            }
            if (q > 4)
            {
                q = 4;
            }
            if (print)
            {
                Console.WriteLine("p: " + p.ToString() + " q: " + q.ToString());
                Console.WriteLine("Fitting the model...");
            }
            var model = new ARMAModel(p, q);
            model.TheData = ts;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            model.FitByMLE(200, 100, 0, null);
            watch.Stop();
            if (print)
            {
                Console.WriteLine("Model took " + watch.Elapsed.TotalSeconds.ToString() + " seconds to fit parameters to the data.");
                Console.WriteLine("Model has been fitted.");
            }

            return model;
        }

        // forecasts values for the given dates from the model
        public TimeSeries predictARMA(List<DateTime> futureTimes, ARMAModel model)
        {
            var forecaster = new ForecastTransform();
            forecaster.FutureTimes = futureTimes.ToArray();

            forecaster.SetInput(0, model, null);
            forecaster.SetInput(1, model.TheData, null);

            var predictors = forecaster.GetOutput(0) as TimeSeries;

            // now we need to merge the forecast time series and the original time series with the original data

            var merger = new MergeTransform();
            merger.SetInput(0, model.TheData, null);
            merger.SetInput(1, predictors, null);

            merger.Recompute();

            return merger.GetOutput(0) as TimeSeries;
        }

        // evaluates the predicted values to the actual values and saves them to the file specified by the string filepath and prints out the error metrics
        public static double[] evaluatePrediction(TimeSeries ts, DataObject[] data, List<DateTime> predictdates, string filepath, int offset=0, bool log=false)
        {
            double mae = 0;
            double rmse = 0;
            double mape = 0;

            double absmean = 0;
            double sqmean = 0;
            double p = 0;
            Double[] error = new double[data.Count()];
            var csv = new StringBuilder();
            var newLine = string.Format("{0},{1},{2}", "Date", "Predicted", "Actual");
            csv.AppendLine(newLine);
            for (int i = 0; i < data.Count(); i++)
            {
                var predictedval = ts[offset + i];
                if (log)
                {
                    predictedval = Math.Pow(2, predictedval);
                }
                newLine = string.Format("{0},{1},{2}", predictdates[i].ToString(), predictedval.ToString(), data[i].Value.ToString());
                csv.AppendLine(newLine);
                error[i] =  predictedval - (data.ElementAt(i).Value);
                absmean = absmean + Math.Abs(error[i]);
                sqmean = sqmean + Math.Pow(error[i], 2);
                var p_i = (100 * error[i]) / (data.ElementAt(i).Value);
                p = p + Math.Abs(p_i);
            }
            File.WriteAllText(filepath, csv.ToString());
            mae = absmean/error.Length;
            rmse = Math.Sqrt(sqmean/error.Length);
            mape = p/error.Length;
            
            Double[] toReturn = new Double[3];
            toReturn[0] = mae;
            toReturn[1] = rmse;
            toReturn[2] = mape;
            return toReturn;
        }

        // returns array of the time series, with X being the first column
        // given data read from a csv file
        public DataObject[] getSeries(List<string[]> data, int len, bool change=false, int start=0)
        {
            if (len < 0)
            {
                len = data.Count;
            }
            DataObject[] series = new DataObject[len];
            for (int i = start; i < len + start; i++)
            {
                double val1 = -1;
                try
                {
                    val1 = Double.Parse(data[i][data[i].Length - 1]);
                } catch (System.FormatException)
                {
                    continue;
                }
                // hard coded the different delimiters for the date and time according to the different datasets
                if (change)
                {
                    series[i - start] = new DataObject(data[i][0], data[i][1], val1, hastime: false, ddelimiter: '-', dateyear: 0, dateday: 2);
                }
                else
                {
                    series[i - start] = new DataObject(data[i][0], data[i][1], val1);
                }
            }
            return series;
        }

        // tests the stationarity of the DataObject series at the designated significance level
        public bool testStationarity(DataObject[] series, double siglevel)
        {
            Vector<double> X = Vector<double>.Build.Dense(series.Length);
            for (int i = 0; i < X.Count; i++)
            {
                X[i] = series[i].Value;
            }
            ADF adftest = new stats.ADF(X);
            return adftest.adfuller(siglevel);
        }

    }
}
