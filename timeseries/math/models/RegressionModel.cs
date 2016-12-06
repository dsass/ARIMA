using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ARIMA.timeseries.models
{
    public class RegressionModel : Model
    {
        string[] data_attr;
        Matrix<double> normalized_cov_params;
        static double thisrank;
        static double df_model;
        Matrix<double> pinv_wexog;
        Matrix<double> wexog;
        protected double nobs;
        Matrix<double> wendog;
        string thismodel;
        static int k_constant;

        public RegressionModel(Matrix<double> endog, Matrix<double> exog, string m) : base(endog, exog)
        {
            data_attr = new string[] { "pinv_wexog", "wendog", "wexog", "weights" };
            
            wexog = whiten(exogdata);
            wendog = whiten(enddata);
            Console.WriteLine(wexog.ToString());
            Console.WriteLine(wendog.ToString());
            //normalized_cov_params = Matrix<double>.Build.Dense(1, 1, 1);
            // overwrite nobs from class Model:
            nobs = (double)wexog.RowCount;

            //self._df_model = None
            //self._df_resid = None
            thisrank = 0;
            df_model = 0;
            k_constant = 0;
            thismodel = m;
            computedfmodel();
        }

        public int K_constant
        {
            get
            {
                return k_constant;
            }
        }

        private static void computedfmodel()
        {
            // the model degree of freedom, defined as the rank of the regressor matrix - 1
            // if a constant is included
            if (df_model == 0)
            {
                if (thisrank == 0)
                {
                    thisrank = (double)exogdata.Rank();
                }
                df_model = (double)(thisrank - k_constant);
            }
        }

        public double Df_model
        {
            get
            {
                return df_model;
            }
        }

        public virtual Matrix<double> whiten(Matrix<double> X)
        {
            throw new NotImplementedException();
        }

        public RegressionResults fit(string method="pinv", string cov_type="nonrobust")
        {
            //        Full fit of the model.

            //        The results include an estimate of covariance matrix, (whitened)
            //        residuals and an estimate of scale.

            //        Parameters
            //        ----------
            //        method: str
            //            Can be "pinv", "qr".  "pinv" uses the Moore - Penrose pseudoinverse
            //            to solve the least squares problem. "qr" uses the QR
            //            factorization.

            //        Returns
            //        ------ -
            //        A RegressionResults class instance.

            //        See Also
            //        ---------
            //        regression.RegressionResults

            //        Notes
            //        -----
            //        The fit method uses the pseudoinverse of the design/exogenous variables
            //        to solve the least squares minimization.
            Matrix<double> beta = null;
           // if (String.Equals(method, "pinv"))
           // {
                //if (pinv_wexog == null || normalized_cov_params == null || rank == null)
                //{
                    pinv_wexog = math.linalg.ArrayManipulation.pinv_extend(wexog);
                    normalized_cov_params = pinv_wexog.Transpose().PointwiseMultiply(pinv_wexog);
                //}
                //Console.WriteLine(wendog.ToString());
            int rows = pinv_wexog.RowCount;
            Matrix<double> wendogcopy = Matrix<double>.Build.Dense(rows, rows, 1.0);
            //Matrix<double> wendogcopy = Matrix<double>.Build.Dense((int)pinv_wexog.RowCount, (int)pinv_wexog.RowCount, 1.0);
            beta = pinv_wexog.PointwiseMultiply(wendog);
            //Console.WriteLine(wendogcopy);
            //beta = pinv_wexog.PointwiseMultiply(wendogcopy);
            //} else if (String.Equals(method, "qr"))
            // {
            //   beta = pinv_wexog.PointwiseDivide(wendog);
            //            if ((not hasattr(self, 'exog_Q')) or
            //                (not hasattr(self, 'exog_R')) or
            //                (not hasattr(self, 'normalized_cov_params')) or
            //                (getattr(self, 'rank', None) is None)):
            //                Q, R = np.linalg.qr(self.wexog)
            //                self.exog_Q, self.exog_R = Q, R
            //                self.normalized_cov_params = np.linalg.inv(np.dot(R.T, R))

            //                # Cache singular values from R.
            //                self.wexog_singular_values = np.linalg.svd(R, 0, 0)
            //                self.rank = np_matrix_rank(R)
            //            else:
            //                Q, R = self.exog_Q, self.exog_R

            //# used in ANOVA
            //            self.effects = effects = np.dot(Q.T, self.wendog)
            //            beta = np.linalg.solve(R, effects)
            //  }
            if (df_model == 0)
            {
                df_model = thisrank - k_constant;
            }
            return new RegressionResults(thismodel, this, beta, normalized_cov_params);
        }
    }
}
