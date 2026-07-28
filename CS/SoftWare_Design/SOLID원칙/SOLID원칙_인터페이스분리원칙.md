# 인터페이스 분리 원칙(ISP)

## 인터펭스 분리 원칙이란 ?
> <span style="color:yellow">인터페이스 분리원칙</span> 이란 <Span Style="color:red;font-weight:bold">객체는 자신이 호출하지 않은 메소드에 의존하지 않아야 한다</Span>라는 원칙이다.

구현할 객체에 무의미한 메소드의 구현을 방지하기 위해 반드시 필요한 메소드만을 상속/구현하도록 권고한다. 만약 상속할 객체의 규모가 너무 크다면, 해당 객체의 메소드를 작은 인터페이스로 나누는 것이 좋다.
## 인터페이스란?
동일한 목적하에 동일한 기능을 수행하게끔 강제하는 것이 인터페이스의 역할이자 개념이다.  
어떤 클래스가 특정한 인터페이스를 상속한다면 반드시 그 클래스는 인터페이스 메소드들을 구현하도록 장제하는것이다.  
**다형성을 극대화하여 개발코드의 수정을 줄이고 유지보수성을 높이기 위해 사용.**

<br> 

## 인터페이스 분리 원칙을 준수한 코드
```c#
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
```
S2는 특수 기능이 구혀되어 있지 않으므로, 기본적인 SamartPhone 객체만을 상속 받아 구현된다.  
인터페이스는 다중 상속을 지원하므로, 필요한 기능을 인터페이스로 나누면 해당 기능만을 상속 받을 수 있다. 그 밖에 추후 업데이트를 통해 추가기능이 탑재 된다면, 같은 원리로 인터페이스를 설계해서 사용시 필요한 기능을 쉽게 추가 할 수 있따.

## 정리 
**인터페이스 분리 원칙은 객체가 반드시 필요한 기능만을 가지도록 제한하는 원칙.**
불필요한 기능의 구현을 최대한 방지함으로써 객체는 불필요한 책임을 제거 한다. 큰 규모의 객체는 필요에 따라 인터페이스로 잘게 나누어 확장성을 향상 시킨다.
<br>

객체를 상속 시 해당 객체가 상속 받는 객체에 적합한 객체인지, 의존적인 기능이 없는지를 판단하여 올바른 객체를 구현, 상속 하도록 해야한다.