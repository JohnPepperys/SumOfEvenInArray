using System.Diagnostics;

namespace SumOfEvenInArray
{
    internal class Program
    {
        const int TIMES_REPEAT_TEST = 20;
        const int ARRAY_ELEMENTS = 40 * 1000 * 1000;

        static long[] array = new long[ARRAY_ELEMENTS];
        static long oneTestTimeMilliseconds = 0;
        static long allTestsTimeMilliseconds = 0;

        // for best result better use Release mod. Not Debug.
        static void Main(string[] args)
        {            
            Console.WriteLine("Start app");
            // make some repeat of one tests
            for (int i = 0; i < TIMES_REPEAT_TEST; i++)
            {
                OneTestExecute();
                // skip first and second test in average time calculate, because C# Warm Up
                if (i != 0 && i != 1)
                    allTestsTimeMilliseconds += oneTestTimeMilliseconds;
            }

            Console.WriteLine();
            Console.WriteLine($"Average time from {TIMES_REPEAT_TEST-2} tests: {allTestsTimeMilliseconds / (TIMES_REPEAT_TEST-2)} ms. All time: {allTestsTimeMilliseconds} ms.");
            Console.WriteLine();
            Console.WriteLine("End app");
        }


        static void OneTestExecute()
        {
            // fill random array
            var rand = new Random();
            for (int j = 0; j < ARRAY_ELEMENTS; j++)
                array[j] = rand.Next(int.MaxValue);

            // start timer
            var stopWatch = new Stopwatch();
            
            // make work
            stopWatch.Start();
            
            // here we can choose one of 7 realisation of function additional even nums 
            // var res = ProcessingWork(array);
            //var res = BitOperation(array);
            // var res = ZerOMultiple(array);
            // var res = FourParaller(array);
            // var res = FourParallerWithoutMultiple(array); 
            // var res = WithPointer(array);
            var res = array.Where(x => (x & 1) == 0).Sum();
            
            stopWatch.Stop();
            oneTestTimeMilliseconds = stopWatch.ElapsedMilliseconds;

            // show results
            Console.WriteLine($"One test!! Result: {res}, Time: {stopWatch.ElapsedMilliseconds}");
        }

        static long ProcessingWork(IEnumerable<int> inputArray)
        {
            long res = 0;
            foreach (int value in inputArray)
                if (value % 2 == 0)
                    res += value;
            return res;
        }   // time: 401.  401.  403.   ms


        static long BitOperation(IEnumerable<int> inputArray)
        {
            long res = 0;
            foreach (int value in inputArray)
                if ((value & 1) == 0)
                    res += value;
            return res;
        }   // time   388.  392.   388. ms


        static long ZerOMultiple(int[] inputArray)
        {
            long res = 0;
            for (int i = 0; i < inputArray.Length; i++)
            {
                var even = (inputArray[i] & 1) ^ 1;
                res += inputArray[i] * even;
            }
            return res;
        }   // time:   96   96  96 ms // ---------------- and was 149.  150.  148 ms


        static long FourParaller(int[] inputArray)
        {
            long resA = 0;
            long resB = 0;
            long resC = 0;
            long resD = 0;

            for (int i = 0; i < inputArray.Length; i += 4)
            {
                var elemA = inputArray[i];
                var elemB = inputArray[i+1];
                var elemC = inputArray[i+2];
                var elemD = inputArray[i+3];

                var evenA = (elemA & 1) ^ 1;
                var evenB = (elemB & 1) ^ 1;
                var evenC = (elemC & 1) ^ 1;
                var evenD = (elemD & 1) ^ 1;

                resA += (elemA * evenA);
                resB += (elemB * evenB);
                resC += (elemC * evenC);
                resD += (elemD * evenD);
            }
            return resA + resB + resC + resD;
        }   // time:    62   62   62


        static long FourParallerWithoutMultiple(int[] inputArray)
        {
            long resA = 0;
            long resB = 0;
            long resC = 0;
            long resD = 0;

            for (int i = 0; i < inputArray.Length; i += 4)
            {
                var elemA = inputArray[i];
                var elemB = inputArray[i + 1];
                var elemC = inputArray[i + 2];
                var elemD = inputArray[i + 3];

                var evenA = (elemA & 1) ^ 1;
                var evenB = (elemB & 1) ^ 1;
                var evenC = (elemC & 1) ^ 1;
                var evenD = (elemD & 1) ^ 1;

                resA += (elemA << (elemA & 1)) - elemA;
                resB += (elemB << (elemB & 1)) - elemB;
                resC += (elemC << (elemC & 1)) - elemC;
                resD += (elemD << (elemD & 1)) - elemD;
            }
            return resA + resB + resC + resD;
        }   // time:    68   68   68


        static long WithPointer(int[] inputArray)
        {
            unsafe
            {
                long resA = 0;
                long resB = 0;
                long resC = 0;
                long resD = 0;

                fixed (int* data = &inputArray[0])
                {
                    var p = (int*)data;
                    for (var i = 0; i < inputArray.Length; i += 4)
                    {
                        resA += (p[0] & 1) * p[0];
                        resB += (p[1] & 1) * p[1];
                        resC += (p[2] & 1) * p[2];
                        resD += (p[3] & 1) * p[3];

                        p += 4;
                    }
                }

                return resA + resB + resC + resD;
            }
        } ///  16.  16.     16. ms.

        static long WithTwoPointer(int[] inputArray)
        {
            unsafe
            {
                long resA = 0;
                long resB = 0;
                long resC = 0;
                long resD = 0;

                fixed (int* data = &inputArray[0])
                {
                    var p = (int*)data;
                    var n = (int*)data;

                    for (var i = 0; i < inputArray.Length; i += 4)
                    {
                        resA += (n[0] & 1) * p[0];
                        resB += (n[1] & 1) * p[1];
                        resC += (n[2] & 1) * p[2];
                        resD += (n[3] & 1) * p[3];

                        p += 4;
                        n += 4;
                    }
                }

                return resA + resB + resC + resD;
            }
        } /// 15    16  16 ms.
    }
}