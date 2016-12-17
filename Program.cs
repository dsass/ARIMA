using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARIMA.timeseries;

namespace ARIMA
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length < 1)
            //{
            //    Console.WriteLine("ERROR: Please specify a dataset contained in a .csv or a .txt file" +
            //        "structured like a .csv file.");
            //    return;
            //}
            //string file = args[0];
            //string file = "C:/Users/Steffani/Documents/Visual Studio 2015/Projects/ARIMA/data/household_power_consumption.txt";
            //TimeSeriesWrapper ts = new TimeSeriesWrapper("Global_active_power", "Global_intensity", file);
            //ts.beginARMAProcess(';', 0.05);
            TSAnalyzer analyzer = new TSAnalyzer();
            analyzer.demo();
            while (true) { } // to be able to see the output from debugging
        }
    }
}
