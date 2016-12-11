using System;
using ABMath.ModelFramework.Data;

namespace ABMath.ModelFramework.Models
{
    public interface IRealTimePredictable
    {
        void ResetRealTimePrediction();
        void Register(TimeSeries series);
        double Register(DateTime timeStamp, double value);
        double Register(DateTime timeStamp, double value, double[] auxiliaryValues);
        DistributionSummary GetCurrentPredictor(DateTime futureTime);
    }
}
