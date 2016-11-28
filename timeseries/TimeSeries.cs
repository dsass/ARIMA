using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARIMA.dataprocessing;

namespace ARIMA.timeseries
{
    class TimeSeries
    {
        //private string attr;

        //public TimeSeries(string attribute, List<string[]> data)
        //{
        //    attr = attribute;
        //}

        //public string Attr
        //{
        //    get
        //    {
        //        return String.Copy(attr);
        //    }
        //}

        public List<DataObject> getSeries(List<string[]> data)
        {
            List<DataObject> series = new List<DataObject>();
            foreach (var line in data)
            {
                series.Add(new DataObject(line[0], line[1], line[2]));
            }
            return series;
        }
    }
}
