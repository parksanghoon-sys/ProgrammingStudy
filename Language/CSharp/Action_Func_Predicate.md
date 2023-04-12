# Action\<T> Delegate
Action\<T> Delegate 는 하나의 파라미터를 받고 리턴 값이 없는 함수에 사용되는 **Delegate**이다. 파라미터 수에 따라 0 개부터 16개의 파라미터까지 받아들이는 delegate가 있다. 즉, 파라미터가 없는 Action은 Action delegate, 파라미터가 1개인 Action\<T>delagete 2개인 Action\<T1,T2> ~ 이렇게 16개 파라미터가 있는 Action delegate가 존재한다. **중요한것은 리턴값이 없어야한다**  
아래 예제이다
```c#
using System;
 
namespace Example
{
  // 클래스 생성
  class Program
  {
    // 실행 함수
    static void Main(string[] args)
    {
      // 변수 선언      
      // 익명 함수 생성
      Action<string> action = (val) =>
      {
        // 콘솔 출력
        Console.WriteLine(val);
      };
 
      // 함수 호출
      action("Test");
 
      // 아무 키나 누르시면 종료합니다.
      Console.WriteLine("Press any key...");
      Console.ReadLine();
    }
    //출력
    // Test
  }
}
```
# Func\<T,TResult> Delegate
Func\<T> delegate는 Action과 비슷한데, Action이 리턴이 없는 경우에 사용하는데 Func는 반드시 리턴 타입이 <u>**Generic**</u> 파라미터안에 존재한다, 즉 **Func\<T>의 T는 리턴 값을** 가리키며, 이 경우 입력파라미터는 없다. **(일반적으로 T를 입력과 구분하기 위해 TResult로 표현한다**). Action과 마찬가지로 입력 파라미터수에 따라 여러 변형이 존재하는데, 입력이 1개인 경우 Func<T, TResult>, 입력이 2개인 경우 Func<T1, T2, TResult> 를 사용한다. Action 과 같이 16개까지 입력파라미터를 받아 들일 수 있다.  

예제
```c#
using System;
 
namespace Example
{
  // 클래스 생성
  class Program
  {
    // 실행 함수
    static void Main(string[] args)
    {
      // 익명 함수, 가장 마지막의 파라미터는 반환값
      Func<int, string> func = (val) =>
      {
        // int 값을 받아 100을 곱한다.
        int ret = val * 100;
        // string형식으로 반환한다.
        return ret.ToString();
      };
      // 익명 함수, 반환값이 없다.
      Action<string> action = (val) =>
      {
        // 콘솔 출력
        Console.WriteLine("Action = " + val);
      };
      // 익명 함수 func를 호출 -> 파라미터는 int형을 넘기고 반환값은 string값
      string data = func(10);
      // 익명 함수 action을 호출 -> 파라미터 string형을 넘김
      action(data);
 
      // 아무 키나 누르시면 종료합니다.
      Console.WriteLine("Press any key...");
      Console.ReadLine();
    }
  }
}
```
# Predicate\<T> Delegate
.NET의 Predicate/<T> delegate는 Action/Func delegate와 비슷한데, 리턴값이 반드시 bool이고 입력값이 T 타입인 delegate이다. **Action이나 Func와 달리, 입력 파라미터는 1개이다**. 이 특수한 delegate는 .NET의 Array나 List 클래스의 메서드들에서 자주 사용된다. Predicate/<T>은 **Func/<T, bool>와 같이 표현할 수 있는데**, Func이 실제로 보다 많은 함수들을 표현할 수 있다. Predicate은 .NET 2.0에서 Array나 List등을 지원하기 위해 만들어 졌으며, 보다 일반화된 Func는 .NET 3.5에서 도입되어 LINQ 등을 지원하도록 만들어 졌다.  

예제
```c#
// Predicate<T>
Predicate<int> p = delegate(int n)
{
   return n >= 0;
};
bool res = p(-1);

Predicate<string> p2 = s => s.StartsWith("A");
res = p2("Apple");
```