# 개요 — 옵션 패턴(Options pattern) 한눈에

ASP.NET Core에서 `IOptions<T>`, `IOptionsSnapshot<T>`, `IOptionsMonitor<T>`는 설정 바인딩(예: `appsettings.json`)으로 만든 **설정 POCO**를 DI로 안전하게 주입하는 표준 방법입니다. 목적은 설정을 POCO로 바인딩하고 타입 안전성, 검증, 구성 변경 처리 등을 제공하는 것.

아래에 차이점, 사용 시점, 예제(최신 .NET 스타일), 고급 팁까지 정리합니다.

---

# 핵심 차이 (요약)

* `IOptions<T>`

  * **싱글톤 읽기 전용**: 앱 시작 시(또는 DI가 구성될 때) 한 번 계산된 값을 제공.
  * 주로 **싱글톤 서비스에서 읽기 전용 초기 설정**이 필요할 때 사용.
  * 설정 변경 자동 반영 없음.

* `IOptionsSnapshot<T>`

  * **스코프(요청) 기준 스냅샷**: 각 DI 스코프(예: HTTP 요청)마다 최신 구성으로 새 인스턴스를 만듦.
  * **Scoped/Transient/Controller** 등에서 사용하면, 같은 요청 내에서는 동일한 인스턴스. 다음 요청부터는 변경 반영 가능.
  * 싱글톤 서비스에는 주입 불가(스코프가 없으므로).

* `IOptionsMonitor<T>`

  * **실시간(애플리케이션 전체) 모니터링**: 설정이 바뀌면 즉시 반영.
  * `OnChange` 콜백으로 변경 알림 가능.
  * 싱글톤에 안전하게 주입 가능(모니터 자체는 싱글톤).
  * 내부적으로는 이름 있는 옵션(named options) 캐시도 관리.

---

# 언제 무엇을 쓸까 (실무 가이드)

* 앱 전체에서 변경 없이 단 한 번 읽으면 충분하면 → `IOptions<T>`.
* HTTP 요청 단위로 최신 값을 반영하고 싶으면(예: 사용자가 요청별로 바뀐 설정을 기대) → `IOptionsSnapshot<T>` (Controllers / scoped services).
* 설정 파일이 바뀔 때 실시간으로 반영해서 동작해야 하고, 변경 시 콜백 처리도 필요하면 → `IOptionsMonitor<T>` (싱글톤 서비스 포함 가능).
* 실시간 구독(예: 캐시 TTL, 로깅 레벨 동적 변경 등)에선 `IOptionsMonitor<T>.OnChange`가 매우 유용.

---

# 예제 (최신 .NET / minimal API 스타일)

`appsettings.json`

```json
{
  "MyOptions": {
    "Host": "example.com",
    "Port": 1234,
    "EnableFeatureX": true
  }
}
```

옵션 POCO:

```csharp
public class MyOptions
{
    public string Host { get; set; } = "";
    public int Port { get; set; }
    public bool EnableFeatureX { get; set; }
}
```

`Program.cs` (.NET 6/7/8 스타일):

```csharp
var builder = WebApplication.CreateBuilder(args);

// bind and validate
builder.Services.AddOptions<MyOptions>()
    .Bind(builder.Configuration.GetSection("MyOptions"))
    .ValidateDataAnnotations() // if you use [Required], [Range], etc.
    .Validate(o => !string.IsNullOrEmpty(o.Host), "Host required")
    .ValidateOnStart(); // throw on startup if invalid

var app = builder.Build();

app.MapGet("/", (IOptions<MyOptions> optsStatic, 
                 IOptionsSnapshot<MyOptions> optsSnapshot,
                 IOptionsMonitor<MyOptions> optsMonitor) =>
{
    // IOptions: app start 시점 값
    var v1 = optsStatic.Value;

    // IOptionsSnapshot: 요청(스코프) 기준 최신값
    var v2 = optsSnapshot.Value;

    // IOptionsMonitor: 항상 최신값 (동적 반영) 및 OnChange 사용 가능
    var v3 = optsMonitor.CurrentValue;

    return Results.Ok(new {
        staticValue = v1,
        snapshotValue = v2,
        monitorValue = v3
    });
});

app.Run();
```

`Configuration`에서 파일 변경을 자동 반영하려면 (파일 소스의 reloadOnChange 활성화 — 기본 `CreateDefaultBuilder`에서 활성화됨):

```csharp
// 보통 CreateDefaultBuilder 가 이미 reloadOnChange=true로 구성함
var builder = WebApplication.CreateBuilder(new WebApplicationOptions { /*...*/ });
// 또는 직접:
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
```

`IOptionsMonitor` 콜백 사용:

```csharp
services.AddSingleton<MyService>();

public class MyService
{
    private readonly IDisposable _subscription;

    public MyService(IOptionsMonitor<MyOptions> monitor)
    {
        // 초기값
        var current = monitor.CurrentValue;

        // 변경시 콜백
        _subscription = monitor.OnChange(newVal =>
        {
            // 동적 업데이트 처리 (스레드 안전 고려)
            // 예: 내부 캐시 갱신, 로그레벨 변경 등
            Console.WriteLine("MyOptions changed: " + newVal.Host);
        });
    }
}
```

