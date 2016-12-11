using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace ABMath.ModelFramework.Models
{
    public class MVDistributionSummary
    {
        private Matrix sigma;
        public Vector Mean { get; set; }

        public Matrix Variance
        {
            get { return sigma; }
            set { sigma = value; }
        }

        public Vector Kurtosis { get; set; }

        public DistributionSummary GetMarginal(int component)
        {
            throw new NotImplementedException();
        }
    }
}
