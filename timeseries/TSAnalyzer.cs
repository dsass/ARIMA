using System;
using System.Collections.Generic;
using ARIMA.dataprocessing;
using ABMath.ModelFramework.Models;
using ABMath.ModelFramework.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ARIMA.timeseries
{
    public class TSAnalyzer
    {
        // given a file, x attr, y attr, date delimiter, has time boolean, how many values to include in the model, how many to predict, 
        // whether the data should be reversed to have the dates increasing, and the significance level for the stationarity test, 
        // the predicted values, the actual values, and the MAE, RMSE, and MAPE errors. 
        public void forecasttest(string file, string outfile, string xattr, char delimiter, bool hastime, bool change, double inmodel, double numpredict, bool reverse, double siglevel, bool log)
        {
            CSVReader reader = new CSVReader();
            string[] headers = reader.getHeaders(file, ',');
            int xindex = -1;
            for (int j = 0; j < headers.Length; j++)
            {
                if (String.Compare(headers[j], xattr) == 0)
                {
                    xindex = j;
                }
            }
            List<String[]> data = new List<String[]>();
            data = reader.getData(xindex, 0, file, ',', hastime);
            if (numpredict < 1)
            {
                numpredict = (int)(data.Count * numpredict);
            }
            if (inmodel < 1)
            {
                inmodel = (int)(data.Count * inmodel);
            }
            int trainnum = (int)inmodel;
            Console.WriteLine("Model will contain " + trainnum.ToString() + " events.");
            List<double> predicted = new List<double>();
            TimeSeriesWrapper ts = new TimeSeriesWrapper();
            DataObject[] series = new DataObject[1];
            if (reverse)
            {
                data.Reverse();
                series = ts.getSeries(data, trainnum, change);
            }
            else
            {
                series = ts.getSeries(data, trainnum, change);
            }
            ARMAModel model = ts.beginARMAProcess(series, siglevel, -1, log: log);
            DataObject[] Y = series;
            List<DateTime> futureTimes = new List<DateTime>();
            int totaldata = trainnum + (int)numpredict;
            DateTime last = ((TimeSeries)model.TheData).GetLastTime();
            Console.WriteLine(last.ToString());
            DataObject[] yactual = ts.getSeries(data, (int)numpredict, change, start: trainnum);
            for (int t = 0; t < yactual.Length; ++t)
            {
                futureTimes.Add(yactual[t].Date);
            }
            var mergedtimeseries = ts.predictARMA(futureTimes, model);
            double[] errresults = TimeSeriesWrapper.evaluatePrediction(mergedtimeseries, yactual, futureTimes, outfile, trainnum);
            Console.WriteLine("Mean absolute error (MAE): " + errresults[0].ToString());
            Console.WriteLine("Root mean squared error (RMSE): " + errresults[1].ToString());
            Console.WriteLine("Mean absolute percentage error (MAPE): " + errresults[2].ToString());
        
        }

        // a demo of the ARIMA model
        public void demo()
        {
            string[] files = new string[] { "../../data/consumption.csv",
                "../../data/MSFT.csv",
                "../../data/mean-daily-saugeen-river-flows.csv",
                "../../data/dailymintempmelbourne.csv"};
            string[] xattrs = new string[] { "Voltage", "Open", "saugeenRiverflows", "Dailymintemp" };
            char[] delimiters = new char[] { '/', '-' };
            Stopwatch watch = new Stopwatch();
            watch.Start();
            forecasttest(files[0], "C:/Users/Steffani/Desktop/" + xattrs[0] + ".csv", xattrs[0], delimiters[0], true, false, 0.01, 0.001, false, 0.05, false);
            watch.Stop();
            Console.WriteLine("First dataset took: " + watch.Elapsed.TotalSeconds.ToString());
            watch = new Stopwatch();
            watch.Start();
            forecasttest(files[1], "C:/Users/Steffani/Desktop/" + xattrs[1] + ".csv", xattrs[1], delimiters[1], false, true, 0.9, 0.1, true, 0.05, false);
            watch.Stop();
            Console.WriteLine("Second dataset took: " + watch.Elapsed.TotalSeconds.ToString());
            watch = new Stopwatch();
            watch.Start();
            forecasttest(files[2], "C:/Users/Steffani/Desktop/" + xattrs[2] + ".csv", xattrs[2], delimiters[1], false, true, 0.9, 0.1, false, 0.05, false);
            watch.Stop();
            Console.WriteLine("Third dataset took: " + watch.Elapsed.TotalSeconds.ToString());
            watch = new Stopwatch();
            watch.Start();
            forecasttest(files[3], "C:/Users/Steffani/Desktop/" + xattrs[3] + ".csv", xattrs[3], delimiters[1], false, true, 0.9, 0.1, false, 0.05, false);
            watch.Stop();
            Console.WriteLine("Fourth dataset took: " + watch.Elapsed.TotalSeconds.ToString());
        }
    }
}
