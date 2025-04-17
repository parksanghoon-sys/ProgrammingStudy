# 의존성 역전 법칙(DIP)

## 의존성 역전 원칙이란?
> 의존성 역전 원칙이란 객체는 저수준 모듈보다 고수준 모듈에 의존해야 한다는 원칙이다.
> 

* 고수준 모듈 : 인터페이스 및 추상 클래스
* 저수준 모듈 : 구현된 객체 일반 클래스 같은것

DIP는 의존관계를 맺을때 변하기 쉬운 것 또는 자주 변화하는 것에 의존하기 보다는, **변화하기 어려운것, 거의 변화가 없는 것**에 의존하라는 원칙이다.
![Example](/SoftWare_Design/SOLID%EC%9B%90%EC%B9%99/Dip_Example.png)
<br>
**이 경우 아이가 장난감을 가지고 노는 사실은 변하기 어려운 것이다.** 다만 어떤것을 가지고 노는것은 자주 변화하는것이다. (인터페이스, 추상클래스 = 변하지 않는것, 구현체 클래스 - 변하기 쉬운것)  
Dip를 만족하려면 어떤 클래스가 도움을 받을 때 구체적인 클래스 보다 인터페이스나 추상클래스에 의존관계를 맺도록 설계를 해야한다.  
**의존성 주입 : 말 그대로 클래스 외부에서 의존되는 것을 대상의 객체의 인스턴스 변수에 주입하는 기술**

## 코드로보는 의존성 역전 원칙
```c#
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
```
이렇게 아이가 가지고 노는 장난감이 단순히 장난감에 종속적이지 않고 어떤 장난감이던 의존성을 Toy에 주입하는 클래스의 의존관계를 역전하여 작성 할 수 있도록 하는게 DIP 원칙이다.  
DIP원칙을 준수하게 된다면 자연스레 OCP(계방 폐쇠 원칙)에 맞는 설계가 될수 있다.  
<br>
**객체에 대한 생성의 주입을 하위 모듈중 어떠한것을 넣어도 동일한 기능을 하는 것이 DIP 원칙을 바람직하게 준수한 방법이라고 생각한다**
