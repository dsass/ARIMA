using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARIMA.dataprocessing;
using ARIMA.timeseries.stats;
using MathNet.Numerics.LinearAlgebra;

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
            CSVReader reader = new ARIMA.dataprocessing.CSVReader();
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
