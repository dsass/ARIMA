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
            TSAnalyzer analyzer = new TSAnalyzer();
            analyzer.demo();
            while (true) { } // to be able to see the output from debugging
        }
    }
}
