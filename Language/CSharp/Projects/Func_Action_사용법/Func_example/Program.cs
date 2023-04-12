class Program
{

    static void Main()
    {
        Func<int, string> func = (val) =>
        {
            int ret = val * 100;
            return ret.ToString();
        };
        Action<string> action = (val) =>
        {
            Console.WriteLine($"Action : {val}");
        };
        string data = func(10);
        action(data);
        Console.WriteLine("Press any Key ...");
        Console.ReadLine();
    }
}