#  유니크 포인터 | std::unique_ptr
C++ 11부터 특별한 포인터 클래스가 포함되었다. 스마트 포인터라고 불리며 포인터 사용 시 사용자의 실수에 의한 메모리 누수(memory leak)를  방지하고 안전한 사용을 위해 나온 3가지 클래스가 등장하였다.  
* std::unique_ptr 
* std::shared_ptr
* std::weak_ptr

## unique_ptr
---
[특징]
* 해당 주소의 소유자는 한명이다
* 소유권을 이전 할수 있지만, 복사 대입과 같은 고유는 불가능하다
* 객체가 소멸시 포인터도 소멸된다.
  
[생성 방식]
```c++
class Foo
{
public:
   Foo(const char* name, unsigned int age) {}
   void DoSomething();
   ...
};

void MakeUniqueExample()
{
   // std::make_unique 함수를 통한 unique_ptr 생성 
   std::unique_ptr<Foo> f = std::make_unique<Foo>("foo", 14);
   
   f->DoSomthing(); // f가 소유한 원시 포인터의 멤버에 접근
   f.reset();       // 유니크 포인터의 멤버 함수 
}
```
[원시 포인터 해제 및 재설정]
```c++
class Foo {};

void GetUniquePointer()
{
   auto f = std::make_unique<Foo>();
   f.reset();     // 원시 포인터를 해제
   //f = nullptr; // 원시 포인터를 해제
   f.reset(new Foo()); // 기존 원시 포인터를 삭제 후 새 원시 포인터를 소유함 
}
```
**[단점]**  
함수에 인자로 포인터를 넘길경우 get을 통해 원시 포인터를 반환한다. 그러나 외부에서 지워버릴수 있어 위험하다, 또한 반환해서 다른곳에서 사용한다는 것 자체가 스마트 포인터를 사용하는 이점을 감소시킨다, 이는 공유가 아닌 이동(move)를 통해 자신의 소유권을 포기하고 다른 유니큐포인터에 넘겨도 가능 하지만, 초인터를 함수에 전달해야하는경우에 문제가 발생한다.