class Program
{
    private delegate void Print(string val);
    private static Print list;
    static void Main()
    {
        list += (val) =>
        {
            Console.WriteLine($"Lamda1 {val.ToString()}");
        };
        list += (val) =>
        {
            Console.WriteLine($"Lamda2 {val.ToString()}");
        };
        list("Hellow World");
        Console.WriteLine("Press any Key ...");
        Console.ReadLine();
    }
}