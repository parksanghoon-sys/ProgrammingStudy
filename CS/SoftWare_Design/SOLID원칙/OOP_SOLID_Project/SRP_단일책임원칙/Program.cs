public abstract class Car
{
    protected readonly string _wd;
	protected readonly int[] Whell = { 0, 0, 0, 0 };
    public Car(string wd)
	{
		_wd = wd;
	}
	public abstract void Run(int power);
}
class FontWhellCar : Car    
{
	public FontWhellCar(string wd) : base(wd)
	{

	}
    public override void Run(int power)
    {
		Whell[0] = power;
		Whell[1] = power;
		Console.WriteLine($"{_wd} 휠 동력상태 {Whell[0]} , {Whell[1]} , {Whell[2]}, {Whell[3]}");
    }
}
class BackWhellCar : Car
{
    public BackWhellCar(string wd) : base(wd)
    {

    }
    public override void Run(int power)
    {
        Whell[2] = power;
        Whell[3] = power;
        Console.WriteLine($"{_wd}휠 동력상태 {Whell[0]} , {Whell[1]} , {Whell[2]}, {Whell[3]}");
    }
}
class AllWhellCar : Car
{
    public AllWhellCar(string wd) : base(wd)
    {

    }
    public override void Run(int power)
    {
        Whell[0] = power;
        Whell[1] = power;
        Whell[2] = power;
        Whell[3] = power;
        Console.WriteLine($"{_wd}휠 동력상태 {Whell[0]} , {Whell[1]} , {Whell[2]}, {Whell[3]}");
    }
}
class Program
{
    public static void Main()
    {
        Car car = new AllWhellCar("All");
        car.Run(100);
        car = new BackWhellCar("Back");
        car.Run(50);
        car = new FontWhellCar("Front");
        car.Run(20);
        Console.ReadLine();
    }
}
