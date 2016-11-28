using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using ARIMA.timeseries.math;

namespace ARIMA.timeseries.stats
{
    // Augmented Dickey-Fuller Test
    class ADF
    {
        private double[] autolag(string model, int startlag, int maxlag, string method)
        {
            return null;
        }
        //    def _autolag(mod, endog, exog, startlag, maxlag, method, modargs= (),
        //    fitargs= (), regresults= False):
        //"""
        //Returns the results for the lag length that maximimizes the info criterion.

        //Parameters
        //----------
        //mod : Model class
        //    Model estimator class.
        //modargs : tuple
        //    args to pass to model.See notes.
        //fitargs : tuple
        //    args to pass to fit.  See notes.
        //lagstart : int
        //    The first zero-indexed column to hold a lag.  See Notes.
        //maxlag : int
        //    The highest lag order for lag length selection.
        //method : str { "aic","bic","t-stat"}
        //    aic - Akaike Information Criterion
        //    bic - Bayes Information Criterion
        //    t-stat - Based on last lag

        //Returns
        //-------
        //icbest : float
        //    Best information criteria.
        //bestlag : int
        //    The lag length that maximizes the information criterion.


        //Notes
        //-----
        //Does estimation like mod(endog, exog[:,:i], * modargs).fit(*fitargs)
        // where i goes from lagstart to lagstart+maxlag+1.  Therefore, lags are

        // assumed to be in contiguous columns from low to high lag length with

        // the highest lag in the last column.
        // """
        //#TODO: can tcol be replaced by maxlag + 2?
        //#TODO: This could be changed to laggedRHS and exog keyword arguments if
        //#    this will be more general.

        //    results = { }
        //        method = method.lower()
        //    for lag in range(startlag, startlag+maxlag+1):
        //        mod_instance = mod(endog, exog[:,:lag], * modargs)
        //        results[lag] = mod_instance.fit()

        //    if method == "aic":
        //        icbest, bestlag = min((v.aic, k) for k,v in results.iteritems())
        //    elif method == "bic":
        //        icbest, bestlag = min((v.bic, k) for k,v in results.iteritems())
        //    elif method == "t-stat":
        //        lags = sorted(results.keys())[::-1]
        //# stop = stats.norm.ppf(.95)
        //        stop = 1.6448536269514722
        //        for lag in range(startlag + maxlag, startlag - 1, -1):
        //            icbest = np.abs(results[lag].tvalues[-1])
        //            if np.abs(icbest) >= stop:
        //                bestlag = lag
        //                icbest = icbest
        //                break
        //    else:
        //        raise ValueError("Information Criterion %s not understood.") % method

        //    if not regresults:
        //        return icbest, bestlag
        //    else:
        //        return icbest, bestlag, results


        public double[] adfuller(double[] x, int maxlag = -1, int regression = 0, string auto = "AIC")
        {
            //return null;
            Dictionary<int, string> trenddict = new Dictionary<int, string>
            {
                { -1, "nc" },
                { 0, "c" },
                { 1, "ct" },
                { 2, "ctt" }
            };
            string reg = null;
            int length = 0;
            if (-2 < regression && regression < 3)
            {
                trenddict.TryGetValue(regression, out reg);
                length = x.Length;
                if (maxlag < 0)
                {
                    maxlag = (int)Math.Ceiling(12.0 * Math.Pow(length / 100, 1 / 4.0));
                }
                double[] xdiff = MathFunctions.diff(x);
                double[] xdall = MathFunctions.lagmat(xdiff, maxlag);
                length = xdall.Length;
                if (auto != null)
                {
                    double[] fullRHS = null;
                    if (String.Compare(reg, "nc", StringComparison.CurrentCulture) == 0)
                    {
                        fullRHS = MathFunctions.add_trend(xdall, reg, true);
                    }
                    else
                    {
                        fullRHS = xdall;
                    }
                    int startlag = fullRHS.Length - xdall.Length + 1; // 1 for level
                    // search for lag length with smallest information criteria
                    // Note: use the same number of observations to have comparable IC
                    // aic and bic: smaller is better


                    // call autolag
                    double[] best = autolag("OLS", startlag, maxlag, auto);
                    best[1] -= startlag; // convert to lag not column index

                    // rerun OLS with best autolag
                    xdall = MathFunctions.lagmat(xdiff, (int) best[1]);
                    length = xdall.Length;

                }
        //        if regression != 'nc':
        //            fullRHS = add_trend(xdall, regression, prepend= True)
        //        else:
        //            fullRHS = xdall
        //        startlag = fullRHS.shape[1] - xdall.shape[1] + 1 # 1 for level  # pylint: disable=E1103
        //        #search for lag length with smallest information criteria
        //        #Note: use the same number of observations to have comparable IC
        //        #aic and bic: smaller is better

                    //        if not regresults:
                    //            icbest, bestlag = _autolag(OLS, xdshort, fullRHS, startlag,
                    //                                       maxlag, autolag)
                    //        else:
                    //            icbest, bestlag, alres = _autolag(OLS, xdshort, fullRHS, startlag,
                    //                                        maxlag, autolag, regresults= regresults)
                    //            resstore.autolag_results = alres

                    //        bestlag -= startlag  #convert to lag not column index

                    //        #rerun ols with best autolag
                    //        xdall = lagmat(xdiff[:, None], bestlag, trim = 'both', original = 'in')
                    //        nobs = xdall.shape[0]   # pylint: disable=E1103
                    //        xdall[:, 0] = x[-nobs - 1:-1] # replace 0 xdiff with level of x
                    //        xdshort = xdiff[-nobs:]
                    //        usedlag = bestlag
                    //    else:
                    //        usedlag = maxlag
                    //        icbest = None
                    //    if regression != 'nc':
                    //        resols = OLS(xdshort, add_trend(xdall[:,:usedlag + 1], regression)).fit()
                    //    else:
                    //        resols = OLS(xdshort, xdall[:,:usedlag + 1]).fit()
            }
            else
            {
                throw new ArgumentException();
            }
            return null;
        }


