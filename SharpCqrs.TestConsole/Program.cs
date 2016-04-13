using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using NLog;
using StackExchange.Profiling;

namespace SharpCqrs.TestConsole
{
    static class Program
    {
        internal static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            MiniProfiler.Settings.ProfilerProvider = new SingletonProfilerProvider();
            var profiler = MiniProfiler.Start();

            TestApiPerformance(profiler);

            MiniProfiler.Stop();

            Logger.Info(MiniProfiler.Current.RenderImpl());

            Console.ReadKey();
        }

        private static void TestApiPerformance(MiniProfiler profiler)
        {
            int warmupTimes = 1;
            int testTimes = 0;

            using (profiler.Step("Test API Performance"))
            {
                HttpClient client;
                using (profiler.Step("Create http client"))
                {
                    client = new HttpClient(new HttpClientHandler(), true);
                }
                using (client)
                {
                    using (profiler.Step("Setup http client"))
                    {
                        client.BaseAddress = new Uri("http://localhost:1239/", UriKind.Absolute);
                    }

                    using (profiler.Step("Warming up!"))
                    {
                        for (int i = 0; i < warmupTimes; i++)
                        {
                            CallApi(client, profiler);
                        }
                    }

                    using (profiler.Step("TEST!!!"))
                    {
                        for (int i = 0; i < testTimes; i++)
                        {
                            CallApi(client, profiler);
                        }
                    }
                }
            }
        }

        private static void CallApi(HttpClient client, MiniProfiler profiler)
        {
            using (profiler.CustomTiming("CallApi", "Call cmd/account/create"))
            {
                string json = @"{ Owner: ""Hello world!!!"" }";

                StringContent content;
                using (profiler.CustomTiming("StringContent", "Create http content"))
                    content = new StringContent(json, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> postTask;
                using (profiler.CustomTiming("PostAsync", "cmd/account/create"))
                    postTask = client.PostAsync("cmd/account/create", content, CancellationToken.None);

                HttpResponseMessage response;
                using (profiler.CustomTiming("WaitResponse", ""))
                    response = postTask.GetAwaiter().GetResult();

                string message;
                using (profiler.CustomTiming("GetMessage", ""))
                    message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                // Logger.Trace(message);
            }
        }

        private static string RenderImpl(this MiniProfiler profiler, bool htmlEncode = false)
        {
            if (profiler == null)
                return string.Empty;
            StringBuilder stringBuilder = new StringBuilder().Append(htmlEncode ? HttpUtility.HtmlEncode(Environment.MachineName) : Environment.MachineName).Append(" at ").Append((object) DateTime.UtcNow).AppendLine();
            Stack<Timing> stack = new Stack<Timing>();
            stack.Push(profiler.Root);
            while (stack.Count > 0)
            {
                Timing timing = stack.Pop();
                string str = htmlEncode ? HttpUtility.HtmlEncode(timing.Name) : timing.Name;
                var indent = new string(' ', 2 * (int) timing.Depth);
                var totalDuration = timing.DurationMilliseconds ?? 0;
                stringBuilder.AppendFormat($@"{indent}{str} = {totalDuration.ToString("###,##0.##")}ms");
                stringBuilder.AppendLine();
                if (timing.HasCustomTimings)
                {
                    var keyLength = timing.CustomTimings.Keys.Max(k => k.Length);

                    foreach (var keyValuePair in timing.CustomTimings)
                    {
                        string key = keyValuePair.Key;
                        var list = keyValuePair.Value;
                        var sum = list.Sum(ct => ct.DurationMilliseconds) ?? 0;
                        var count = list.Count;
                        var percent = sum/totalDuration;
                        var avg = sum/count;
                        var plural = count == 1 ? "" : "s";
                        stringBuilder.AppendFormat($@"{indent}  {key.PadRight(keyLength)} ({percent.ToString("P1").PadLeft(7)}) = {avg.ToString("###,##0.00")}ms ({sum.ToString("###,##0.00")}ms in {count} cmd{plural})");
                        stringBuilder.AppendLine();
                    }
                }
                //stringBuilder.AppendLine();
                if (timing.HasChildren)
                {
                    List<Timing> children = timing.Children;
                    for (int index = children.Count - 1; index >= 0; --index)
                        stack.Push(children[index]);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
