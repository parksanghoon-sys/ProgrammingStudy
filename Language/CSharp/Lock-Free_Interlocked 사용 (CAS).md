## Lock Free 란?

멀티 스레드에서 데이터의 동시접근을 막기위한 lock 없이 Non Blocking으로 동기화 처리 할수 있는 방법을 말한다

### Lock 단점

* 성능 하락
  * 여러 스레드가 하나의 공유자원을 획득하기 위해 경쟁할 때 CPU는 어떤 스레드에 자원을 허락해야 할지 연산에 있어 성능 저하가 발생
* 라이브락
  * 어떤 스레드가 락을 획득하고 해제되지 않으면 프로그램이 블로킹 될수 있다. 이를 데드락 이라고 부른다

## Compare And Exchange (CAS)

Lock-Free를 구현하는 방법에는 여러가지가 있지만 선형자료구조인 Queue 와 Tack, 이진트리 구조를 사용된다. 이런 자료구조를 이용해 처리연산 방법에 해당되는것이 CAS 이다

CAS는 특정 값을 비교해 같다면 X의 값으로 교체하는 연산 처리를 말한다

동기화처리를 차단하지 않고 Non-Blocking 하게 처리되도록 하여 성능은 유지하고 데드락 발생을 방지한다.

해당 Lock Free는 주로 게임서버 같은 극대의 성능을 끌어올리면서 비동기 방식으로 처리해야하는 서버 환경에 많이 사용된다.

## Interlocked

c# 에서는 Lock-Free를 비교적 쉽게 구현이 가능하다. System.Threading.Interlocked 클래스에는 CAS를 사용할 수 있는 *CompareExchange* 메서드를 제공한다

예제는 아래와 같다

```csharp
// CAS를 이용한 Non-Blocking 동기 처리 클래스
public class CAS_Lock
{
        // 0 = false
        // 1 = true
        private volatile int _lock = 0;

        public void Lock()
        {
            while(true)
            {
                if(Interlocked.CompareExchange(ref _lock, 1, 0) == 0)
                {
                    return;
                }
            }
        }

        public void Free()
        {
            _lock = 0;
        }
}

CAS_Lock _cas = new CAS_Lock();
private async void StartWorker()
{
  Enumerable.Range(0, 10).ToList().ForEach(item =>
  {
    Task.Run(this.Worker);
  });
}

private void Worker()
{
  _cas.Lock();

  Console.WriteLine($"Lock 획득! - id : {System.Threading.Thread.CurrentThread.ManagedThreadId} / {DateTime.Now.Second}s");
  Thread.Sleep(1000);

  _cas.Free();
}
```

위와 같이 System.Threading.Interlocked 를 사용해 Non-Blocking 동기 처리를 수행하는 CAS_Lock 클래스를 구현하고 스레드가 동시에 위 Cas_Lock 클래스를 사요하여 동기화 처리를 한 코드이다. 위 코드를 실행하면 1초마다 각 스레드가 순차적으로 lock 상태에 따라 Worker() 메서드의 내부 코드를 처리 하게 된다.


이처럼 스레드가 Lock 처리 되어 있는 구간에 진입 시 lock 대기열로 진입하지 않고 계속해서 lock이 해제되었는지 무한 체크하는 방식으로 동기화 처리가 진행된다. 하지만 이런 방법은 보호하려는 임계구간이 짧으면서도 한 스레드가 오랫동안 머물지 않는 처리에서 효과 적으로 사용될 수 있다.
