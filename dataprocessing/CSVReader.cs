using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARIMA.dataprocessing
{
    class CSVReader
    {
        public string[] getHeaders(string filename)
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
            string[] headers = header.Split(',').Select(s => s.Trim()).Where(s => s != String.Empty).ToArray();
            return headers;                    
        }

        public List<string> getData(int attrindex, string filename)
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
            List<String> data = new List<String>();
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(',').Select(s => s.Trim()).Where(s => s != String.Empty).ToArray();
                data.Add(values[attrindex]);
            }
            return data;
        }

    }
}
