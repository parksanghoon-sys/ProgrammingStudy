# 스마트 포인터
스마트 포인터는 표준 포인터의 모든 기능을 가지고 있으며 자동으로 가비지 컬렉션 기능을 제공하는 클래스이다. Qt로 프로그래밍시 메모리 누수를 도와주는 스마트포인터를 설명한다.
## QSharedPoint
---
QSharedPointsms 개체 외부에 배치된 참조를 통해 공유 포인터를 보유한다 C++의 `std::shared_ptr`과 같다. constness를 포함하여 일반적인 목적을 위해 일반적인 포인터와 같이 동작한다.  
다른 QSharedPointer 개체가 참조하지 않는 한 QSharedPointer는 범위를 벗어날 때 보유하고 있는 포인터를 삭제한다.

```c++
int *pI = new int;
QSharedPointer<int> pI1(pI);
QSharedPointer<int> pI2 = pI1;
pI1.clear();
// pI2는 여전히 pI를 가리키고 있으므로 삭제되지 않는다.
pI2.clear();
// 더 이상 공유하는 포인터가 없으므로 pI가 삭제된다.
```
## QScopedPointer
---
QScopedPointer는 단순히 힙 할당 객체에 대한 포인터를 보유하고 소멸자에서 삭제된다. 따라서 현재 범위를 벗어날 때 가리키는 객체가 삭제되도록 보장한다.
```C++
MyClass *foo() {
    QScopedPointer<MyClass> myItem(new MyClass);
    // Some logic
    if (some condition) {
        return nullptr; // myItem은 여기서 삭제된다.
    }
    return myItem.take(); // 범위가 지정된 포인터에서 항목을 해제하고 반환한다.
}
```
## QPointer
---
QPointer은 QObject 파생클래스 인스턴스에 대한 보호된 포인터를 제공하는 템플릿 클래스이다. 다른 누군가 소유한 QObject에 대한 포인터를 저장해야 할때 유용하며 참조를 계속 보유하는 동안 파괴될수 있다.
```C++
QObject *obj = new QObject;
QPointer<QObject> pObj(obj);
delete obj;
Q_ASSERT(pObj.isNull()); // pObj는 이제 nullptr이 된다.
```
외부 소단을 통해 개체가 삭제되지 않음을 보장할 때 QPointer를 사용하여 개체에 엑세스가 가능하다.
## QWeakPointer
공유 포인터에대한 약한 참조를 보유한다. `std::week_ptr`과 동일하며 lock()ㅡㄴ toStringRef()와 동일하다.
```c++
int *pI = new int;
QSharedPointer<int> pI1(pI);
QWeakPointer<int> pI2 = pI1;
pI1.clear();
// 더 이상 공유 포인터가 없으며 pI가 삭제된다.
//
// 참조 횟수를 증가시켜 삭제되지 않도록 한다.
QSharedPointer<int> pI2_locked = pI2.toStrongRef();
Q_ASSERT(pI2_locked.isNull());
```
