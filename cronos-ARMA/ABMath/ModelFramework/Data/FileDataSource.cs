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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ABMath.ModelFramework.Data
{
    /// <summary>
    /// This data source allows connection to different file types.
    /// For now, only csv or tsv files are supported.
    /// </summary>
    [Serializable]
    public class FileDataSource : DataSource
    {
        [Category("Parameter"), Description("File name")]
        public string FileName { get; set; }

        public override bool RefreshFromSource(out string infoMsg)
        {
            // load from file
            //Data = null;
            //infoMsg = "Failure";
            //return false;

            infoMsg = "Failure";
            if (string.IsNullOrEmpty(FileName))
                return false;

            List<TimeSeries> collection = null;
            try
            {
                using (var fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        using (var sreader = new StreamReader(fs))
                            collection = TimeSeries.GetTSFromReader(sreader, false);
                    }
                    catch (Exception loadException)
                    {
                        if (loadException.Message.Substring(0, 9) == "Duplicate")
                        {
                            var result = MessageBox.Show(loadException.Message + Environment.NewLine
                                                         + "Try again and ignore duplicates?", "Problem",
                                                         MessageBoxButtons.OKCancel);
                            if (result == DialogResult.OK)
                            {
                                using (
                                    var sreader =
                                        new StreamReader(new FileStream(FileName, FileMode.Open,
                                                                        FileAccess.Read, FileShare.Read)))
                                    collection = TimeSeries.GetTSFromReader(sreader, true);
                            }
                        }
                        else
                        {
                            collection = null;
                            infoMsg = loadException.Message;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                infoMsg = e.Message;
                collection = null;
                Data = null;
            }
            if (collection != null)
            {
                infoMsg = "Success";
                if (collection.Count == 1)
                    Data = collection[0];
                else if (collection.Count > 1)
                    Data = new MVTimeSeries(collection, false);
                else
                    infoMsg = "Failure";
            }

            return Data != null;
        }
    }
}
