## 대리자 계층 이해

NET의 대리자는메서드에 대한 원격 컨트롤과 같다, 마치 변수인 것 처럼 함수를 전달하고 트리거 할 수있으므로 나중에 어떤 메서드가 실행되는지 결정할 수 있는 권한을 제공한다.

### Delegate Type 에코 시스템

현제 Net에서는 서로다른 용도로 사용되는 여러가지 기본 대리자를 제공하며 각 대리자는 사용사례와 특성이 있다.

![1752480304123](image/NET의대리자,이벤트및콜백심층가이드/1752480304123.png)

위 그림에서 볼 수 있듯 대리자는 단순성과 다양성으로 인해 C# 개발을 이끈다, 사용자 지정 디리자는 강력하지만 NET2.0의 Generic 도입으로 잘 사용되지 않는다.

## Types of Delegates 심층 분석

각 대리자를 사용해야 하는 경우를 이해하기 위해 다양한 유형의 대리자를 더 자세히 살펴보겠습니다.

* 사용자 지정 대리자사용자 지정 대리자는 메서드 서명을 정확하게 제어할 수 있으며 특수한 콜백 시나리오에 적합합니다. 키워드를 사용하여 선언되며 필요한 모든 매개 변수 및 반환 형식 조합을 포함할 수 있습니다.`delegate`

```csharp
public delegate decimal TaxCalculationDelegate(decimal amount, string region);
public delegate bool ValidationDelegate<T>(T item);
```

사용자 지정 대리자는 기본 제공 대리자가 특정 요구 사항과 일치하지 않거나 API를 보다 자체 문서화하려는 경우에 사용해야 합니다.

* 일반 기본 제공 대리자: .NET Framework에서는 대부분의 사용 사례를 다루는 세 가지 기본 제네릭 대리자 형식을 제공합니다.- Action `<T>` 대리자: 대리자는 값(void)을 반환하지 않고 최대 16개의 매개변수를 사용할 수 있는 메서드를 나타냅니다. 결과 없이 작업을 수행하는 작업에 적합합니다. 이러한 대리자는 이벤트 처리기, 로깅 작업 및 모든 void 반환 메서드에 적합합니다.`Action<T>`

```csharp
Action simpleAction = () => Console.WriteLine("Hello!");
Action<string> logMessage = msg => Console.WriteLine(msg);
Action<int, string> processItem = (id, name) => Database.Save(id, name);
```

- Func<T, TResult> 대리자: 대리자는 값을 반환하는 메서드를 나타내며 최대 16개의 입력 매개 변수를 사용할 수 있습니다. 매개 변수의 마지막 형식은 항상 반환 형식을 나타냅니다. Func 대리자는 LINQ 쿼리, 변환 및 계산된 결과가 필요한 모든 시나리오에서 빛을 발합니다.`Func<T, TResult>`

```csharp
Func<string> getCurrentTime = () => DateTime.Now.ToString();
Func<int, int, int> calculator = (x, y) => x + y;
Func<List<int>, int> findMax = numbers => numbers.Max();
```

- Predicate `<T>` 대리자: 대리자는 하나의 매개변수를 취하고 를 반환하는 메서드를 나타내는 특수 버전입니다. 필터링 및 유효성 검사 논리에 자주 사용됩니다. .NET Framework는 List `<T>`와 같은 컬렉션 클래스에서 및 와 같은 메서드에 대해 Predicate를 광범위하게 사용합니다.`Predicate<T>``Func<T, bool>``boolean``FindAll``RemoveAll`

```
Predicate<int> isEven = number => number % 2 == 0;
Predicate<string> isValidEmail = email => email.Contains("@");
```

## 멀티캐스트 대리자 및 오류 처리

NET의 모든 대리자는 멀티캐스트이므로 순차적으로 호출되는 여러 메서드에 대한 참조를 보유할 수 있습니다. 이 강력한 기능을 사용하면 이벤트 기반 프로그래밍을 수행할 수 있지만 예외를 신중하게 처리해야 합니다.

**멀티캐스트 대리자의
문제점**호출 목록의 대리자가 예외를 throw하면 전체 호출 체인이 중단됩니다. 이 동작은 특히 이벤트 기반 응용 프로그램에서 예기치 않은 결과를 초래할 수 있습니다.

