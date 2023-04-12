namespace Task_5가지_확장;

class Program
{
    public static async Task Main(string[] args)
    {
        //Action someAction = () =>
        //{
        //    Thread.Sleep(3000);
        //    Console.WriteLine("Print Task");
        //};
        //Task myTask = new Task(someAction);
        //HelperTask.FireAndForget(myTask);

        //Console.WriteLine("실행완료");

        using var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "https://jsonplaceholder.typicode.com/todos/1");

        //var response = await (httpClient.SendAsyncWithTimeout(request, TimeSpan.FromSeconds(10))).Retry(3, TimeSpan.FromSeconds(1));

        var response = await HelperTask.Retry(async () =>
        {
            var result = await httpClient.SendAsyncWithTimeout(request, TimeSpan.FromSeconds(10));
            return result;
        },3,TimeSpan.FromSeconds(1));
        //httpClient.SendAsyncVoid(request, TimeSpan.FromSeconds(10)).FireAndForget(errorHandler => 
        //Console.WriteLine(errorHandler.Message));

        Console.WriteLine($"status code: {(int)response.StatusCode} {response.StatusCode}");
        Console.WriteLine($"content: {await response.Content.ReadAsStringAsync()}");

        Console.WriteLine($"Suscess");

        Console.ReadLine();
    }

}
