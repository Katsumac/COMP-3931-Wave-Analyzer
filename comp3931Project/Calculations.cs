using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp3931Project
{
    internal class Calculations
    {

        public static double[] DFT(double[] s, int n)

        {
            double[] A = new double[n];
            for (int f = 0; f < n; f++)
            {
                double ATemp = 0;
                for (int t = 0; t < n; t++)
                {
                    ATemp += s[t] * Math.Cos(2 * Math.PI * t * f / n);
                    // ATemp.imaginary -= s[t] * Math.Sin(2 * Math.PI * t * f / n);
                }
                A[f] = ATemp;
            }
            return A;
        }

       /* public static ComplexNumber.complexnumber[] DFT(double[] s, int n)

        {
            double[] A = new double[n];
            for (int f = 0; f < n; f++)
            {
                double ATemp = 0;
                for (int t = 0; t < n; t++)
                {
                    ATemp += s[t] * Math.Cos(2 * Math.PI * t * f / n);
                   // ATemp.imaginary -= s[t] * Math.Sin(2 * Math.PI * t * f / n);
                }
                A[f] = ATemp;
            }
            return A;
        }*/

    public static double[] createSamples(int N, int f)
    {
        double[] S = new double[N];
        for (int t = 0; t < N; t++)
        {
                S[t] = Math.Cos((2 * Math.PI * t * f) / N);// + Math.Cos((2 * Math.PI * t * (f + 2)) / N);
        }
        return S;
    }

    }
}
