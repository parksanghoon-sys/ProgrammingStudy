using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_5가지_확장
{
    internal static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> SendAsyncWithTimeout(this HttpClient httpClient, HttpRequestMessage request, TimeSpan timeout)
        {
            var cancellationTokenSource = new CancellationTokenSource(timeout);
            try
            {
                await Task.Delay(200);
                Console.WriteLine("비동기 온");
                return await httpClient.SendAsync(request, cancellationTokenSource.Token).ConfigureAwait(false);
            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken == cancellationTokenSource.Token)
                {
                    throw new TimeoutException($"The request timed out after {timeout.TotalSeconds} seconds.");
                }
                else
                {
                    throw;
                }
            }
        }
        public static async Task SendAsyncVoid(this HttpClient httpClient, HttpRequestMessage request, TimeSpan timeout)
        {
            var cancellationTokenSource = new CancellationTokenSource(timeout);
            try
            {
                await Task.Delay(200);
                Console.WriteLine("비동기 온");
                throw new Exception("오류 발생");
            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken == cancellationTokenSource.Token)
                {
                    throw new TimeoutException($"The request timed out after {timeout.TotalSeconds} seconds.");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