```csharp
Action<string> loggers = message => Console.WriteLine($"Logger 1: {message}");
loggers += message => throw new Exception("Logger 2 failed!");
loggers += message => Console.WriteLine($"Logger 3: {message}"); //never executed if Logger 2 fails
```

이 문제를 해결하려면 개발자가 호출 목록을 수동으로 반복하고 각 대리자에 대한 예외를 처리해야 합니다. 이 패턴은 호출 목록의 모든 대리자가 일부 실패하더라도 호출되도록 합니다.

```csharp
foreach (Action<string> logger in loggers.GetInvocationList())
{
    try
    {
        logger("Log message");
    }
    catch (Exception ex)
    {
        //handle or log the exception without breaking the chain
        Console.WriteLine($"Logger failed: {ex.Message}");
    }
}
```

### NET의 이벤트 패턴

Net의  이벤트는 게시자-구독자 관계로 관찰자 패턴을 구현하기 위해 대리자를 기반으로 한다, 다양한 이벤트 패턴은 다양한 수준의 메모리 안전성, 복잡성 및 성능을 제공한다.

> NET의 이벤트는 대리자 위에 빌드된 알람 시스템과 같다. 프로그램의 한 부분이 다른 부분에 신호를 보내고 관심있는 청취자가 자신의 방식으로 응답하는 방법이다

* 메모리 누수 문제
  * NET의 표준 이벤트는 구독자가 제대로 취소하지 않을 때 메모리 누수를 일으킬 수 있다, 이는 게시자가 구독자에 대한 강력한 참조를 보유하며 가비지 수집을 방지하기 때문에 발생한다, 인스턴스가 더이상 필요하지 않지만 이벤트 구독을 취소하지 못하는 경우 존재하는 한 가비지 수집이 되지 않는다.
  * ```csharp
    public class Publisher
    {
        public event EventHandler DataChanged;

        public void RaiseEvent()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    //subscriber with potential memory leak
    public class Subscriber
    {
        private Publisher _publisher;

        public Subscriber(Publisher publisher)
        {
            _publisher = publisher;
            _publisher.DataChanged += OnDataChanged; // Subscription creates reference
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Data changed!");
        }

        //missing: proper unsubscription in Dispose or finalizer
    }
    ```
* 약한 이벤트 패턴
  * 약한 이벤트 패턴은 구독자에 대한 약한 참조를 사용하여 메모리 누수 문제를 해결합니다. 이렇게 하면 가비지 수집기가 구독자 개체가 이벤트에 등록된 상태로 유지되더라도 해당 개체를 회수할 수 있습니다. 이 패턴은 수명이 긴 시스템에서 더 나은 메모리 안전성을 제공하는 동시에 이벤트 기반 아키텍처의 이점을 유지
  * ```csharp
    public class WeakEventManager
    {
        private readonly List<WeakReference> _subscribers = new();

        public void Subscribe(EventHandler handler)
        {
            _subscribers.Add(new WeakReference(handler));
        }

        public void Raise(object sender, EventArgs e)
        {
            for (int i = _subscribers.Count - 1; i >= 0; i--)
            {
                if (_subscribers[i].IsAlive)
                {
                    var handler = _subscribers[i].Target as EventHandler;
                    handler?.Invoke(sender, e);
                }
                else
                {
                    _subscribers.RemoveAt(i); //clean up dead references
                }
            }
        }
    }
    ```

## 대리자 vs 이벤트

대리자와 이벤트는 서로 관련된 개념이지만 응요프로그램 디자인에서 서로 다른 용도로 사용이 된다, 유지 관리가능한 코드를 작성하기 위해서는 각각을 언제 사용해야하는 것이 중요하다

### 대리자 사용 시 이점

* 동작을 매개변수로 전달 해야한다
* 메서드에는 값을 반환하는 콜백이 필요하다
* 호출자는 실행할 메서드를 결정해야한다
* 함수형 프로그래밍 패턴을 구현

### 이벤트를 사용 시 이점 순간

* 게시자 - 구독자 패턴
* 여러 구독자에게 알림이 필요할 때
* 소유 클래스만 알림을 트리거 할 수 있는 규칙을 적용
* 구독자는 게시자보다 수명이 더길어질 수있다

## Enterprise Applications의 실제 대리자 시나리오

### 플러그인 아키택쳐

