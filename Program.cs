using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OceansLab.VTKManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            ParamInfo paramInfo = new ParamInfo(args);
            HycomExtent hycom = new HycomExtent();
            hycom.GetData(paramInfo.Extent, paramInfo.OutputPath);
            //Test test = new Test();
            //test.GetData();
        }
    }
}
