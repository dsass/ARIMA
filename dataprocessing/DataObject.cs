using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARIMA.dataprocessing
{
    public class DataObject
    {
        // the date of one value of a time series
        private DateTime date;
        // the actual value of one value of a time series
        private string val;
        private char datedelimiter = '/';
        private char timedelimiter = ':';

        // assuming that date is in day/month/fullyear format
        // assuming that time is in hour:minute:second in 24 hour format
        public DataObject(string datestring, string time, string value)
        {
            val = value;
            string[] dateparts = datestring.Split(datedelimiter);
            string[] timeparts = time.Split(timedelimiter);
            date = new DateTime(Int32.Parse(dateparts[2]), Int32.Parse(dateparts[1]), Int32.Parse(dateparts[0]),
                Int32.Parse(timeparts[0]), Int32.Parse(timeparts[1]), Int32.Parse(timeparts[2]));
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
        }

        public string Value
        {
            get
            {
                return String.Copy(val);
            }
            set
            {
                val = value;
            }
        }
    }
}
