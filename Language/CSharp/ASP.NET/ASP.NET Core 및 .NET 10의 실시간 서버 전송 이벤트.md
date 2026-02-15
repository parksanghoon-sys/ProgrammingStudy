# 실시간 서버 전송 이벤트

[https://medium.com/codex/real-time-server-sent-events-in-asp-net-core-and-net-10-edb8845b2f1c]()

백엔드에서 프로트로 실시간 업데이트를 통합을 해야할때 이를 구현하는 옵션에는 몇가지가 있다

* 폴링
  * 서버에 새로운 데이터가 있는지 지속적으로 확인
* SignalR
  * 프론트엔드는 이벤트를 구독하고 서버는 WebSocket을 사용하여 이벤트를 보낸다
* Server-Sent Event

몇 초마다 엔드포인트를 폴링 시 서버에 과부하가 걸리고 대역폭이 낭비될 수있다, 전이중 WebSoket은 간단한 단방향 업데이트에는 과도할 수 있다

SSE(Server-Sent Events)는 Asp.Core 앱이 양방향의 프로토콜 복잡성  없이 지속적인 데이터 스트림을 푸시할 수 있는 가볍고 안정적인 방법을 제공한다

## 서버에서 보낸 이벤트란

SSE는 서버가 단일 HTTP 연결을 통해 실시간 데이터를 웹클라이언트에 푸시할 수 있도록하는 웹표준이다. 클라이언트가 업데이트를 위해 서버를 반복적으로 폴링하는 기존 요청 - 응답 패턴과 달리 SSE를 사용하면 서버가 통신을 시작하고 정보를 사용할 수있을때마다 데이터를 보낼 수 있다

### SSE의 주요 특징

* 단방향 통신
  * 데이터는 서버에서 클라이언트로만 흐른다
* HTTP/1.1 기반
  * SSE는 MIME 유형을 사용하여 일반 HTTP를 통해 작동한다, 특별한 WebSocket 핸드쉐이크가 필요하지 않는다
* 내장 재 연결
  * 연결이 끊어지면 브라우저가 자동으로 다시 연결된다.
* 경량
  * 다른 솔루션에 비해 오버헤드 최소화

SSE는 일반 HTTP를 통해 작동하기에 모든 브라우저가 SSE를 지원한다, IDE에서 HTTP 요청파일과 crul. Postman, Apidog와 같은 도구를 사용해 SSE를 테스트 할수있다.

SSE는 서버에서 클라이언트로 업데이트를 푸시해야하지만 양방향 통신이 필요하지 않은 경우 적합하다, WebSocket 보다 구현은 간단하며 기존 HTTP인프라와 원활하게 작동한다.

### Asp.NET Core에서 SSE구현

서비전송 이벤트에대한 지원을 .NET 10 부터 가능한다, 내부적으로 Content-Type을 설정하고 플러싱을 처리하고 취소와 통신한다.

주가 업데이트의 비동기스트림을 생성한느 StockService를 예시로 본다

```csharp
public record StockPriceEvent(string Id, string Symbol, decimal Price, DateTime Timestamp);

public class StockService
{
    public async IAsyncEnumerable<StockPriceEvent> GenerateStockPrices(
       [EnumeratorCancellation] CancellationToken cancellationToken)
    {
       var symbols = new[] { "MSFT", "AAPL", "GOOG", "AMZN" };

       while (!cancellationToken.IsCancellationRequested)
       {
          // Pick a random symbol and price
          var symbol = symbols[Random.Shared.Next(symbols.Length)];
          var price  = Math.Round((decimal)(100 + Random.Shared.NextDouble() * 50), 2);

          var id = DateTime.UtcNow.ToString("o");

          yield return new StockPriceEvent(id, symbol, price, DateTime.UtcNow);

          // Wait 2 seconds before sending the next update
          await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
       }
    }
}
```

이 메시드는 고정된 간격으로 *StockPriceEvnet* 스트림을 생성한다

주가 업데이트 SSE를 보내는 최소 API 엔드포인트를 만들어본다면

```csharp
builder.Services.AddSingleton<StockService>();

app.MapGet("/stocks", (StockService stockService, CancellationToken ct) =>
{
    return TypedResults.ServerSentEvents(
       stockService.GenerateStockPrices(ct),
       eventType: "stockUpdate"
    );
});
```

### 재연결 논리 및 Last-Event-ID 헤더

SSE의 가장강력한 기능중 하나는 자동 재연결이다, 연결이 끊어지면 브라우져는 자동으로 재연결을 시도하고 Last Event ID 헤더를 사용하여 중단한 부분부터 다시 시작할 수있다. 

연결이 끊어지면 브라우저는 스트림을 열고 다음을 포함한다

```csharp
Last-Event-ID : 20250616T150430Z
```

백엔드에서 다시 시작할 위치를 결정하기윈해 검사 할 수 있다, 이전 항목을 건너뛰거나 , 누락한 항목을 재생하거나, 재연결 이벤트르 기록 할 수 있다.

예시는 아래와 같다

```csharp
app.MapGet("/stocks2", (
    StockService stockService,
    HttpRequest httpRequest,
    CancellationToken ct) =>
{
    // 1. Read Last-Event-ID (if any)
    var lastEventId = httpRequest.Headers.TryGetValue("Last-Event-ID", out var id)
       ? id.ToString()
       : null;

    // 2. Optionally log or handle resume logic
    if (!string.IsNullOrEmpty(lastEventId))
    {
       app.Logger.LogInformation("Reconnected, client last saw ID {LastId}", lastEventId);
    }

    // 3. Stream SSE with lastEventId and retry
    var stream = stockService.GenerateStockPricesSince(lastEventId, ct)
       .Select(evt =>
       {
          var sseItem = new SseItem<StockPriceEvent>(evt, "stockUpdate")
          {
             EventId = evt.Id
          };

          return sseItem;
       });

    return TypedResults.ServerSentEvents(
       stream,
       eventType: "stockUpdate"
    );
});
```

### HTTP 파일을 이용하여 SSE 엔드포인트 테스트

모든 IDE는 API 엔드포인트 테스트하는데 사용할 수 있는 HTTP 요청 파일을 지원한다, 또한 서버전송 이벤트를 지원한다

```csharp
@ServerSentEvents_HostAddress = http://localhost:5000

### Test SSE stream from .NET 10 Minimal API
GET {{ServerSentEvents_HostAddress}}/stocks
Accept: text/event-stream
```

예시 응답

```csharp
Response code: 200 (OK); Time: 410ms (410 ms)

event: stockUpdate
data: {"id":"2025-06-16T05:31:10.5426180Z","symbol":"AMZN","price":122.67,"timestamp":"2025-06-16T05:31:10.5445659Z"}

event: stockUpdate
data: {"id":"2025-06-16T05:31:12.5838704Z","symbol":"AAPL","price":118.88,"timestamp":"2025-06-16T05:31:12.5838771Z"}

event: stockUpdate
data: {"id":"2025-06-16T05:31:14.5937683Z","symbol":"AAPL","price":104.01,"timestamp":"2025-06-16T05:31:14.593772Z"}
```
