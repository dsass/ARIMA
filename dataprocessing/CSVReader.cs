/**
 * This class handles reading the data from the csv, representing a row as a string array
 * meant to mimic python's csvreader
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARIMA.dataprocessing
{
    public class CSVReader
    {
        // returns an array of strings representing the different fields of the csv file
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
            header = header.Replace('"', ' ');
            string[] headers = header.Split(delimiter).Select(s => s.Trim()).Where(s => s != String.Empty).ToArray();
            return headers;                    
        }

        // returns the data in a list of string arrays in the form of [date, time, xvalue, yvalue] if the dataset contains both a date and a time
        public List<string[]> getData(int xindex, int dateindex, string filename, char delimiter, bool hastime, int timeindex=1)
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
                line = line.Replace('"', ' ');
                string[] values = line.Split(delimiter).Select(s => s.Trim()).Where(s => s != String.Empty).ToArray();
                if (hastime)
                {
                    try
                    {
                        if (values.Length == 0)
                        {
                            throw new System.FormatException();
                        }
                        double val1 = Double.Parse(values[xindex]);
                    }
                    catch (System.FormatException)
                    {
                        continue;
                    } 
                    string[] linedata = new string[] { values[dateindex], values[timeindex], values[xindex] };
                    data.Add(linedata);
                }
                else
                {
                    try
                    {
                        if (values.Length == 0)
                        {
                            throw new System.FormatException();
                        }
                        double val1 = Double.Parse(values[xindex]);
                    }
                    catch (System.FormatException)
                    {
                        continue;
                    }
                    string[] linedata = new string[] { values[dateindex], values[xindex] };
                    data.Add(linedata);                   
                }
            }
            return data;
        }

    }
}