        //        def adfuller(x, maxlag= None, regression= "c", autolag= 'AIC',
        //    store= False, regresults= False):
        //    '''Augmented Dickey-Fuller unit root test

        //    The Augmented Dickey-Fuller test can be used to test for a unit root in a
        //    univariate process in the presence of serial correlation.

        //    Parameters
        //    ----------
        //    x : array_like, 1d
        //        data series
        //    maxlag : int
        //        Maximum lag which is included in test, default 12*(nobs/100)^{1/4}
        //    regression : str {'c','ct','ctt','nc'}
        //        Constant and trend order to include in regression
        //        * 'c' : constant only
        //        * 'ct' : constant and trend
        //        * 'ctt' : constant, and linear and quadratic trend
        //        * 'nc' : no constant, no trend
        //    autolag : {'AIC', 'BIC', 't-stat', None}
        //        * if None, then maxlag lags are used
        //        * if 'AIC' or 'BIC', then the number of lags is chosen to minimize the
        //          corresponding information criterium
        //        * 't-stat' based choice of maxlag.Starts with maxlag and drops a
        //          lag until the t-statistic on the last lag length is significant at
        //          the 95 % level.
        //    store : bool
        //        If True, then a result instance is returned additionally to
        //        the adf statistic
        //    regresults : bool
        //        If True, the full regression results are returned.

        //    Returns
        //    -------
        //    adf : float
        //        Test statistic
        //    pvalue : float
        //        MacKinnon's approximate p-value based on MacKinnon (1994)
        //    usedlag : int
        //        Number of lags used.
        //    nobs : int
        //        Number of observations used for the ADF regression and calculation of
        //        the critical values.
        //    critical values : dict
        //        Critical values for the test statistic at the 1 %, 5 %, and 10 % levels.
        //        Based on MacKinnon (2010)
        //    icbest : float
        //        The maximized information criterion if autolag is not None.
        //    regresults : RegressionResults instance
        //        The
        //    resstore : (optional) instance of ResultStore
        //        an instance of a dummy class with results attached as attributes

        //    Notes
        //    -----
        //    The null hypothesis of the Augmented Dickey-Fuller is that there is a unit
        //    root, with the alternative that there is no unit root.If the pvalue is
        //    above a critical size, then we cannot reject that there is a unit root.

        //    The p-values are obtained through regression surface approximation from
        //    MacKinnon 1994, but using the updated 2010 tables.
        //    If the p-value is close to significant, then the critical values should be
        //    used to judge whether to accept or reject the null.

        //    The autolag option and maxlag for it are described in Greene.

        //    Examples
        //    --------
        //    see example script

        //    References
        //    ----------
        //    Greene
        //    Hamilton


        //    P-Values(regression surface approximation)
        //    MacKinnon, J.G. 1994.  "Approximate asymptotic distribution functions for
        //    unit-root and cointegration tests.  `Journal of Business and Economic
        //    Statistics` 12, 167-76.

