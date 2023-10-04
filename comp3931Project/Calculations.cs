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

        static double[] dftFull(int N, double[] S)
        {
            double[] amplitude = new double[N];
            double[] phase = new double[N];
            double[] real = new double[N];
            double[] imaginary = new double[N];

            for (int f = 0; f <= N - 1; f++)
            {
                for (int t = 0; t <= N - 1; t++)
                {
                    real[f] += S[t] * Math.Cos((2 * Math.PI * t * f) / N);
                    imaginary[f] -= S[t] * Math.Sin((2 * Math.PI * t * f) / N);
                }
                amplitude[f] = Math.Sqrt(Math.Pow(real[f], 2) + Math.Pow(imaginary[f], 2));
                phase[f] = Math.Atan(real[f] / imaginary[f]);
            }
            printArray(real, "r");
            printArray(imaginary, "i");
            printArray(amplitude, "amplitude");
            printArray(phase, "phase");
            return amplitude;
        }

        static void printArray(double[] array, String name)
        {
            for (int f = 0; f < array.Length; f++)
            {
                Console.WriteLine(name + "[" + f + "] = " + array[f]);
            }
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
