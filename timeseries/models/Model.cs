using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ARIMA.timeseries.models
{
    class Model
    {
        protected Matrix<double> enddata;
        protected Matrix<double> exogdata;

        public Model(Matrix<double> endog, Matrix<double> exog)
        {
            enddata = endog;
            exogdata = exog;
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
        double loglike()
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
