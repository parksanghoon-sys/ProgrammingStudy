# 약한 포인터 | std::weak_ptr
week_ptr는 unique_ptr, shared_ptr과 다르게 단독으로 사용이 불가능한 포인터이다.  
## 강한 참조
---
shared_ptr은 참조카운팅을 사용해 생성 소멸을 관리한다. 참조 종류에는 강한 참조와 약한 참조가 있고, 한개의 원시 포인터를 3개의 공유 포인터가 소유한다면 참조 카운터는 3이지만 해당 카운터는 강한 참조이다. 만약 3개중 1개가 사라진다면 strong ref는 1이 감소하고 증가시 1이 증가한다.  
## weak_ptr
---
`weak_ptr`은 단독으로 사용이 불가능하다. unique_ptr 또는 `shared_ptr`은 원시 포인터를 건네받아 내부적으로 참조하게 되있다면, **weak_ptr은 원시포인터대신 shared_ptr을 건네받아 shared_ptr가 소유한 원시포인터를 참조하도록 한다.**  

`std::shared_ptr`의 보조 역할을 합니다. `std::shared_ptr`은 객체에 대한 참조 카운트를 관리하므로, 한 객체에 대해 여러 개의 `std::shared_ptr`이 존재하는 경우 참조 카운트가 0이 될 때까지 객체가 소멸되지 않습니다. 이는 객체의 수명을 연장할 수 있는 장점이 있지만, 때로는 객체가 삭제된 후에도 포인터가 남아 있어 메모리 누수를 유발할 수 있습니다. 

### **shared_ptr 과 weak_ptr의 차이**
1. `std::weak_ptr`은 객체에 대한 약한 참조를 제공하지만, 객체를 직접 소유하지 않으므로 객체의 수명을 연장할 수 없습니다.
2. `std::shared_ptr`은 객체의 수명을 관리하기 때문에 참조 카운트가 0이 될 때까지 객체가 소멸되지 않습니다. 하지만 `std::weak_ptr`은 객체를 직접 소유하지 않기 때문에 객체가 소멸되어도 참조하던 포인터는 그대로 유지됩니다. 따라서 std::weak_ptr을 사용할 때는 `std::shared_ptr`이 여전히 유효한지 검사하는 것이 좋습니다.
3. `std::weak_ptr`은 `std::shared_ptr`으로부터 생성될 때만 사용할 수 있습니다. 따라서 객체를 직접 생성하고 `std::weak_ptr`으로 약한 참조를 만드는 것은 불가능합니다.
4. `std::weak_ptr`은 `std::shared_ptr`과 달리 객체에 대한 포인터를 직접 가지고 있지 않습니다. 대신 `lock()` 함수를 사용하여 `std::shared_ptr`으로 변환할 수 있습니다. 이때 **객체가 이미 삭제되었거나 `std::shared_ptr`이 만료된 경우 `lock()` 함수는 `nullptr`을 반환**합니다.

### **예제**
```c++
#include <memory>
#include <iostream>

class MyClass {
public:
    MyClass() { std::cout << "MyClass 생성자 호출" << std::endl; }
    ~MyClass() { std::cout << "MyClass 소멸자 호출" << std::endl; }
};

int main() {
    std::shared_ptr<MyClass> p1(new MyClass());
    std::weak_ptr<MyClass> wp1 =
    std::weak_ptr<MyClass> wp1 = p1;

    std::cout << "shared_ptr count: " << p1.use_count() << std::endl;
    std::cout << "weak_ptr count: " << wp1.use_count() << std::endl;

    // std::weak_ptr를 std::shared_ptr로 변환하여 객체를 사용
    if (auto p2 = wp1.lock()) {
        std::cout << "객체 사용" << std::endl;
    }
    else {
        std::cout << "객체 삭제됨" << std::endl;
    }

    p1.reset();

    std::cout << "shared_ptr count: " << p1.use_count() << std::endl;
    std::cout << "weak_ptr count: " << wp1.use_count() << std::endl;

    // 객체가 이미 삭제되었으므로 nullptr을 반환
    if (auto p3 = wp1.lock()) {
        std::cout << "객체 사용" << std::endl;
    }
    else {
        std::cout << "객체 삭제됨" << std::endl;
        wp1.
    }

    return 0;
}
```

이 코드에서는 `std::shared_ptr`을 생성한 후 `std::weak_ptr`로 약한 참조를 만듭니다. `std::shared_ptr`의 참조 카운트와 `std::weak_ptr`의 참조 카운트를 출력한 후, `std::weak_ptr`을 `lock()` 함수를 사용하여 `std::shared_ptr`으로 변환합니다. 이때 `std::shared_ptr`이 여전히 유효한 경우 객체를 사용하고, 이미 삭제된 경우 `nullptr`을 반환합니다.

그 다음 `std::shared_ptr`의 `reset()` 함수를 사용하여 객체를 삭제한 후, 다시 한 번 참조 카운트를 출력합니다. 마지막으로 `std::weak_ptr`을 `lock()` 함수를 사용하여 `std::shared_ptr`으로 변환하면 이미 삭제된 객체를 가리키므로 `nullptr`을 반환합니다.

### **expired 멤버 함수**
`weak_ptr`의 멤버 함수 중 `expired`라는 함수가 존재한다. 이는 해당 `weak_ptr`가 expired 상태인지 확인하는 함수이다. 즉 현재 참조하는 원시 포인터가 소멸되었다면 "expired" 상태이므로 true를 반환하고, 존재한다면 false를 반환한다.
### **reset을 통해 참조 끊기**
로 `reset` 함수를 통해 현재 소유 중인 원시 포인터를 버릴 수 있다. 역시 메모리 해제가 아닌 단순히 일방적으로 참조를 끊는 것이며 strong ref가 아닌 weak ref(count)가 감소한다. 아까도 말했지만 weak ref가 0이 된다고 원시 포인터가 해제되지 않는다. 원시 포인터를 버린 weak_ptr 개체는 empty 상태가 된다.