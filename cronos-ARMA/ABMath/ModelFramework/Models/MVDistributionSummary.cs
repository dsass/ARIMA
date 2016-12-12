using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace ABMath.ModelFramework.Models
{
    public class MVDistributionSummary
    {
        private Matrix<double> sigma;
        public Vector<double> Mean { get; set; }

        public Matrix<double> Variance
        {
            get { return sigma; }
            set { sigma = value; }
        }

        public Vector<double> Kurtosis { get; set; }

        public DistributionSummary GetMarginal(int component)
        {
            throw new NotImplementedException();
        }
    }
}
