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
        public DateTime Date { get; }
        // the actual value of one value of a time series
        public double Value { get; set; }
        // the delimiter of how the date should be split, if in year/month/day, then delimiter is /
        private char datedelimiter;
        // the delimiter for how the time should be split
        private char timedelimiter;

        // assuming that date is in fullyear/month/day format
        // assuming that time is in hour:minute:second in 24 hour format
        public DataObject(string datestring, string time, double value, char ddelimiter='/', char tdelimiter=':',
            int dateyear=2, int datemonth=1, int dateday=0, int timehour=0, int timemin=1, int timesec=2, bool hastime=true)
        {
            Value = value;
            datedelimiter = ddelimiter;
            timedelimiter = tdelimiter;
            string[] dateparts = datestring.Split(datedelimiter);
            string[] timeparts = time.Split(timedelimiter);
            if (hastime)
            {
                Date = new DateTime(Int32.Parse(dateparts[dateyear]), Int32.Parse(dateparts[datemonth]), Int32.Parse(dateparts[dateday]),
                    Int32.Parse(timeparts[timehour]), Int32.Parse(timeparts[timemin]), Int32.Parse(timeparts[timesec]));
            } else
            {
                Date = new DateTime(Int32.Parse(dateparts[dateyear]), Int32.Parse(dateparts[datemonth]), Int32.Parse(dateparts[dateday]));
            }
        }

        // returns a string version of this object
        public override string ToString()
        {
            return "Date: " + Date.ToString() + "\nValue: " + Value.ToString();
        }
    }
}
