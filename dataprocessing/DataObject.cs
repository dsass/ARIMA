using System;

namespace ARIMA.dataprocessing
{
    public struct DataObject
    {
        public DataObject(DateTime date, double value)
        {
            this.Date = date;
            this.Value = value;
        }

        /// <summary>
        /// Gets the date of one value of a time series.
        /// </summary>
        public DateTime Date { get;  }

        /// <summary>
        /// Gets the actual value of one value of a time series.
        /// </summary>
        public double Value { get;  }


        /// <inheritdoc cref="ToString"/>
        public override string ToString() => "Date: " + Date.ToString() + "\nValue: " + Value.ToString();
        
    }
}
