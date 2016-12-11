using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ARIMA.timeseries.models
{
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

        string predict()
        {
            //if the intent is to re-initialize the model with new data then 
            // this method needs to take inputs...
            throw new NotImplementedException();
        }
        public virtual Matrix<double> loglike(Matrix<double> param)
        {
            //def loglike(self, params):
            //Log-likelihood of model.
            throw new NotImplementedException();
        }
        double[] score()
        {
            // Score vector of model.
            //The gradient of logL with respect to each parameter.
            throw new NotImplementedException();
        }
        //def score(self, params):
        // Score vector of model.
        //The gradient of logL with respect to each parameter.
        double[,] information()
        {
            //def information(self, params):
            //Fisher information matrix of model
            //Returns -Hessian of loglike evaluated at params.
            throw new NotImplementedException();
        }
        double[,] hessian()
        {
            //def hessian(self, params):
            //The Hessian matrix of the model
            //def predict(self, params, exog= None, * args, ** kwargs)
            throw new NotImplementedException();
        }

    }
}
