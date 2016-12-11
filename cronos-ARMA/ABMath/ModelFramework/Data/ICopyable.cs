using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABMath.ModelFramework.Data
{
    public interface ICopyable
    {
        string CreateFullString(int detailLevel);
        void ParseFromFullString(string s);
    }
}
