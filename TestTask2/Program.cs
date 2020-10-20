using System;
using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestTask2
{
    class Program
    {
        private const string ListenerUri = "http://localhost:1234/";
        private static readonly ConcurrentDictionary<int, int> _countByThreadId = new ConcurrentDictionary<int, int>();

        static async Task Main(string[] args)
        {
            using (var source = new CancellationTokenSource())
            {
                try
                {
                    var task = ListenAsync(source.Token);
                    Console.WriteLine($"Listener started at {ListenerUri}");
                    Console.WriteLine("Press any key to stop listening.");
                    Console.ReadKey(true);
                    source.Cancel();
                    await task;
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        private static async Task ListenAsync(CancellationToken token)
        {
            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add(ListenerUri);
                listener.Start();
                token.Register(() => listener.Abort());
                while (!token.IsCancellationRequested)
                {
                    var context = await listener.GetContextAsync();
                    var id = Thread.CurrentThread.ManagedThreadId;
                    _countByThreadId.AddOrUpdate(id, 1, (id, count) => count + 1);
                    var html = $"<html><body>Thread {id}, request {_countByThreadId[id]}</body></html>";
                    var bytes = Encoding.UTF8.GetBytes(html);
                    var response = context.Response;
                    response.ContentLength64 = bytes.Length;
                    using (var output = response.OutputStream)
                    {
                        response.OutputStream.Write(bytes, 0, bytes.Length);
                    }
                }
                listener.Stop();
            }
        }
    }
}
