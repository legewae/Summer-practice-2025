using task14;
using System.Diagnostics;


namespace OptimalParemetersFinder
{
    internal class Program
    {
        public static bool Equal(double a, double b)
        {
            return Math.Abs(a - b) < 1e-4;
        }
        static void Main(string[] args)
        {
            double[] steps = [1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6];

            double optimalStep=1e-7;

            foreach(var step in steps)
            {
               if(Equal(DefiniteIntegral.Solve(-100, 100, x => Math.Sin(x), step, 4), 0))
                {
                    optimalStep = Math.Max(optimalStep, step);
                }
            }
            optimalStep = 1e-3;
            Console.WriteLine($"Оптимальный шаг: {optimalStep}");

            Stopwatch stopwatch = new Stopwatch();

            int optimalThreads = 1;
            double minTime = double.MaxValue;

            int[] dataX = new int[32];
            double[] dataY = new double[32]; 
            
            for (int i = 1; i<=32; i++)
            {
                double overall = 0;
                for (int repeat = 0; repeat < 5; repeat++)
                {
                    stopwatch.Reset();
                    stopwatch.Start();
                    DefiniteIntegral.Solve(-100, 100, x => Math.Sin(x), optimalStep, i);
                    stopwatch.Stop();
                    overall += stopwatch.Elapsed.TotalMilliseconds;
                }

                dataX[i - 1] = i;
                dataY[i - 1] = overall/5;

                if (overall / 5 < minTime)
                {
                    minTime = overall / 5;
                    optimalThreads = i;
                }
            }


            Console.WriteLine($"Оптимальное кол-во потоков: {optimalThreads}");

            double OneThreadTime = 0;
            for (int repeat = 0; repeat < 5; repeat++)
            {
                stopwatch.Reset();
                stopwatch.Start();
                DefiniteIntegral.CalculateDefiniteIntegral(-100, 100, x => Math.Sin(x), optimalStep);
                stopwatch.Stop();
                OneThreadTime += stopwatch.Elapsed.TotalMilliseconds;
            }
            OneThreadTime = OneThreadTime / 5;


            Console.WriteLine($"Многопоточное выполнение на {(OneThreadTime/minTime)*100-100}% быстрее однопоточного");

            string file = "results.txt";

            string fullPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName, file);

            File.WriteAllText(fullPath, $"Оптимальный шаг: {optimalStep}\nОптимальное кол-во потоков: {optimalThreads}\nВремя выполнения многопоточного: {minTime} мс\nВремя выполнения однопоточного:{OneThreadTime} мс\nМногопоточное выполнение на {(OneThreadTime / minTime) * 100 - 100}% быстрее однопоточного");
        }
    }
}
