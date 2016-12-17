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
            string[] headers = header.Split(delimiter).Select(s => s.Trim()).Where(s => s != String.Empty).ToArray();
            return headers;                    
        }

        // returns the data in a list of string arrays in the form of [date, time, xvalue, yvalue] if the dataset contains both a date and a time
        public List<string[]> getData(int xindex, int yindex, int dateindex, string filename, char delimiter, bool hastime, int timeindex=1)
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
                if (hastime)
                {
                    try
                    {
                        double val1 = Double.Parse(values[xindex]);
                        double val2 = Double.Parse(values[yindex]);
                    }
                    catch (System.FormatException)
                    {
                        continue;
                    } finally
                    {
                        string[] linedata = new string[] { values[dateindex], values[timeindex], values[xindex], values[yindex] };
                        data.Add(linedata);
                    }
                }
                else
                {
                    try
                    {
                        double val1 = Double.Parse(values[xindex]);
                        double val2 = Double.Parse(values[xindex]);
                    }
                    catch (System.FormatException)
                    {
                        continue;
                    }
                    finally
                    {
                        string[] linedata = new string[] { values[dateindex], values[xindex], values[yindex] };
                        data.Add(linedata);
                    }
                }
            }
            return data;
        }

    }
}
