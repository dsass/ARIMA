using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ARIMA.timeseries.models
{
    // super class of model that obtains the x and y data and ensures that all sub classes have a fit method.

    public class Model
    {
        static protected Matrix<double> X;
        static protected Vector<double> Y;

        public Model(Matrix<double> Xdata, Vector<double> Ydata)
        {
            X = Xdata;
            Y = Ydata ;
        }

        string fit()
        {
            throw new NotImplementedException();
        }

    }
}
