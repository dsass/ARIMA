using System;
using ABMath.IridiumExtensions;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;

namespace ABMath.Miscellaneous
{
    public class Regression
    {
        private Vector dependent;
        private Matrix augmentedExplanatory;
        private StandardDistribution stdNormal;

        public double Sigma
        {
            get;
            protected set;
        }

        public Vector BetaHat
        {
            get;
            protected set;
        }

        public Matrix BetaHatCovariance
        {
            get;
            protected set;
        }

        public Vector PValues
        {
            get;
            protected set;
        }

        private void Recompute(bool getBetaHatOnly)
        {
            int p = augmentedExplanatory.ColumnCount;
            int n = augmentedExplanatory.RowCount;

            Matrix xt = augmentedExplanatory.Clone();
            xt.Transpose();
            Vector xty = ((xt * dependent.ToColumnMatrix())).ToVector();
            Matrix xtx = xt * augmentedExplanatory;

            Matrix mxty = new Matrix(xty.Length, 1);
            for (int i = 0; i < xty.Length; ++i)
                mxty[i, 0] = xty[i];

            BetaHat = new Vector(p);

            if (mxty.Norm2()==0)
                return;

            Matrix bm = xtx.SolveRobust(mxty);

            for (int i = 0; i < p; ++i)
                BetaHat[i] = bm[i, 0];

            if (getBetaHatOnly)
                return;

            var fitted = (augmentedExplanatory * BetaHat.ToColumnMatrix()).ToVector();
            var resids = dependent - fitted;

            // now compute approximate p-values
            Sigma = Math.Sqrt(resids.Variance()) * n / (n - p);
            BetaHatCovariance = Sigma * Sigma * xtx.Inverse();
            PValues = new Vector(augmentedExplanatory.ColumnCount);
            for (int i = 0; i < augmentedExplanatory.ColumnCount; ++i)
            {
                double x = Math.Abs(BetaHat[i]) / Math.Sqrt(BetaHatCovariance[i, i]);
                PValues[i] = 2 * (1 - stdNormal.CumulativeDistribution(x));
            }
        }

        public Regression(Vector dependent, Matrix explanatory, bool addConstant, bool getBetaHatOnly)
        {
            stdNormal = new StandardDistribution();

            this.dependent = dependent;
            if (addConstant)
            {
                augmentedExplanatory = new Matrix(explanatory.RowCount, explanatory.ColumnCount + 1);
                for (int i = 0; i < explanatory.RowCount; ++i)
                {
                    augmentedExplanatory[i, 0] = 1.0;
                    for (int j = 0; j < explanatory.ColumnCount; ++j)
                        augmentedExplanatory[i, j + 1] = explanatory[i, j];
                }
            }
            else
                augmentedExplanatory = explanatory;

            Recompute(getBetaHatOnly);
        }

        public Regression(Vector dependent, Matrix explanatory, Vector weights, bool addConstant, bool getBetaHatOnly)
        {
            // to perform weighted regression, we create modified versions of 
            stdNormal = new StandardDistribution();

            this.dependent = new Vector(dependent.Length);
            for (int i = 0; i < dependent.Length; ++i)
                this.dependent[i] = dependent[i]*Math.Sqrt(weights[i]);

            augmentedExplanatory = new Matrix(explanatory.RowCount, explanatory.ColumnCount + (addConstant ? 1 : 0));
            if (addConstant)
                for (int i = 0; i < explanatory.RowCount; ++i)
                {
                    augmentedExplanatory[i, 0] = 1.0 * Math.Sqrt(weights[i]);
                    for (int j = 0; j < explanatory.ColumnCount; ++j)
                        augmentedExplanatory[i, j + 1] = explanatory[i, j] * Math.Sqrt(weights[i]);
                }
            else for (int i = 0; i < explanatory.RowCount; ++i)
            {
                for (int j = 0; j < explanatory.ColumnCount; ++j)
                    augmentedExplanatory[i, j] = explanatory[i, j] * Math.Sqrt(weights[i]);
            }

            Recompute(getBetaHatOnly);
        }
    }
}