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

        private static double[] A; // Amplitudes
        private static double[] phase; // Phases
        private static double[] real; // Real part of DFT
        private static double[] imaginary; // Imaginary part of DFT
        private static double[] s; // Samples
        private static double[] lowPassFilter; // Low pass filter
        private static Mutex mutex = new Mutex(); // Mutex lock

        private static double DFTRuntimeThreaded; // Runtime for threaded DFT
        private static double inverseDFTRuntimeThreaded; // Runtime for threaded inverse DFT
        private static double convolutionRuntimeThreaded; // Runtime for threaded convolution
        private static double DFTRuntimeSync; // Runtime for nonthreaded DFT
        private static double inverseDFTRuntimeSync; // Runtime for nonthreaded inverseDFT
        private static double convolutionRuntimeSync; // Runtime for nonthreaded convolution

        private const int NUM_THREADS = 4;

        /**
         * For comparison purposes. Performs nonthreaded full DFT to the samples passed in
         */
        public static double[] DFTSync(double[] S) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int N = S.Length;
            instantiateArraysForDFT(N);
            for (int f = 0; f <= N - 1; f++) {
                for (int t = 0; t <= N - 1; t++) {
                    real[f] += S[t] * Math.Cos((2 * Math.PI * t * f) / N);
                    imaginary[f] -= S[t] * Math.Sin((2 * Math.PI * t * f) / N);
                }
                A[f] = Math.Sqrt(Math.Pow(real[f], 2) + Math.Pow(imaginary[f], 2)) * 2 / N;
                phase[f] = Math.Atan(real[f] / imaginary[f]);
            }
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            DFTRuntimeSync = ts.TotalMilliseconds;
            displayDFTBenchmark();
            return A;
        }

        /**
         * This is the threadProc for threaded DFT
         */
        public static void DFT_ThreadProc(double[] S, int N, int start, int end) {
            for (int f = start; f <= end - 1; f++) {
                for (int t = 0; t <= N - 1; t++) {
                    real[f] += S[t] * Math.Cos((2 * Math.PI * t * f) / N);
                    imaginary[f] -= S[t] * Math.Sin((2 * Math.PI * t * f) / N);
                }
                A[f] = Math.Sqrt(Math.Pow(real[f], 2) + Math.Pow(imaginary[f], 2)) * 2 / N;
                phase[f] = Math.Atan(real[f] / imaginary[f]);
            }
        }

        /**
         * Performs threaded DFT on a sample
         */
        public static double[] DFT(double[] S) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int N = S.Length;
            instantiateArraysForDFT(N);
            runDFTThreads(S, N);
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            DFTRuntimeThreaded = ts.TotalMilliseconds;
            displayDFTBenchmark();
            return A;
        }

        /**
         * For comparison purposes. Performs nonthreaded DFT on the sample
         */
        public static double[] inverseDFTSync(int N, double[] lowPassFilter) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            s = new double[N];
            for (int t = 0; t <= N - 1; t++) {
                for (int f = 0; f <= N - 1; f++) {
                    s[t] += lowPassFilter[f] * (Math.Cos(2 * Math.PI * t * f / N) + Math.Sin(2 * Math.PI * t * f / N));
                }
                s[t] /= N;
            }
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            inverseDFTRuntimeSync = ts.TotalMilliseconds;
            displayInverseDFTBenchmark(); // Benchmarking
            return s;
        }

        /**
         * The threadProc for inverseDFT
         */
        public static void inverseDFT_ThreadProc(double[] S, double[] lowPassFilter, int N, int start, int end) {
            for (int t = start; t <= end - 1; t++) {
                for (int f = 0; f <= N - 1; f++) {
                    S[t] += lowPassFilter[f] * (Math.Cos(2 * Math.PI * t * f / N) + Math.Sin(2 * Math.PI * t * f / N));
                }
                S[t] /= N;
            }
        }

        /**
         * Performs threaded DFT on the samples
         */
        public static double[] inverseDFT(int N, double[] lowPassFilter) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            s = new double[N];
            runInverseDFTThreads(N, lowPassFilter);
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            inverseDFTRuntimeThreaded = ts.TotalMilliseconds;
            displayInverseDFTBenchmark(); // Benchmarking
            return s;
        }

        /**
         * Creates a low pass filter based on the selected cutoff
         */
        public static double[] createLowPassFilter(int N, int fCutoff) {
            lowPassFilter = new double[N];
            int cutoffBin = fCutoff;
            for (int t = 0; t <= N - 1; t++) {
                if (t <= cutoffBin || t >= N - cutoffBin) {
                    lowPassFilter[t] = 1;
                }
            }
            return lowPassFilter;
        }

        /**
         * For comparison purposes. Performs nonthreaded convolution on samples
         */
        public static void convolveSync(double[] s) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double[] filter = inverseDFT(lowPassFilter.Length, lowPassFilter);
            double[] samples = new double[s.Length + filter.Length - 1];
            for (int t = 0; t < s.Length; t++) {
                samples[t] = s[t];
            }
            double[] convolutedSamples = new double[s.Length];
            for (int sT = 0; sT < s.Length; sT++) {
                for (int fT = 0; fT < filter.Length; fT++) {
                    convolutedSamples[sT] += filter[fT] * samples[sT + fT];
                }
            }
            dynamicWaveGraph.setSample(convolutedSamples); // Filters the current sample. Should we do it so that it filters the original sample every time?
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            convolutionRuntimeSync = ts.TotalMilliseconds;
            displayConvolutionBenchmark();
        }

        /**
         * This is the threadProc for threaded convolution
         */
        public static void convolve_ThreadProc(double[] filter, double[] samples, double[] convolutedSamples, int start, int end) {
/*            mutex.WaitOne();*/
            for (int sT = start; sT < end; sT++) {
                for (int fT = 0; fT < filter.Length; fT++) {
                    convolutedSamples[sT] += filter[fT] * samples[sT + fT];
                }
            }
/*            mutex.ReleaseMutex();
*/        }

        /**
         * Performs threaded convolution on samples
         */
        public static void convolve(double[] s) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double[] filter = inverseDFT(lowPassFilter.Length, lowPassFilter);
            double[] samples = new double[s.Length + filter.Length - 1];
            double[] convolutedSamples = new double[s.Length];
            for (int t = 0; t < s.Length; t++) {
                samples[t] = s[t];
            }
            runConvolutionThread(filter, samples, convolutedSamples);
            dynamicWaveGraph.setSample(convolutedSamples);
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            convolutionRuntimeThreaded = ts.TotalMilliseconds;
            displayConvolutionBenchmark(); // Benchmarking
        }

        /**
         * For testing purposes. Creates samples
         */
        public static double[] createSamples(int N, int f) {
            double[] s = new double[N];
            for (int t = 0; t < N; t++) {
                s[t] = Math.Cos((2 * Math.PI * t * f) / N) + Math.Cos((2 * Math.PI * t * (f + 2)) / N);
            }
            return s;
        }

        /**
         * Displays the benchmark between threaded and nonthreaded DFT
         */
        private static void displayDFTBenchmark() {
            if (DFTRuntimeSync != 0 && DFTRuntimeThreaded != 0) {
                string msg = "With 4 threads, threaded DFT is " + DFTRuntimeSync / DFTRuntimeThreaded + " faster than without threads.";
                string title = "DFT - Threaded vs Unthreaded";
                MessageBox.Show(msg, title);
            }
        }

        /**
         * Displays the benchmark between threaded and nonthreaded inverse DFT
         */
        private static void displayInverseDFTBenchmark() {
            if (inverseDFTRuntimeSync != 0 && inverseDFTRuntimeThreaded != 0) {
                string msg = "With 4 threads, threaded inverse DFT is " + inverseDFTRuntimeSync / inverseDFTRuntimeThreaded + " faster than without threads.";
                string title = "InverseDFT - Threaded vs Unthreaded";
                MessageBox.Show(msg, title);
            }
        }

        /**
         * Displays the benchmark between threaded and nonthreaded convolution
         */
        private static void displayConvolutionBenchmark() {
            if (convolutionRuntimeSync != 0 && convolutionRuntimeThreaded != 0) {
                string msg = "With 4 threads, threaded convolution is " + convolutionRuntimeSync / convolutionRuntimeThreaded + " faster than without threads.";
                string title = "Convolution - Threaded vs Unthreaded";
                MessageBox.Show(msg, title);
            }
        }

        /**
         * Instantiate arrays for DFT
         */
        private static void instantiateArraysForDFT(int N)
        {
            A = new double[N]; // Amplitudes
            phase = new double[N]; // Phases
            real = new double[N]; // Real part of DFT
            imaginary = new double[N]; // Imaginary part of DFT
        }

        /**
         * Runs the threads for DFT
         */
        private static void runDFTThreads(double[] S, int N) {
            Thread[] threads = new Thread[NUM_THREADS];
            for (int threadCount = 0; threadCount < NUM_THREADS; threadCount++) {
                int start = threadCount * S.Length / NUM_THREADS;
                int end = (threadCount + 1) * S.Length / NUM_THREADS;
                threads[threadCount] = new Thread(() => DFT_ThreadProc(S, N, start, end));
                threads[threadCount].Start();
            }
            for (int threadCount = 0; threadCount < NUM_THREADS; threadCount++) {
                threads[threadCount].Join();
            }
        }
        
        /**
         * Runs the threads for inverse DFT
         */
        private static void runInverseDFTThreads(int N, double[] lowPassFilter) {
            Thread[] threads = new Thread[NUM_THREADS];
            for (int threadCount = 0; threadCount < NUM_THREADS; threadCount++) {
                int start = threadCount * s.Length / NUM_THREADS;
                int end = (threadCount + 1) * s.Length / NUM_THREADS;
                threads[threadCount] = new Thread(() => inverseDFT_ThreadProc(s, lowPassFilter, N, start, end));
                threads[threadCount].Start();
            }
            for (int threadCount = 0; threadCount < NUM_THREADS; threadCount++) {
                threads[threadCount].Join();
            }
        }

        /**
         * Runs the threads for convolution
         */
        private static void runConvolutionThread(double[] filter, double[] samples, double[] convolutedSamples) {
            Thread[] threads = new Thread[NUM_THREADS];
            for (int threadCount = 0; threadCount < NUM_THREADS; threadCount++){
                int start = threadCount * s.Length / NUM_THREADS;
                int end = (threadCount + 1) * s.Length / NUM_THREADS;
                threads[threadCount] = new Thread(() => convolve_ThreadProc(filter, samples, convolutedSamples, start, end));
                threads[threadCount].Start();
            }
            for (int threadCount = 0; threadCount < NUM_THREADS; threadCount++){
                threads[threadCount].Join();
            }
        }
    }
}
