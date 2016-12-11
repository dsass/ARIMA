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
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.RandomSources;

namespace ABMath.IridiumExtensions
{
    public class MVNormalDistribution
    {
        private readonly StandardDistribution stdnormal;

        public RandomSource RandomSource
        {
            get { return stdnormal.RandomSource; }
            set { stdnormal.RandomSource = value; }
        }

        private Matrix sigma;
        private Matrix sqrtSigma;
        private Matrix sqrtSigmaInverse;
        private Matrix invSigma;
        private double detSigma;
        private Vector mu;
        private int dimension;

        public Matrix Sigma
        {
            get { return sigma; }
            set
            {
                sigma = value;
                ComputeCholeskyDecomp();
            }
        }

        public Vector Mu
        {
            get { return mu; }
            set
            {
                mu = value;
                dimension = value.Length;
            }
        }

        public MVNormalDistribution() // constructor
        {
            stdnormal = new StandardDistribution();
        }


        private void ComputeCholeskyDecomp()
        {
            var cd = new CholeskyDecomposition(Sigma);
            sqrtSigma = cd.TriangularFactor;
            detSigma = Sigma.Determinant();
            if (detSigma != 0)
            {
                invSigma = Sigma.Inverse();
                sqrtSigmaInverse = sqrtSigma.Inverse();
            }
            else
            {
                invSigma = null;
                sqrtSigmaInverse = null;
            }
        }

        public double LogProbabilityDensity(Vector x)
        {
            Matrix tm1 = (x - mu).ToColumnMatrix();
            tm1.Transpose();
            Matrix tm2 = (tm1*invSigma*(x - mu).ToColumnMatrix());
            double retval = -0.5*tm2[0, 0] - 0.5*Math.Log(detSigma) - dimension/2.0*Math.Log(2*Math.PI);
            return retval;
        }
        
        public Vector NextVector()
        {
            var retval = new Vector(dimension);
            for (int i = 0; i < dimension; ++i)
                retval[i] = stdnormal.NextDouble();
            retval = sqrtSigma.MultiplyBy(retval);
            for (int i = 0; i < dimension; ++i)
                retval[i] += mu[i];
            return retval;
        }

        public Vector Standardize(Vector v)
        {
            if (sqrtSigmaInverse == null)
                throw new ApplicationException("Cannot standardize a MV normal vector when its covariance matrix is singular.");
            return sqrtSigmaInverse.MultiplyBy(v);
        }
    }
 }

