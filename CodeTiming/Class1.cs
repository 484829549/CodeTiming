using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CodeTimer
{
    public static class Timing
    {
        // 🔥 通用：由外部项目设置，不写死
        public static string ProjectName { get; set; } = "DefaultProject";
        private static readonly string _logPath = @"D:\CodeTimerLog.txt";

        private static Stopwatch? _globalSw;
        private static string? _globalTimerName;

        // ==================== 全局 Start / Stop ====================
        public static void Start(string timerName)
        {
            _globalTimerName = timerName;
            _globalSw = Stopwatch.StartNew();
        }

        public static void Stop()
        {
            if (_globalSw == null || string.IsNullOrEmpty(_globalTimerName))
                return;

            _globalSw.Stop();
            string log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{ProjectName}] [{_globalTimerName}] 耗时：{_globalSw.Elapsed.TotalMilliseconds:F2} ms";
            WriteLog(log);

            _globalSw = null;
            _globalTimerName = null;
        }

        // ==================== 原有 Run / RunAsync ====================
        public static TimerScope Run(string name) => new TimerScope(name);

        public static void Run(string name, Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            WriteLog($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{ProjectName}] [{name}] 耗时：{sw.Elapsed.TotalMilliseconds:F2} ms");
        }

        public static async Task RunAsync(string name, Func<Task> func)
        {
            var sw = Stopwatch.StartNew();
            await func();
            sw.Stop();
            WriteLog($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{ProjectName}] [{name}] 耗时：{sw.Elapsed.TotalMilliseconds:F2} ms");
        }

        public static async Task<TimerScope> RunAsync(string name)
        {
            return await Task.FromResult(new TimerScope(name));
        }

        // ==================== 写文件 ====================
        private static void WriteLog(string msg)
        {
            try
            {
                using var writer = new StreamWriter(_logPath, append: true);
                writer.WriteLine(msg);
            }
            catch { }
        }

        // ==================== Scope ====================
        public class TimerScope : IDisposable
        {
            private readonly string _name;
            private readonly Stopwatch _sw;

            public TimerScope(string name)
            {
                _name = name;
                _sw = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                _sw.Stop();
                WriteLog($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{Timing.ProjectName}] [{_name}] 耗时：{_sw.Elapsed.TotalMilliseconds:F2} ms");
            }
        }
    }
}