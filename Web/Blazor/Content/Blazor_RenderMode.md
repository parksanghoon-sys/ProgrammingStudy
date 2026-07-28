# Blazor 8 Render Mode


| RenderMode                | Description                                                                                                                                                      |
| ------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Server-static             | Blazor 페이지는 서버에서 HTML/CSS 로 렌더링된 다음 보여진다                                                                                                      |
| Server-static (streaming) | 서버가 브라우저에 콘텐츠를 보내고 완료 될 때 시간이 지남에 따라 더 많은 콘텐츠를 보내는 점을 제이하면 server-static과 같다                                       |
| Server-interactive        | 브라우저와 서버간 SignalR 연결이 생성되고 사용자가 대화형 환경을 얻는다. 기본적을 페이지는 정적으로 렌더링 된 다음 서버 대화형으로 전환                          |
| WebAssembly-interactive   | Blzor 코드가 WebAssembly를 사용하여 클라이언트의 브라우져에서 완전히 실행된다, 기본적으로 먼저 Server-static으로 렌더링된 다음 전환                              |
| InteractiveAuto           | 먼저 서버 정적 페이지로 렌더링되고, Wasm 코드가 백그라운드에서 다운되는 동안 서버 대화형 페이지로 전환된다. 이후 페이지 방문은 웹어셈블리 대화식으로 이루어 진다 |

### 구성요소 수명주기

1. 모든 페이지가 초기화되면서 `OnInitialized` 되거나 `OnInitializedAsync` 알림을 받을 수 있다.
2. 대화형 페이지는 `OnAfterRender` 또는 `OnAfterRenderAsync` 메소드를 호출한다
   1. `OnAfterRender` 로 페이지가 초기화 되면 서버 정적으로 렌더링 된다

### 코드 기반 솔루션

RenderModes.Clien 측에서 체크 방법

#### ActiveCircuitState 

```csharp
public class ActiveCircuitState
{
    public bool CircuitExists { get; set; }
}
```

* Signal R 회로가 존재하는지 여부를 나타내는 값을 나타내는 클래스이다


#### ActiveCircuitHandler 클래스

```csharp
    public class ActiveCircuitHandler(ActiveCircuitState state) : CircuitHandler
    {
        public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            state.CircuitExists = true;
            return base.OnCircuitOpenedAsync(circuit, cancellationToken);
        }

        public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            state.CircuitExists = false;
            return base.OnCircuitClosedAsync(circuit, cancellationToken);
        }
    }
```

* 서버 측 으로 RenderModes 가 존재하는지 여부를 감시한다
* DI 서비스에 등로갛여 체크한다

```csharp
builder.Services.AddScoped<ActiveCircuitState>();
builder.Services.AddScoped(typeof(CircuitHandler), typeof(ActiveCircuitHandler));
```

#### 페이지에서 StreamRendering 특성 감지

서버 페이지는 StreamRendering Blazor 에게 HTML/CSS 출력을 브라우저로 스트리밍하도록 지시하는속석으로 표시될 수 있다.

```csharp
page.GetType().GetCustomAttributes(
  typeof(StreamRenderingAttribute), true).Length > 0
```

#### Client 에서 확인하기

```csharp
    public class RenderModeProvider(ActiveCircuitState activeCircuitState)
    {
        public string GetRenderMode(ComponentBase page)
        {
            string result;
            var isBrowser = OperatingSystem.IsBrowser();
            if (isBrowser)
                result = "wasm-interactive";
            else if (activeCircuitState.CircuitExists)
                result = "server-interactive";
            else if (page.GetType().GetCustomAttributes(typeof(StreamRenderingAttribute), true).Length > 0)
                result = "server-static (streaming)";
            else
                result = "server-static";
            return result;
        }
    }
```

* 이클래스를 DI 사용하여 현재 사용작에 대한권한 엑세스를 얻고 렌더링 모드를 반환하는 메서드이다
* **builder**.**Services**.**AddTransient**<**RenderModes**.**Client**.**RenderModeProvider**>();
*
