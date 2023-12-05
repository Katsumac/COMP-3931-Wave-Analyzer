using System.Diagnostics;

namespace comp3931Project
{
    /**
     * Class that defines calculations required for the Wave Analyzer
     */
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

        private const int MAX_NUM_THREADS = 4;

        /**
         * Purpose: For comparison purposes. Performs nonthreaded full DFT to the samples passed in.
         * Fills in real, imaginary, A and phase arrays
         * 
         * @param S: The sample array
         * 
         * @return: An array of amplitudes (A)
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
         * Purpose: This is the threadProc for threaded DFT. Contains the logic for DFT.
         * Fills in real, imaginary, A and phase arrays
         * 
         * @param S: The sample array
         * @param N: The sample size
         * @param start: The index at which the thread starts performing DFT in the sample array
         * @param end: The index at which the thread stops performing DFT in the sample array
         * 
         * @return: None
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
         * Purpose: Performs threaded full DFT to the samples passed in
         * 
         * @param S: The sample array
         * 
         * @return: An array of amplitudes
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
         * Purpose: For comparison purposes. Performs nonthreaded inverse DFT on the sample
         * 
         * @param N: The sample size
         * @param lowPassFilter: The low-pass filter
         * 
         * @return: The sample array
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
         * Purpose: This is the threadProc for inverse DFT. Contains the logic for inverseDFT
         * 
         * @param S: The sample array
         * @param lowPassFilter: The low-pass filter
         * @param N: The sample size
         * @param start: The index at which the thread starts performing inverse DFT in the sample array
         * 
         * @return: None
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
         * Purpose: Performs threaded inverse DFT to the samples passed in
         * 
         * @param N: The sample size
         * @param lowPassFilter: The low-pass filter
         * 
         * @return: The sample array
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
         * Purpose: Creates a low pass filter based on the selected cutoff
         * 
         * @param N: The sample size
         * @param fCutoff: The cutoff frequency bin
         * 
         * @return: A low-pass filter
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
         * Purpose: For comparison purposes. Performs non-assembly (C#) convolution on samples
         * 
         * @param s: The sample array
         * 
         * @return: None
         */
        public static void convolveNonAsm(double[] s) {
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
         * Purpose: This is the threadProc for threaded convolution. Contains the logic for convolution
         * 
         * @param filter: The filter
         * @param samples: The samples
         * @param convolutedSamples: The convoluted samples array that stores the values from convolution
         * @param start: The index at which the thread starts performing convolution in the sample array
         * @param end: The index at which the thread stops performing convolution in the sample array
         * 
         * @return: None
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
         * Purpose: Performs threaded convolution to the samples passed in
         * 
         * @param s: The sample array
         * 
         * @return: The sample array after convolution
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
         * Purpose: For testing purposes. Creates samples
         * 
         * @param N: Sample size
         * @param f: Frequency
         * 
         * @return: A sample array
         */
        public static double[] createSamples(int N, int f) {
            double[] s = new double[N];
            for (int t = 0; t < N; t++) {
                s[t] = Math.Cos((2 * Math.PI * t * f) / N) + Math.Cos((2 * Math.PI * t * (f + 2)) / N);
            }
            return s;
        }

        /**
         * Purpose: Displays the benchmark between threaded and nonthreaded DFT via message box
         * 
         * @return: None
         */
        private static void displayDFTBenchmark() {
            if (DFTRuntimeSync != 0 && DFTRuntimeThreaded != 0) {
                string msg = "With 4 threads, threaded DFT is " + DFTRuntimeSync / DFTRuntimeThreaded + " faster than without threads.";
                string title = "DFT - Threaded vs Unthreaded";
                MessageBox.Show(msg, title);
            }
        }

        /**
         * Purpose: Displays the benchmark between threaded and nonthreaded inverse DFT via message box
         * 
         * @return: None
         */
        private static void displayInverseDFTBenchmark() {
            if (inverseDFTRuntimeSync != 0 && inverseDFTRuntimeThreaded != 0) {
                string msg = "With 4 threads, threaded inverse DFT is " + inverseDFTRuntimeSync / inverseDFTRuntimeThreaded + " faster than without threads.";
                string title = "InverseDFT - Threaded vs Unthreaded";
                MessageBox.Show(msg, title);
            }
        }

        /**
         * Displays the benchmark between threaded and nonthreaded convolution via message box
         * 
         * @return: None
         */
        private static void displayConvolutionBenchmark() {
            if (convolutionRuntimeSync != 0 && convolutionRuntimeThreaded != 0) {
                string msg = "With 4 threads, threaded convolution is " + convolutionRuntimeSync / convolutionRuntimeThreaded + " faster than without threads.";
                string title = "Convolution - Threaded vs Unthreaded";
                MessageBox.Show(msg, title);
            }
        }

        /**
         * Purpose: Instantiate arrays for DFT
         * 
         * @param N: The sample size
         * 
         * @return: None
         */
        private static void instantiateArraysForDFT(int N)
        {
            A = new double[N]; // Amplitudes
            phase = new double[N]; // Phases
            real = new double[N]; // Real part of DFT
            imaginary = new double[N]; // Imaginary part of DFT
        }

        /**
         * Purpose: Runs the threads for DFT
         * 
         * @param S: The sample array
         * @param N: The sample size
         * 
         * @return: None
         */
        private static void runDFTThreads(double[] S, int N) {
            Thread[] threads = new Thread[MAX_NUM_THREADS];
            for (int threadCount = 0; threadCount < MAX_NUM_THREADS; threadCount++) {
                int start = threadCount * S.Length / MAX_NUM_THREADS;
                int end = (threadCount + 1) * S.Length / MAX_NUM_THREADS;
                threads[threadCount] = new Thread(() => DFT_ThreadProc(S, N, start, end));
                threads[threadCount].Start();
            }
            for (int threadCount = 0; threadCount < MAX_NUM_THREADS; threadCount++) {
                threads[threadCount].Join();
            }
        }

        /**
         * Purpose: Runs the threads for inverse DFT
         * 
         * @param N: The sample size
         * @param lowPassFilter: The low-pass filter
         * 
         * @return: None
         */
        private static void runInverseDFTThreads(int N, double[] lowPassFilter) {
            Thread[] threads = new Thread[MAX_NUM_THREADS];
            for (int threadCount = 0; threadCount < MAX_NUM_THREADS; threadCount++) {
                int start = threadCount * s.Length / MAX_NUM_THREADS;
                int end = (threadCount + 1) * s.Length / MAX_NUM_THREADS;
                threads[threadCount] = new Thread(() => inverseDFT_ThreadProc(s, lowPassFilter, N, start, end));
                threads[threadCount].Start();
            }
            for (int threadCount = 0; threadCount < MAX_NUM_THREADS; threadCount++) {
                threads[threadCount].Join();
            }
        }

        /**
         * Purpose: Runs the threads for convolution
         * 
         * @param filter: The filter
         * @param samples: The samples
         * @param convolutedSamples: The convolution array to store the values from convolution
         * 
         * @return: None
         */
        private static void runConvolutionThread(double[] filter, double[] samples, double[] convolutedSamples) {
            Thread[] threads = new Thread[MAX_NUM_THREADS];
            for (int threadCount = 0; threadCount < MAX_NUM_THREADS; threadCount++){
                int start = threadCount * s.Length / MAX_NUM_THREADS;
                int end = (threadCount + 1) * s.Length / MAX_NUM_THREADS;
                threads[threadCount] = new Thread(() => convolve_ThreadProc(filter, samples, convolutedSamples, start, end));
                threads[threadCount].Start();
            }
            for (int threadCount = 0; threadCount < MAX_NUM_THREADS; threadCount++){
                threads[threadCount].Join();
            }
        }
    }
}
