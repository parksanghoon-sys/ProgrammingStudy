using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Print(string data, Action<string> action = null)
    {
        string ret = "Print" + data;
        Console.WriteLine(ret);
        if(action != null)
            action(ret);
    }
    static void Main()
    {
        string tmp = "Park";
        Action<string> action = null;
        action = (val) =>
        {
            Console.WriteLine(val + tmp);
        };
        Print("Test");
        Print("Hellow World", action);

        Console.WriteLine("Press any Key ...");
        Console.ReadLine();
    }
}