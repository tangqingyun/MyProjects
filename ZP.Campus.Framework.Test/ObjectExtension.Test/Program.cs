using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectExtension.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string sss = "中lll";
            string str=sss.CutOffStr(2);
            Console.WriteLine(str);
            Console.Read();
        }
    }
}
