using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OceansLab.VTKManagement
{
    public class ParamInfo
    {
        public string OutputPath { get; private set; } = "output.vtk";
        public Commons.Map.ExtentF Extent { get; private set; }
        public ParamInfo(string[] paramArr)
        {
            Initial(paramArr);
        }

        public void Initial(string[] paramArr)
        {
            for (int i = 0; i < paramArr.Length; i++)
            {
                if (paramArr[i].StartsWith("-o"))
                {
                    OutputPath = paramArr[i].Substring(paramArr[i].IndexOf("-o") + 2);
                }
                else if (paramArr[i].StartsWith("-e"))
                {
                    string extentStr = paramArr[i].Substring(paramArr[i].IndexOf("-e") + 2);
                    Extent = new Commons.Map.ExtentF(extentStr);
                }
            }
        }
    }
}
