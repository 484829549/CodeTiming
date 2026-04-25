using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CodeTiming
{
    public static class CodeTimer
    {
        /// <summary>
        /// 同步代码耗时统计
        /// </summary>
        public static void Run(string name, Action action)
        {
            Stopwatch sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            Console.WriteLine($"[{name}] 耗时：{sw.Elapsed.TotalMilliseconds:F2} ms");
        }

        /// <summary>
        /// 异步代码耗时统计
        /// </summary>
        public static async Task RunAsync(string name, Func<Task> func)
        {
            Stopwatch sw = Stopwatch.StartNew();
            await func();
            sw.Stop();
            Console.WriteLine($"[{name}] 耗时：{sw.Elapsed.TotalMilliseconds:F2} ms");
        }
    }
}