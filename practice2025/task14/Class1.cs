using System.Threading;

namespace task14
{
    public class DefiniteIntegral
    {
        public static double CalculateDefiniteIntegral(double a, double b, Func<double, double> function, double step)
        {
            double result = 0.0;
            double current = a;

            while (current < b)
            {
                double next = Math.Min(current + step, b);
                result += (function(current) + function(next)) * 0.5 * (next - current);
                current = next;
            }

            return result;
        }

        public static double Add(ref double location1, double value)
        {
            double newCurrentValue = location1;
            while (true)
            {
                double currentValue = newCurrentValue;
                double newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref location1, newValue, currentValue);
                if (newCurrentValue.Equals(currentValue))
                    return newValue;
            }
        }
        public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
        {
            double result = 0.0;

            Thread[] threads = new Thread[threadsnumber];
            for (int i = 0; i < threadsnumber; i++)
            {
                double local_i = i;
                threads[i] = new Thread(() =>
                {
                    double da = a + local_i * (b - a) / threadsnumber;
                    double db = a + (local_i + 1) * (b - a) / threadsnumber;

                    double localResult = CalculateDefiniteIntegral(da, db, function, step);
                    Add(ref result, localResult);
                });
                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            // отсюда надо начинать реализацию задачи
            return result;
        }
    }
}
