using System;
using System.Collections.Generic;
using ARIMA.dataprocessing;
using ABMath.ModelFramework.Models;
using System.Diagnostics;

namespace ARIMA.timeseries
{
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    using ABMath.ModelFramework.Data;

    using DataObject = ARIMA.dataprocessing.DataObject;

    public class TSAnalyzer
    {
        private static readonly string[] files =  {
                                                                  "MSFT.csv",
                                                                  "mean-daily-saugeen-river-flows.csv",
                                                                  "dailymintempmelbourne.csv"};

        private static readonly string[] parameters = {
                                                                   "Open",
                                                                   "saugeenRiverflows",
                                                                   "Dailymintemp"
                                                               };
        private static Form1 form1;


        // a RunDemo of the ARIMA model
        public static void RunDemo()
        {
            System.IO.DirectoryInfo outDirectoryInfo = System.IO.Directory.CreateDirectory("../../Output");

            char[] delimiters = new char[] { '/', '-' };
            Stopwatch watch = new Stopwatch();
            int i = 0;
            foreach (var xx in files.Zip(parameters, (a, b) => Tuple.Create(a, b)))
            {
                watch.Start();
                forecasttest("../../data/" + xx.Item1, outDirectoryInfo.FullName + "/" + xx.Item2 + ".csv", xx.Item2, delimiters[1], false, true, 0.9, i == 0, 0.05, false);
                watch.Stop();
                watch.Reset();
                Console.WriteLine($"dataset {xx.Item1}, variable, {xx.Item2}, {i} of {files.Length}, took: " + watch.Elapsed.TotalSeconds);
                i++;
            }
        }



        // given a file, x attr, y attr, date delimiter, has time boolean, how many values to include in the model, how many to predict, 
        // whether the data should be reversed to have the dates increasing, and the significance level for the stationarity test, 
        // the predicted values, the actual values, and the MAE, RMSE, and MAPE errors. 
        public static void forecasttest(string file, string outfile, string xattr, char delimiter, bool hastime, bool change, double split, bool reverse, double siglevel, bool log)
        {
            var data = GetData(file, xattr, hastime).Select(a => TimeSeriesWrapper.getDataObject(a, change));

            if (reverse)
            {
                data = data.Reverse();
            }

            var splitData = DivideData(data.ToArray(), split);

            TimeSeriesWrapper ts = new TimeSeriesWrapper();

            ARMAModel model = ts.buildARMAModel(splitData.Item1.ToArray(), siglevel, -1, log: log);

            List<DateTime> futureTimes = splitData.Item2.Select(a => a.Date).ToList();

            TimeSeries mergedtimeseries = ts.predictARMA(futureTimes, model);

            var points = GetPoints(mergedtimeseries, splitData);

            ChartPoints(file.Split('/').Last(), points.Item1, points.Item2);

            double[] errresults = TimeSeriesWrapper.evaluatePrediction(mergedtimeseries, splitData.Item2, outfile);
            Console.WriteLine("Mean absolute error (MAE): " + errresults[0]);
            Console.WriteLine("Root mean squared error (RMSE): " + errresults[1]);
            Console.WriteLine("Mean absolute percentage error (MAPE): " + errresults[2]);
        }


        private static void ChartPoints(string name, params IEnumerable<Point>[] points)
        {
            form1 = new Form1();
            System.Windows.Forms.DataVisualization.Charting.Chart chart = form1.Chart;
            chart.Name = name;
            for (int i = 0; i < points.Length; i++)
            {
                chart.Series.Add(i.ToString());
                chart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

                foreach (var point in points[i])
                {
                    chart.Series[i].Points.AddXY(point.X, point.Y);
                }
            }

            Application.Run(form1);

            // Save sin_0_2pi.png image file
            //chart.SaveImage("sin_0_2pi.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <param name="timeSeries">A l.</param>
        /// <param name="splitData">A k.</param>
        /// <returns>A f.</returns>
        private static Tuple<List<Point>, List<Point>> GetPoints(TimeSeries timeSeries, Tuple<List<DataObject>, List<DataObject>> splitData)
        {
            var defaultDate = splitData.Item1[0].Date;

            List<Point> points1 = new List<Point>();
            List<Point> points2 = new List<Point>();
            for (int i = 0; i < timeSeries.Count; i++)
            {

                if (i < splitData.Item1.Count)
                {
                    points1.Add(new Point((int)(splitData.Item1[i].Date - defaultDate).TotalDays, (int)(splitData.Item1[i].Value * 100)));
                }
                else
                {
                    points2.Add(new Point((int)(splitData.Item2[i - splitData.Item1.Count].Date - defaultDate).TotalDays, (int)(timeSeries[i] * 100)));
                }
              
            }

            return Tuple.Create(points1, points2);
        }

        private static List<string[]> GetData(string file, string variable, bool hastime)
        {
            CSVReader reader = new CSVReader();
            string[] headers = reader.getHeaders(file, ',');
            int xindex = headers.Select((h, i) => Tuple.Create(h, i))
                .Single(hi => hi.Item1.Equals(variable))
                .Item2;

            return reader.getData(xindex, 0, file, ',', hastime);
        }

        private static Tuple<List<T>, List<T>> DivideData<T>(IList<T> data, double ss)
        {
            List<T> listOne = new List<T>();
            List<T> listTwo = new List<T>();
            Random random = new Random();
            int threshold = (int)(data.Count * ss);
            for (int i = 0; i < data.Count; i++)
            {
                if (i > threshold)
                {
                    listTwo.Add(data[i]);
                }
                else
                {
                    listOne.Add(data[i]);
                }
            }

            return Tuple.Create(listOne, listTwo);
        }

        private static Tuple<List<T>, List<T>> SplitData<T>(IEnumerable<T> data, double ss)
        {
            List<T> listOne = new List<T>();
            List<T> listTwo = new List<T>();
            Random random = new Random();
            foreach (var row in data)
            {
                if (random.NextDouble() > ss)
                {
                    listTwo.Add(row);
                }
                else
                {
                    listOne.Add(row);
                }
            }

            return Tuple.Create(listOne, listTwo);
        }
    }
}
