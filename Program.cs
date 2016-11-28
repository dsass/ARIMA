using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARIMA
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("ERROR: Please specify a dataset contained in a .csv");
                return;
            }
            string file = args[0];
            ARIMA.dataprocessing.CSVReader reader = new ARIMA.dataprocessing.CSVReader();
            string[] headers = reader.getHeaders(file);
            List<String[]> data = reader.getData(0, 1, 2, file);
        }
    }
}
