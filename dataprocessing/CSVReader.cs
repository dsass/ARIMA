using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARIMA.dataprocessing
{
    class CSVReader
    {
        public string[] getHeaders(string filename, char delimiter)
        {
            System.IO.StreamReader filereader = null;
            try
            {
                filereader = new System.IO.StreamReader(@filename);
            } catch (System.IO.FileNotFoundException)
            {
                return null;
            }
            string header = filereader.ReadLine();
            string[] headers = header.Split(delimiter).Select(s => s.Trim()).Where(s => s != String.Empty).ToArray();
            return headers;                    
        }

        // returns the data in a list of string arrays in the form of [date, time, xvalue, yvalue]
        public List<string[]> getData(int xindex, int yindex, int dateindex, int timeindex, string filename, char delimiter)
        {
            System.IO.StreamReader reader = null;
            try
            {
                reader = new System.IO.StreamReader(@filename);
            } catch (System.IO.FileNotFoundException)
            {
                return null;
            }
            string line = reader.ReadLine();
            List<String[]> data = new List<String[]>();
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(delimiter).Select(s => s.Trim()).Where(s => s != String.Empty).ToArray();
                string[] linedata = new string[] { values[dateindex], values[timeindex], values[xindex], values[yindex] };
                data.Add(linedata);
            }
            return data;
        }

    }
}