        //    Critical values
        //    MacKinnon, J.G. 2010. "Critical Values for Cointegration Tests."  Queen's
        //    University, Dept of Economics, Working Papers.  Available at
        //    http://ideas.repec.org/p/qed/wpaper/1227.html

        //    '''

        //    if regresults:
        //        store = True

        //    trenddict = { None: 'nc', 0:'c', 1:'ct', 2:'ctt'}
        //    if regression is None or isinstance(regression, int):
        //        regression = trenddict[regression]
        //    regression = regression.lower()
        //    if regression not in ['c','nc','ct','ctt']:
        //        raise ValueError("regression option %s not understood") % regression
        //    x = np.asarray(x)
        //    nobs = x.shape[0]

        //    if maxlag is None:
        //        #from Greene referencing Schwert 1989
        //        maxlag = int(np.ceil(12. * np.power(nobs/100., 1/4.)))

        //    xdiff = np.diff(x)
        //    xdall = lagmat(xdiff[:, None], maxlag, trim= 'both', original= 'in')
        //    nobs = xdall.shape[0]  # pylint: disable=E1103

        //    xdall[:, 0] = x[-nobs - 1:-1] # replace 0 xdiff with level of x
        //    xdshort = xdiff[-nobs:]

        //    if store:
        //        resstore = ResultsStore()
        //    if autolag:
        //        if regression != 'nc':
        //            fullRHS = add_trend(xdall, regression, prepend= True)
        //        else:
        //            fullRHS = xdall
        //        startlag = fullRHS.shape[1] - xdall.shape[1] + 1 # 1 for level  # pylint: disable=E1103
        //        #search for lag length with smallest information criteria
        //        #Note: use the same number of observations to have comparable IC
        //        #aic and bic: smaller is better

        //        if not regresults:
        //            icbest, bestlag = _autolag(OLS, xdshort, fullRHS, startlag,
        //                                       maxlag, autolag)
        //        else:
        //            icbest, bestlag, alres = _autolag(OLS, xdshort, fullRHS, startlag,
        //                                        maxlag, autolag, regresults= regresults)
        //            resstore.autolag_results = alres

        //        bestlag -= startlag  #convert to lag not column index

        //        #rerun ols with best autolag
        //        xdall = lagmat(xdiff[:, None], bestlag, trim = 'both', original = 'in')
        //        nobs = xdall.shape[0]   # pylint: disable=E1103
        //        xdall[:, 0] = x[-nobs - 1:-1] # replace 0 xdiff with level of x
        //        xdshort = xdiff[-nobs:]
        //        usedlag = bestlag
        //    else:
        //        usedlag = maxlag
        //        icbest = None
        //    if regression != 'nc':
        //        resols = OLS(xdshort, add_trend(xdall[:,:usedlag + 1], regression)).fit()
        //    else:
        //        resols = OLS(xdshort, xdall[:,:usedlag + 1]).fit()

        //    adfstat = resols.tvalues[0]
        //# adfstat = (resols.params[0]-1.0)/resols.bse[0]
        //# the "asymptotically correct" z statistic is obtained as
        //# nobs/(1-np.sum(resols.params[1:-(trendorder+1)])) (resols.params[0] - 1)
        //# I think this is the statistic that is used for series that are integrated
        //# for orders higher than I(1), ie., not ADF but cointegration tests.

        //# Get approx p-value and critical values
        //    pvalue = mackinnonp(adfstat, regression = regression, N = 1)
        //    critvalues = mackinnoncrit(N= 1, regression= regression, nobs= nobs)
        //    critvalues = {"1%" : critvalues[0], "5%" : critvalues[1],
        //            "10%" : critvalues[2]
        //}
        //    if store:
        //        resstore.resols = resols
        //        resstore.maxlag = maxlag
        //        resstore.usedlag = usedlag
        //        resstore.adfstat = adfstat
        //        resstore.critvalues = critvalues
        //        resstore.nobs = nobs
        //        resstore.H0 = "The coefficient on the lagged level equals 1 - unit root"
        //        resstore.HA = "The coefficient on the lagged level < 1 - stationary"
        //        resstore.icbest = icbest
        //        return adfstat, pvalue, critvalues, resstore
        //    else:
        //        if not autolag:
        //            return adfstat, pvalue, usedlag, nobs, critvalues
        //        else:
        //            return adfstat, pvalue, usedlag, nobs, critvalues, icbest


    }
}
