#region License Info
//Component of Cronos Package, http://www.codeplex.com/cronos
//Copyright (C) 2009 Anthony Brockwell

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
//of the License, or (at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
#endregion


using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABMath.Forms
{
    public partial class ForecastRangeForm : Form
    {
        public List<DateTime> GetRange()
        {
            var range = new List<DateTime>();
            var current = new DateTime(StartTime.Ticks);
            while (current <= EndTime)
            {
                bool skipit = false;
                if (SkipWeekends)
                    if (current.DayOfWeek == DayOfWeek.Sunday || current.DayOfWeek == DayOfWeek.Saturday)
                        skipit = true;
                if (!skipit)
                    range.Add(new DateTime(current.Ticks));
                current += Interval;
            }
            return range;
        }

        public DateTime StartTime
        {
            get { return dateTimePicker1.Value; }
            set { dateTimePicker1.Value = value; }
        }
        public DateTime EndTime
        {
            get { return dateTimePicker2.Value; }
            set { dateTimePicker2.Value = value; }
        }
        public TimeSpan Interval
        {
            get
            {
                var span = new TimeSpan((int) daysUpDown.Value, (int) hoursUpDown.Value,
                                        (int) minutesUpDown.Value, (int) secondsUpDown.Value);
                return span;
            }
            set
            {
                daysUpDown.Value = value.Days;
                hoursUpDown.Value = value.Hours;
                minutesUpDown.Value = value.Minutes;
                secondsUpDown.Value = value.Seconds;
            }
        }
        public bool SkipWeekends { 
            get { return skipWeekendsCheckBox.Checked; }
            set { skipWeekendsCheckBox.Checked = value;  }
        }

        public ForecastRangeForm(DateTime start, DateTime end, TimeSpan interval, bool skipWeekends)
        {
            InitializeComponent();

            StartTime = start;
            EndTime = end;
            Interval = interval;
            SkipWeekends = skipWeekends;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}