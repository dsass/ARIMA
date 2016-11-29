using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ARIMA.timeseries.models
{
    class RegressionModel : Model
    {
        string[] data_attr;
        Matrix<double> normalized_cov_params;
        double[] rank;
        Matrix<double> pinv_wexog;
        Matrix<double> wexog;
        protected double nobs;
        Matrix<double> wendog;
        string model;

        public RegressionModel(Matrix<double> endog, Matrix<double> exog, string m) : base(endog, exog)
        {
            data_attr = new string[] { "pinv_wexog", "wendog", "wexog", "weights" };
            
            wexog = whiten(exogdata);
            wendog = whiten(enddata);
            // overwrite nobs from class Model:
            nobs = (double)wexog.RowCount;

            //self._df_model = None
            //self._df_resid = None
            rank = null;
            model = m;
        }

        public virtual Matrix<double> whiten(Matrix<double> X)
        {
            throw new NotImplementedException();
        }

        public RegressionResults fit(double[] cov_kwds, double[] use_t, string method="pinv", string cov_type="nonrobust")
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
            if (String.Equals(method, "pinv"))
            {
                if (pinv_wexog == null || normalized_cov_params == null || rank == null)
                {
                    pinv_wexog = wexog.Inverse();
                    pinv_wexog.TransposeAndMultiply(pinv_wexog, normalized_cov_params);
                }
                beta = pinv_wexog.PointwiseMultiply(wendog);
            } else if (String.Equals(method, "qr"))
            {
                beta = pinv_wexog.PointwiseDivide(wendog);
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
            }
            return new RegressionResults(model, beta, normalized_cov_params);
        }
    }
}
