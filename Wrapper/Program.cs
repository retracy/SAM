using System;
using System.Runtime.InteropServices;

namespace Wrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Debug.WriteLine(fnDriver());
        }

        [DllImport("Driver.dll")]
        extern static private int fnDriver();
    }
}
