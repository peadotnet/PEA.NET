using System;
using System.Collections.Generic;
using System.Text;

namespace PEA_VehicleScheduling_Example
{
    public class ArrayLengthComparer : IComparer<int[]>
    {
        public int Compare(int[] x, int[] y)
        {
            return 0 - x.Length.CompareTo(y.Length);
        }
    }
}
