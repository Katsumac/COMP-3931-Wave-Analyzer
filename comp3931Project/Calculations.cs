using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp3931Project
{
    internal class Calculations
    {

        public double[] DFT(double[] s, int n)
        {
            double[] A =  new double[n];
            for (int f = 0; f < n; f++)
            {
                double ATemp = 0;
                for (int t = 0; t < n; t++)
                {
                    ATemp += s[t] * Math.Cos(2 * Math.PI * t * f / n);
                }
                A[f] = ATemp;
            }
            return A;
        }
    }
}