대리자를 사용하여 핵심 기능을 수정하지 않고 구성요소를 추가하거나 교체할 수있는 플러그인 시스템을 지원한다, 이 접근 방식을 사용하면 타 개발자가 소스코드에 엑세스 하지 않고 앱을 확장이 가능하다

```csharp
public interface IPluginHost
{
    void RegisterDataProcessor(Func<string, string> processor);
    void RegisterValidator(Predicate<object> validator);
    void RegisterCompletionCallback(Action<ProcessingResult> callback);
}
```

### 비지니스 규칙 엔진

대리자는 엔터프라이즈 시스템에서 동적 비지니스 규칙을 평가를 가능하게 한다, 이 패턴을 사용하면 비지니스 규칙을 다시 컴파일 하지않고 런타임에 제거 및 수정이 가능하다

```csharp
public class RuleEngine
{
    private List<Func<Order, bool>> _validationRules = new();
  
    public void AddValidationRule(Func<Order, bool> rule)
    {
        _validationRules.Add(rule);
    }
  
    public bool ValidateOrder(Order order)
    {
        return _validationRules.All(rule => rule(order));
    }
}
```

### 마이크로서비스 이벤트 기반 아키택처

이벤트 기반 패턴을 활용하여 서비스간의 느슨한 결함을 유지한다, 이 아키텍쳐를 사용시 서비스 톡립적으로 발전시키면서 이벤트를 통해 통신을 유지가 가능하다

```csharp
//publisher service
public class OrderService
{
    public event EventHandler<OrderEventArgs> OrderPlaced;
  
    public void PlaceOrder(Order order)
    {
        //process order
        OrderPlaced?.Invoke(this, new OrderEventArgs { Order = order });
    }
}

//subscriber services
public class NotificationService
{
    public NotificationService(OrderService orderService)
    {
        orderService.OrderPlaced += (sender, e) => SendNotification(e.Order);
    }
  
    private void SendNotification(Order order) { /* some logic */ }
}

public class InventoryService
{
    public InventoryService(OrderService orderService)
    {
        orderService.OrderPlaced += (sender, e) => UpdateInventory(e.Order);
    }
  
    private void UpdateInventory(Order order) { /* some logic */ }
}
```


## 모범 사례 및 성능 최적화:

대리자 및 이벤트로 작업할 때 최선의 구현 방법을 따르면 최적의 성능과 유지 관리성을 보장할 수 있습니다.

* 대리자 성능 최적화:- 대리자 인스턴스를 각 호출에 대해 다시 만드는 대신 캐시합니다.- 가능한 경우 대리자에 정적 메서드를 사용하여 캡처를 피하고 성능 향상을 위해 메서드 그룹 변환(예: 대신)을 고려합니다.- 성능이 중요한 코드에서 멀티캐스트 대리자를 사용할 때는 주의해야 합니다.`this<br/>-<span> </span>``handler = MethodName``handler = x => MethodName(x)`
* 이벤트 모범 사례:- null 조건부 연산자()를 사용하여 이벤트를 호출하기 전에 항상 null을 확인합니다.- 삭제 가능한 개체에서 이벤트 구독 취소를 사용하여 적절한 폐기를 구현합니다.- 수명이 긴 게시자를 위한 약한 이벤트 패턴을 고려합니다.- 사용자 지정 대리자 유형보다 제네릭을 사용합니다.- 애플리케이션 충돌을 방지하기 위해 이벤트 처리기에서 예외를 적절하게 처리합니다.`?.Invoke()``EventHandler<T>`
* 콜백에 대한 모범 사례:- 대부분의 시나리오에 기본 제공 대리자(, )를 사용하면 간결하고 형식이 안전합니다.- 비동기 콜백에서 기본 스레드를 차단하지 말고 가능하면 사용합니다.- 예기치 않은 충돌을 방지하기 위해 콜백 내에서 예외를 처리합니다.- 콜백 기대치를 명확하게 문서화합니다(언제, 얼마나 자주, 어떤 스레드에서 호출될지).`Action``Func``async/await`


## 결론

대리자, 이벤트 및 콜백은 NET에서 느슨하게 결합된 유연한 시스템의 중추를 형성한다, 이를 통해 보다 유지관리 가능하고 확자잉 가능한 애플리케이션을 만들 수 있다.

대리자는 강력한 기능을 제공하지만 성능 및 유지 관리성에 미치는 영향을 신중하게 고려하여 신중하게 사용해야한다.
