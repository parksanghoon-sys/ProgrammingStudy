using System.Runtime.CompilerServices;

public abstract class SamrtPhone
{
    public void Call(string number)
    {
        Console.WriteLine(number);
    }
    public virtual void Message(string number, string message)
    {
        Console.WriteLine($"{number} : {message}");
    }
}
/// <summary>
/// 무선 충전기능
/// </summary>
interface IWirelessChargable
{
    void WireLessCharge();
}
/// <summary>
/// AR 기능
/// </summary>
interface IARable
{
    void Ar();
}
/// <summary>
/// 생채인식 기능
/// </summary>
interface IBiometricsable
{
    void Biometrics();
}
public class Phone23 : SamrtPhone, IWirelessChargable, IARable, IBiometricsable
{
    public void Ar()
    {
        Console.WriteLine("AR 기능");
    }

    public void Biometrics()
    {
        Console.WriteLine("얼굴인식 기능");
    }

    public void WireLessCharge()
    {
        Console.WriteLine("무선충전 기능");
    }
}
public class Phone2 : SamrtPhone
{
    public override void Message(string number, string message)
    {
        Console.WriteLine("In S2");
        base.Message(number, message);
    }
}
class Program
{
    public static void Main()
    {
        string? number = "010-1111-2222";
        string? message = "TEST";

        Phone23 phone = new Phone23();
        phone.Message(number, message);
        phone.Biometrics();

        Phone2  phone2= new Phone2();
        phone2.Message(number, message);
        
        Console.ReadLine();
    }
}