---

# Named options (이름 붙인 옵션) 예시

동일 타입의 옵션을 여러 설정 블록으로 관리할 때 유용:

```csharp
services.Configure<MyOptions>("A", configuration.GetSection("A"));
services.Configure<MyOptions>("B", configuration.GetSection("B"));

// 주입
IOptionsMonitor<MyOptions> monitor;
// 사용: monitor.Get("A"), monitor.Get("B")
```

`IOptionsSnapshot`도 `Get(name)` 지원.

---

# 내부 동작 & 구현 포인트 (좀 더 깊게)

* `IOptions<T>.Value`

  * 내부적으로 `IOptionsFactory<T>`로부터 생성된 인스턴스를 반환. 기본은 **싱글톤 캐시**(애플리케이션 전체에 하나).

* `IOptionsSnapshot<T>`

  * `IOptionsSnapshot`는 **스코프가 생성될 때**(예: 요청 시작) 캐시를 초기화하고 그 스코프 동안 같은 값을 반환. 다음 스코프에서 다시 새로 계산(또는 최신값으로 갱신).
  * 그래서 **Controller(Scoped)** 에 주입하면 요청마다 최신 값을 보장.

* `IOptionsMonitor<T>`

  * 내부에 `IOptionsMonitorCache`를 가지고 있고, 구성 변경시 캐시를 무효화하고 관련 `OnChange` 구독자에게 알림.
  * 싱글톤 컨텍스트에서 안전하게 사용 가능.

---

# 실무 팁 / 주의사항

1. **싱글톤 서비스에 IOptionsSnapshot 주입 금지**

   * 스코프를 필요로 하기 때문에 예외가 발생하거나 미작동. 싱글톤에서는 `IOptionsMonitor`를 사용하라.

2. **스레드 안전성**

   * `IOptions<T>.Value`가 반환하는 객체를 직접 변경하지 말자. 값은 불변(또는 불변처럼 다루는 게 안전). `OnChange`에서 상태를 바꿀 때는 동기화 필요.

3. **성능 고려**

   * `IOptionsSnapshot`는 스코프마다 새 인스턴스를 만들므로, 아주 빈번한 스코프 생성/파괴 환경에서는 비용이 있다. 보통 HTTP 요청 단위라면 문제 없음.
   * `IOptionsMonitor`는 변경이 드문 설정엔 비용이 작음. OnChange 콜백 등록도 가볍다.

4. **옵션 검증**

   * `AddOptions<T>().Bind(...).Validate...` 사용. `ValidateOnStart()`는 앱 시작 시 검증 실패시 즉시 예외 발생(프로덕션에선 유용).

5. **환경 별 설정 및 우선순위**

   * 구성 소스(환경 변수, command-line, JSON 등)의 우선순위는 `Configuration` 규칙에 따름. `IOptions` 계층은 `IConfiguration`이 제공하는 값을 그대로 바인딩.

6. **Mutable한 POCO 주의**

   * 바인딩된 POCO가 mutable일 수 있음. 외부에서 수정하면 다른 코드가 의도치 않은 변경을 보게 됨. 읽기 전용으로 다루거나 복사본을 만들어 사용하자.

---

# 자주 묻는 질문 (FAQ)

Q. `IOptionsSnapshot`는 언제 값이 갱신되나요?
A. 새 DI 스코프가 생성될 때(HTTP 요청 시작 시) `IOptionsSnapshot`는 최신 구성으로 새 인스턴스를 만들어 제공합니다. 요청 중간에 파일이 바뀌어도 그 요청의 `IOptionsSnapshot` 값은 변하지 않음(다음 요청부터 반영).

Q. 파일 변경을 감지하려면 무엇이 필요한가?
A. `Configuration` 소스가 `reloadOnChange: true`여야 하며, `IOptionsMonitor` 또는 `IOptionsSnapshot`를 통해 새 값으로 바인딩된다. `IOptionsMonitor`는 즉시 변경 알림을 제공.

Q. Controller에서 그냥 생성자에 `MyOptions`를 넣을 수는?
A. DI가 `IOptions<T>` 인스턴스를 주입하도록 `IOptions<MyOptions>` 또는 `IOptionsSnapshot<MyOptions>` 등을 받아야 한다. 하지만 직접 `MyOptions` 타입을 DI에 등록해도 가능하긴 하지만(권장 X), 옵션 패턴의 장점(Validate, named options, OnChange 등)을 잃는다.

---

# 요약(한 문장)

* `IOptions<T>`: 시작 시 고정된 값(싱글톤 스타일).
* `IOptionsSnapshot<T>`: 요청 스코프별 최신 스냅샷(Controller/Scoped에서 유용).
* `IOptionsMonitor<T>`: 앱 전역에서 실시간 변경 감지 + OnChange 콜백(싱글톤에 안전).

---