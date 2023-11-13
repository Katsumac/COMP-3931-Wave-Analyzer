using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp3931Project
{
    internal class Calculations
    {

        private static double[] A;
        private static double[] phase;
        private static double[] real;
        private static double[] imaginary;
        private static double[] s;
        private static double[] lowPassFilter;

        public static double[] halfDFT(double[] s, int n)

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

        public static double[] DFT(double[] S, int N)
        {
            A = new double[N];
            phase = new double[N];
            real = new double[N];
            imaginary = new double[N];

            for (int f = 0; f <= N - 1; f++)
            {
                for (int t = 0; t <= N - 1; t++)
                {
                    real[f] += S[t] * Math.Cos((2 * Math.PI * t * f) / N);
                    imaginary[f] -= S[t] * Math.Sin((2 * Math.PI * t * f) / N);
                }
                A[f] = Math.Sqrt(Math.Pow(real[f], 2) + Math.Pow(imaginary[f], 2)) * 2 / N;
                phase[f] = Math.Atan(real[f] / imaginary[f]);
            }
            return A;
        }

        static double[] inverseDFT(int N, double[] s)
        {
            s = new double[N];

            for (int t = 0; t <= N - 1; t++)
            {
                for (int f = 0; f <= N - 1; f++)
                {
                    s[t] += A[f] * (Math.Cos(2 * Math.PI * t * f / N) + Math.Sin(2 * Math.PI * t * f / N));
                }
                s[t] /= N;
            }
            return s;
        }

        public static double[] createLowPassFilter(int N, int fCutoff)
        {
            lowPassFilter = new double[N];

            int cutoffBin = fCutoff;

            for (int t = 0; t <= N - 1; t++)
            {
                if (t <= cutoffBin || t >= N - cutoffBin)
                {
                    lowPassFilter[t] = 1;
                }
            }
            return lowPassFilter;
        }

        public static void convolute(double[] s)
        {
            
            double[] filter = inverseDFT(lowPassFilter.Length, lowPassFilter);

            double[] samples = new double[s.Length + filter.Length - 1];

            for (int t = 0; t < s.Length; t++)
            {
                samples[t] = s[t];
            }

            double[] convolutedSamples = new double[s.Length];

            for (int sT = 0; sT < s.Length; sT++)
            {
                for (int fT = 0; fT < filter.Length; fT++)
                {
                    convolutedSamples[sT] += filter[fT] * samples[sT + fT];
                }
            }
            dynamicWaveGraph.setSample(convolutedSamples);
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
        double[] s = new double[N];
        for (int t = 0; t < N; t++)
        {
                s[t] = Math.Cos((2 * Math.PI * t * f) / N) + Math.Cos((2 * Math.PI * t * (f + 2)) / N);
        }
        return s;
    }

    }
}
