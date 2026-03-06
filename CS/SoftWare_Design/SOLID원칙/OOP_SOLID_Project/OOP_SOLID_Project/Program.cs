class Kid
{
    private readonly Toy _toy;

    public Kid(Toy toy)
    {
        _toy = toy; 
    }
    public void Play()
    {
        Console.WriteLine(_toy.toString());
    }
}
public class Robot : Toy
{
    public string toString()
    {
        return "Robot";
    }
}
public class Lego : Toy
{
    public string toString()
    {
        return "Lego";
    }
}
internal interface Toy
{
    public string toString();
}
class Program
{
    public static void Main()
    {
        //Toy t = new Robot();
        Toy t = new Lego();
        Kid kid = new Kid(t);        
        kid.Play();
        Console.ReadLine();
    }
}