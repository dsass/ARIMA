using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABMath.ModelFramework
{
    public interface IExtraFunctionality
    {
        int NumAuxiliaryFunctions();
        string AuxiliaryFunctionName(int index);
        string AuxiliaryFunctionHelp(int index);
        bool AuxiliaryFunction(int index, out object output);
    }
}
