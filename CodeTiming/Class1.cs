using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CodeTimer
{
    public static class Timing
    {
        private static Stopwatch? _globalSw;
        private static string? _globalName;
        private static readonly string _logPath = @"D:\CodeTimerLog.txt";

        #region 全局计时 Start / Stop
        public static void Start(string name)
        {
            _globalName = name;
            _globalSw = Stopwatch.StartNew();
        }

        public static void Stop()
        {
            if (_globalSw == null || string.IsNullOrEmpty(_globalName))
                return;

            _globalSw.Stop();
            string msg = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{_globalName}] 耗时：{_globalSw.Elapsed.TotalMilliseconds:F2} ms";
            WriteLog(msg);

            // 重置
            _globalSw = null;
            _globalName = null;
        }
        #endregion

        #region 原有计时方法
        public static TimerScope Run(string name)
        {
            return new TimerScope(name);
        }

        public static void Run(string name, Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            string msg = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{name}] 耗时：{sw.Elapsed.TotalMilliseconds:F2} ms";
            WriteLog(msg);
        }

        public static async Task RunAsync(string name, Func<Task> func)
        {
            var sw = Stopwatch.StartNew();
            await func();
            sw.Stop();
            string msg = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{name}] 耗时：{sw.Elapsed.TotalMilliseconds:F2} ms";
            WriteLog(msg);
        }

        public static async Task<TimerScope> RunAsync(string name)
        {
            return await Task.FromResult(new TimerScope(name));
        }
        #endregion

        #region 自动写入 D 盘日志文件
        private static void WriteLog(string msg)
        {
            try
            {
                // 追加写入 D:\CodeTimerLog.txt
                using (var sw = new StreamWriter(_logPath, append: true))
                {
                    sw.WriteLine(msg);
                }
            }
            catch
            {
                // 写入失败不影响业务
            }
        }
        #endregion

        public class TimerScope : IDisposable
        {
            private readonly string _name;
            private readonly Stopwatch _stopwatch;

            public TimerScope(string name)
            {
                _name = name;
                _stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                string msg = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{_name}] 耗时：{_stopwatch.Elapsed.TotalMilliseconds:F2} ms";
                WriteLog(msg);
            }
        }
    }
}