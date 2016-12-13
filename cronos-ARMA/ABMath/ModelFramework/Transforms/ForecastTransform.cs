using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ABMath.Forms;
using ABMath.ModelFramework.Data;
using ABMath.ModelFramework.Models;

namespace ABMath.ModelFramework.Transforms
{
    [Serializable]
    public class ForecastTransform : TimeSeriesTransformation, IExtraFunctionality
    {
        [Category("Parameter"), Description("Times at which to generate forecasts")]
        public DateTime[] FutureTimes { get; set; }

        public ForecastTransform()
        {
            FutureTimes = new DateTime[10];
            for (int i=0 ; i<10 ; ++i)
                FutureTimes[i] = new DateTime(2011, 1, (i + 1)); // default
        }

        public override int NumInputs()
        {
            return 2; // model and starting data
        }

        public override int NumOutputs()
        {
            return 1; // predictive means
        }

        public override string GetInputName(int index)
        {
            if (index == 0)
                return "Model";
            if (index == 1)
                return "Starting Data";
            throw new SocketException();
        }

        public override string GetOutputName(int index)
        {
            if (index == 0)
                return "Predictive Mean";
            throw new SocketException();
        }

        public override string GetDescription()
        {
            return "Forecasts";
        }

        public override string GetShortDescription()
        {
            return "Forecasts";
        }

        //public override Icon GetIcon()
        //{
        //    return null;
        //}

        public override bool SetInput(int socket, object item, StringBuilder failMsg)
        {
            CheckInputsReady();
            if (socket >= NumInputs())
                throw new ArgumentException("Bad socket.");

            if (socket == 0)
                if (item as TimeSeriesModel == null)
                {
                    if (failMsg != null)
                        failMsg.Append("The first input must be a model.");
                    return false;
                }
            if (socket == 1)
                if (item as TimeSeries == null)
                    return false;

            socketedInputs[socket] = item;

            if (AllInputsValid())
                Recompute();

            return true;
        }

        public override void Recompute()
        {
            outputs = new List<TimeSeries>();
            IsValid = false;

            if (FutureTimes == null)
                return;
            if (FutureTimes.Length == 0)
                return;

            var tsm = GetInput(0) as UnivariateTimeSeriesModel;
            var tsd = GetInput(1) as TimeSeries;

            if (tsm == null || tsd == null)
                return;

            var fdt = new List<DateTime>();
            foreach (DateTime dt in FutureTimes)
                fdt.Add(dt);
            var forecasts = tsm.BuildForecasts(tsd, fdt) as TimeSeriesBase<DistributionSummary>;
            if (forecasts == null)
                return;

            var predictiveMeans = new TimeSeries();
            for (int t = 0; t < forecasts.Count; ++t)
                predictiveMeans.Add(forecasts.TimeStamp(t), forecasts[t].Mean, false);

            outputs.Add(predictiveMeans);
            IsValid = predictiveMeans.Count > 0;
        }

        public int NumAuxiliaryFunctions()
        {
            return 1;
        }

        public string AuxiliaryFunctionName(int index)
        {
            if (index == 0)
                return "Specify Times";
            return null;
        }

        public string AuxiliaryFunctionHelp(int index)
        {
            if (index == 0)
                return "Specifies time points in the future at which forecasts will be generated.";
            return null;
        }

        public bool AuxiliaryFunction(int idx, out object output)
        {
            output = null;
            switch (idx)
            {
                case 0:
                    var ts = GetInput(1) as TimeSeries;
                    if (ts == null)
                        return false;

                    var interval = ts.GetCommonSamplingInterval();
                    var first = ts.GetLastTime() + interval;
                    var last = ts.GetLastTime() + new TimeSpan(interval.Ticks*20);
                    var futureForm = new ForecastRangeForm(first, last, interval, true)
                                         {StartPosition = FormStartPosition.CenterParent};
                    if (futureForm.ShowDialog() == DialogResult.OK)
                    {
                        List<DateTime> range = futureForm.GetRange();
                        FutureTimes = new DateTime[range.Count];
                        for (int i = 0; i < range.Count; ++i)
                            FutureTimes[i] = range[i];
                        return true;
                    }
                    break;
            }
            return false;
        }

        public override List<Type> GetAllowedInputTypesFor(int socket)
        {
            if (socket == 0)
                return new List<Type> { typeof(Model) };
            if (socket == 1)
                return new List<Type> { typeof(TimeSeries), typeof(MVTimeSeries) };
            throw new SocketException();
        }

        public override List<Type> GetOutputTypesFor(int socket)
        {
            if (socket >= NumOutputs())
                throw new SocketException();
            return new List<Type> { typeof(TimeSeries), typeof(MVTimeSeries) };
        }
    }
}