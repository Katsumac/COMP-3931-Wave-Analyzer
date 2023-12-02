using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

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
        private static Mutex mutex = new Mutex();

        private static double DFTRuntimeThreaded;
        private static double inverseDFTRuntimeThreaded;
        private static double convolutionRuntimeThreaded;
        private static double DFTRuntimeSync;
        private static double inverseDFTRuntimeSync;
        private static double convolutionRuntimeSync;

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

        public static double[] DFTSync(double[] S)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int N = S.Length;
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


            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;

            DFTRuntimeSync = ts.TotalMilliseconds;

            if (DFTRuntimeSync != 0 && DFTRuntimeThreaded != 0)
            {
                string msg = "With 4 threads, threaded DFT is " + DFTRuntimeSync / DFTRuntimeThreaded + " faster than without threads.";
                string title = "DFT - Threaded vs Unthreaded";
                MessageBox.Show(msg, title);
            }

            Debug.WriteLine("Time: " + ts.TotalMilliseconds + "ms");

            return A;
        }

        public static void DFT_ThreadProc(double[] S, int N, int start, int end)
        {
            for (int f = start; f <= end - 1; f++)
            {
                for (int t = 0; t <= N - 1; t++)
                {
                    real[f] += S[t] * Math.Cos((2 * Math.PI * t * f) / N);
                    imaginary[f] -= S[t] * Math.Sin((2 * Math.PI * t * f) / N);
                }
                A[f] = Math.Sqrt(Math.Pow(real[f], 2) + Math.Pow(imaginary[f], 2)) * 2 / N;
                phase[f] = Math.Atan(real[f] / imaginary[f]);
            }
        }

        public static double[] DFT(double[] S)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            int N = S.Length;
            A = new double[N];
            phase = new double[N];
            real = new double[N];
            imaginary = new double[N];

            Thread t1 = new Thread(() => DFT_ThreadProc(S, N, 0, S.Length / 4));
            Thread t2 = new Thread(() => DFT_ThreadProc(S, N, S.Length / 4, S.Length / 2));
            Thread t3 = new Thread(() => DFT_ThreadProc(S, N, S.Length / 2, 3 * S.Length / 4));
            Thread t4 = new Thread(() => DFT_ThreadProc(S, N, 3 * S.Length / 4, S.Length));
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();

            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;

            DFTRuntimeThreaded = ts.TotalMilliseconds;

            if (DFTRuntimeSync != 0 && DFTRuntimeThreaded != 0)
            {
                string msg = "With 4 threads, threaded DFT is " + DFTRuntimeSync / DFTRuntimeThreaded + " faster than without threads.";
                string title = "DFT - Threaded vs Unthreaded";
                MessageBox.Show(msg, title);
            }

            Debug.WriteLine("Time: " + ts.TotalMilliseconds + "ms");

            return A;
        }


        public static double[] inverseDFTSync(int N, double[] lowPassFilter)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            s = new double[N];

            for (int t = 0; t <= N - 1; t++)
            {
                for (int f = 0; f <= N - 1; f++)
                {
                    s[t] += lowPassFilter[f] * (Math.Cos(2 * Math.PI * t * f / N) + Math.Sin(2 * Math.PI * t * f / N));
                }
                s[t] /= N;
            }


            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;

            inverseDFTRuntimeSync = ts.TotalMilliseconds;

            if (inverseDFTRuntimeSync != 0 && inverseDFTRuntimeThreaded != 0)
            {
                string msg = "With 4 threads, threaded inverse DFT is " + inverseDFTRuntimeSync / inverseDFTRuntimeThreaded + " faster than without threads.";
                string title = "InverseDFT - Threaded vs Unthreaded";
                MessageBox.Show(msg, title);
            }

            return s;
        }

        public static void inverseDFT_ThreadProc(double[] S, double[] lowPassFilter, int N, int start, int end)
        {

            for (int t = start; t <= end - 1; t++)
            {
                for (int f = 0; f <= N - 1; f++)
                {
                    S[t] += lowPassFilter[f] * (Math.Cos(2 * Math.PI * t * f / N) + Math.Sin(2 * Math.PI * t * f / N));
                }
                S[t] /= N;
            }
        }

        public static double[] inverseDFT(int N, double[] lowPassFilter)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            s = new double[N];

            Thread t1 = new Thread(() => inverseDFT_ThreadProc(s, lowPassFilter, N, 0, s.Length / 4));
            Thread t2 = new Thread(() => inverseDFT_ThreadProc(s, lowPassFilter, N, s.Length / 4, s.Length / 2));
            Thread t3 = new Thread(() => inverseDFT_ThreadProc(s, lowPassFilter, N, s.Length / 2, 3 * s.Length / 4));
            Thread t4 = new Thread(() => inverseDFT_ThreadProc(s, lowPassFilter, N, 3 * s.Length / 4, s.Length));
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();


            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;

            inverseDFTRuntimeThreaded = ts.TotalMilliseconds;

            if (inverseDFTRuntimeSync != 0 && inverseDFTRuntimeThreaded != 0)
            {
                string msg = "With 4 threads, threaded inverse DFT is " + inverseDFTRuntimeSync / inverseDFTRuntimeThreaded + " faster than without threads.";
                string title = "InverseDFT - Threaded vs Unthreaded";
                MessageBox.Show(msg, title);
            }

            Debug.WriteLine("Time: " + ts.TotalMilliseconds + "ms");

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

        public static void convolveSync(double[] s)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


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


            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;

            convolutionRuntimeSync = ts.TotalMilliseconds;

            if (convolutionRuntimeSync != 0 && convolutionRuntimeThreaded != 0)
            {
                string msg = "With 4 threads, threaded convolution is " + convolutionRuntimeSync / convolutionRuntimeThreaded + " faster than without threads.";
                string title = "Convolution - Threaded vs Unthreaded";
                MessageBox.Show(msg, title);
            }

            Debug.WriteLine("Time: " + ts.TotalMilliseconds + "ms");
        }

        public static void convolve_ThreadProc(double[] filter, double[] samples, double[] convolutedSamples, int start, int end)
        {
            /*            mutex.WaitOne();*/
            for (int sT = start; sT < end; sT++)
            {
                for (int fT = 0; fT < filter.Length; fT++)
                {
                    convolutedSamples[sT] += filter[fT] * samples[sT + fT];
                }

            }
            /*            mutex.ReleaseMutex();
            */
        }

        public static void convolve(double[] s)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            double[] filter = inverseDFT(lowPassFilter.Length, lowPassFilter);

            double[] samples = new double[s.Length + filter.Length - 1];

            double[] convolutedSamples = new double[s.Length];

            for (int t = 0; t < s.Length; t++)
            {
                samples[t] = s[t];
            }

            Thread t1 = new Thread(() => convolve_ThreadProc(filter, samples, convolutedSamples, 0, s.Length / 4));
            Thread t2 = new Thread(() => convolve_ThreadProc(filter, samples, convolutedSamples, s.Length / 4, s.Length / 2));
            Thread t3 = new Thread(() => convolve_ThreadProc(filter, samples, convolutedSamples, s.Length / 2, 3 * s.Length / 4));
            Thread t4 = new Thread(() => convolve_ThreadProc(filter, samples, convolutedSamples, 3 * s.Length / 4, s.Length));
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();

            dynamicWaveGraph.setSample(convolutedSamples);


            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;

            convolutionRuntimeThreaded = ts.TotalMilliseconds;

            if (convolutionRuntimeSync != 0 && convolutionRuntimeThreaded != 0)
            {
                string msg = "With 4 threads, threaded convolution is " + convolutionRuntimeSync / convolutionRuntimeThreaded + " faster than without threads.";
                string title = "Convolution - Threaded vs Unthreaded";
                MessageBox.Show(msg, title);
            }

            Debug.WriteLine("Time: " + ts.TotalMilliseconds + "ms");

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