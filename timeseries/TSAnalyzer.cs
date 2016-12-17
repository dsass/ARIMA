using System;
using System.Collections.Generic;
using ARIMA.dataprocessing;
using ABMath.ModelFramework.Models;
using ABMath.ModelFramework.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARIMA.timeseries
{
    public class TSAnalyzer
    {
        // given a file, x attr, y attr, date delimiter, has time boolean, how many values to include in the model, how many to predict, 
        // whether the data should be reversed to have the dates increasing, and the significance level for the stationarity test, 
        // the predicted values, the actual values, and the MAE, RMSE, and MAPE errors. 
        public void forecasttest(string file, string xattr, string yattr, char delimiter, bool hastime, bool change, double inmodel, double numpredict, bool reverse, double siglevel)
        {
            CSVReader reader = new CSVReader();
            string[] headers = reader.getHeaders(file, ',');
            int xindex = -1;
            int yindex = -1;
            for (int j = 0; j < headers.Length; j++)
            {
                if (String.Compare(headers[j], xattr) == 0)
                {
                    xindex = j;
                }
                else if (String.Compare(headers[j], yattr) == 0)
                {
                    yindex = j;
                }
            }
            List<String[]> data = new List<String[]>();
            data = reader.getData(xindex, yindex, 0, file, ',', hastime);
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
            DataObject[,] series = new DataObject[5, 5];
            if (reverse)
            {
                data.Reverse();
                series = ts.getSeries(data, trainnum, change);
            }
            else
            {
                series = ts.getSeries(data, trainnum, change);
            }
            ARMAModel model = ts.beginARMAProcess(series, siglevel, -1);
            DataObject[] Y = TimeSeriesWrapper.GetCol<DataObject>(series, 1);
            List<DateTime> futureTimes = new List<DateTime>();
            int totaldata = trainnum + (int)numpredict;
            DateTime last = ((TimeSeries)model.TheData).GetLastTime();
            Console.WriteLine(last.ToString());
            DataObject[] yactual = TimeSeriesWrapper.GetCol(ts.getSeries(data, (int)numpredict, change, start: trainnum), 1);
            for (int t = 0; t < yactual.Length; ++t)
            {
                futureTimes.Add(yactual[t].Date);
            }
            var mergedtimeseries = ts.predictARMA(futureTimes, model);
            double[] errresults = TimeSeriesWrapper.evaluatePrediction(mergedtimeseries, yactual, futureTimes, trainnum);
            Console.WriteLine("Mean absolute error (MAE): " + errresults[0].ToString());
            Console.WriteLine("Root mean squared error (RMSE): " + errresults[1].ToString());
            Console.WriteLine("Mean absolute percentage error (MAPE): " + errresults[2].ToString());
        
        }

        // a demo of the ARIMA model
        public void demo()
        {
            string[] files = new string[] { "C:/Users/Steffani/Documents/Visual Studio 2015/Projects/ARIMA/data/consumption.csv",
                "C:/Users/Steffani/Documents/Visual Studio 2015/Projects/ARIMA/data/MSFT.csv" };
            string[] xattrs = new string[] { "Voltage", "Open" };
            string[] yattrs = new string[] { "Global_intensity", "Close" };
            char[] delimiters = new char[] { '/', '-' };
            forecasttest(files[0], xattrs[0], yattrs[0], delimiters[0], true, false, 0.001, 100, false, 0.05);
            forecasttest(files[1], xattrs[1], yattrs[1], delimiters[1], false, true, 0.9, 0.1, true, 0.05);
        }
    }
}
