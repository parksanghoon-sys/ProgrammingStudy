
(최신 .NET 기준, `Span / Memory / SocketAsyncEventArgs` 사용)

아래 예제는 **길이 프레임 기반 프로토콜**이고,
👉 **직렬화 시 새 배열 생성 X**
👉 **역직렬화 시 객체 생성 X (View 패턴)**
👉 **수신 버퍼 재사용**

---

# 1️⃣ 프로토콜 정의

```
[0~3]   int32  TotalLength (Header + Payload)
[4~7]   int32  MessageId
[8~ ]   Payload (binary)
```

---

# 2️⃣ Packet View (Zero-Copy 역직렬화)

```csharp
using System.Buffers.Binary;

public readonly ref struct PacketView
{
    private readonly ReadOnlySpan<byte> _buffer;

    public PacketView(ReadOnlySpan<byte> buffer)
    {
        _buffer = buffer;
    }

    public int TotalLength =>
        BinaryPrimitives.ReadInt32LittleEndian(_buffer.Slice(0, 4));

    public int MessageId =>
        BinaryPrimitives.ReadInt32LittleEndian(_buffer.Slice(4, 4));

    public ReadOnlySpan<byte> Payload =>
        _buffer.Slice(8, TotalLength - 8);
}
```

✔ 객체 생성 없음
✔ 복사 없음

---

# 3️⃣ Send (Zero-Copy 직렬화)

```csharp
public static int WritePacket(
    Span<byte> buffer,
    int messageId,
    ReadOnlySpan<byte> payload)
{
    int totalLength = 8 + payload.Length;

    BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(0, 4), totalLength);
    BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(4, 4), messageId);

    payload.CopyTo(buffer.Slice(8));

    return totalLength;
}
```

---

## 실제 송신 코드

```csharp
async Task SendAsync(Socket socket, ReadOnlySpan<byte> payload)
{
    byte[] sendBuffer = ArrayPool<byte>.Shared.Rent(1024);

    try
    {
        int len = WritePacket(sendBuffer, 1001, payload);
        await socket.SendAsync(sendBuffer.AsMemory(0, len), SocketFlags.None);
    }
    finally
    {
        ArrayPool<byte>.Shared.Return(sendBuffer);
    }
}
```

✔ payload → 복사 1회 (불가피)
✔ packet buffer 재사용

---

# 4️⃣ Receive (TCP 스트림 처리 핵심)

TCP는 **메시지 경계가 없음** → 누적 버퍼 필요

---

## 4-1️⃣ 수신 버퍼 관리 클래스

```csharp
public sealed class ReceiveBuffer
{
    private byte[] _buffer;
    private int _writePos;

    public ReceiveBuffer(int size = 8192)
    {
        _buffer = new byte[size];
    }

    public Memory<byte> Writable =>
        _buffer.AsMemory(_writePos);

    public void Advance(int bytes) =>
        _writePos += bytes;

    public ReadOnlySpan<byte> Readable =>
        _buffer.AsSpan(0, _writePos);

    public void Consume(int bytes)
    {
        Buffer.BlockCopy(_buffer, bytes, _buffer, 0, _writePos - bytes);
        _writePos -= bytes;
    }
}
```

---

## 4-2️⃣ Receive Loop (Zero-Copy 파싱)

```csharp
async Task ReceiveLoopAsync(Socket socket)
{
    var recvBuffer = new ReceiveBuffer();

    while (true)
    {
        int received = await socket.ReceiveAsync(
            recvBuffer.Writable,
            SocketFlags.None);

        if (received == 0)
            break;

        recvBuffer.Advance(received);

        ProcessPackets(recvBuffer);
    }
}
```

---

## 4-3️⃣ 패킷 파싱 (핵심)

```csharp
void ProcessPackets(ReceiveBuffer buffer)
{
    while (true)
    {
        var span = buffer.Readable;

        if (span.Length < 4)
            return;

        int totalLength =
            BinaryPrimitives.ReadInt32LittleEndian(span.Slice(0, 4));

        if (span.Length < totalLength)
            return;

        var packetSpan = span.Slice(0, totalLength);
        var packet = new PacketView(packetSpan);

        HandlePacket(packet);

        buffer.Consume(totalLength);
    }
}
```

---

## 4-4️⃣ 패킷 처리

```csharp
void HandlePacket(PacketView packet)
{
    Console.WriteLine($"MsgId={packet.MessageId}, PayloadLen={packet.Payload.Length}");

    // Payload는 그대로 Span
    // 필요하면 여기서만 복사
}
```

✔ recv → parse → handle **전 과정 노카피**
✔ View는 stack-only (`ref struct`)
✔ GC pressure 최소화

---

# 5️⃣ SocketAsyncEventArgs 버전 (실전 서버용)

> 초당 수만 연결이면 이 방식 추천

```csharp
var args = new SocketAsyncEventArgs();
args.SetBuffer(ArrayPool<byte>.Shared.Rent(8192), 0, 8192);
args.Completed += OnReceive;

socket.ReceiveAsync(args);
```

이 구조도 내부에서 **같은 ReceiveBuffer + View** 패턴을 쓰면 된다.

---

# 6️⃣ 실전에서 이 패턴을 쓰는 경우

* 게임 서버
* HL7 MLLP 인터페이스
* 고속 장비 제어
* 금융 시세 수신
* 커스텀 바이너리 프로토콜

---

## 🔑 핵심 정리

> **TCP Zero-Copy의 본질은**
> ✔ 메시지 객체를 만들지 않는 것
> ✔ byte[]를 재사용하는 것
> ✔ Span/View로 읽는 것

원하면 다음도 바로 이어서 보여줄 수 있어:

* TCP에서 **partial recv / fragmentation** 완전 대응
* 멀티 패킷/멀티 스레드 안전 설계
* `PipeReader` 기반 구현
* UDP 버전 비교

어디까지 필요해?

---

# 전체 로드맵

1. TCP 스트림 특성 & 문제점
2. Zero-Copy 설계 원칙
3. 프로토콜 정의
4. PacketView (완전 노카피)
5. 송신 (Send)
6. 수신 (ReceiveBuffer)
7. Fragmentation / Sticky Packet 처리
8. 멀티 패킷 처리
9. `SocketAsyncEventArgs` 서버 구조
10. `PipeReader` 기반 대안
11. 스레드 안전 설계
12. 실전 체크리스트

---

## 1️⃣ TCP 스트림의 현실

TCP는 절대 이렇게 안 온다 ❌

```
[패킷1][패킷2]
```

실제로는 이렇게 온다 ✅

```
[패킷1 절반]
[패킷1 나머지 + 패킷2 일부]
[패킷2 나머지]
```

그래서 반드시 필요:

* 누적 버퍼
* 길이 프레임
* 파싱 루프

---

## 2️⃣ Zero-Copy 설계 원칙 (핵심)

| 항목     | 원칙             |
| -------- | ---------------- |
| 객체     | 만들지 않는다    |
| 역직렬화 | View(ref struct) |
| 버퍼     | 재사용           |
| 접근     | Span / Memory    |
| 복사     | 최후의 순간에만  |

---

## 3️⃣ 프로토콜 정의 (Length-Prefix)

```
[0~3]   int32 TotalLength
[4~7]   int32 MessageId
[8~ ]   Payload
```

---

## 4️⃣ PacketView (역직렬화 핵심)

```csharp
using System.Buffers.Binary;

public readonly ref struct PacketView
{
    private readonly ReadOnlySpan<byte> _buffer;

    public PacketView(ReadOnlySpan<byte> buffer)
        => _buffer = buffer;

    public int TotalLength =>
        BinaryPrimitives.ReadInt32LittleEndian(_buffer[..4]);

    public int MessageId =>
        BinaryPrimitives.ReadInt32LittleEndian(_buffer.Slice(4, 4));

    public ReadOnlySpan<byte> Payload =>
        _buffer.Slice(8, TotalLength - 8);
}
```

✔ 힙 할당 0
✔ GC 영향 0
✔ 네이티브 수준 접근

---

## 5️⃣ 송신 (Zero-Copy 직렬화)

```csharp
public static int WritePacket(
    Span<byte> buffer,
    int messageId,
    ReadOnlySpan<byte> payload)
{
    int totalLength = 8 + payload.Length;

    BinaryPrimitives.WriteInt32LittleEndian(buffer[..4], totalLength);
    BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(4, 4), messageId);

    payload.CopyTo(buffer.Slice(8));
    return totalLength;
}
```

---

### 실제 Send

```csharp
async Task SendAsync(Socket socket, ReadOnlySpan<byte> payload)
{
    byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);

    try
    {
        int len = WritePacket(buffer, 1001, payload);
        await socket.SendAsync(buffer.AsMemory(0, len), SocketFlags.None);
    }
    finally
    {
        ArrayPool<byte>.Shared.Return(buffer);
    }
}
```

---

## 6️⃣ 수신 버퍼 (누적 구조)

```csharp
public sealed class ReceiveBuffer
{
    private readonly byte[] _buffer;
    private int _writePos;

    public ReceiveBuffer(int size = 8192)
        => _buffer = new byte[size];

    public Memory<byte> Writable =>
        _buffer.AsMemory(_writePos);

    public void Advance(int bytes) =>
        _writePos += bytes;

    public ReadOnlySpan<byte> Readable =>
        _buffer.AsSpan(0, _writePos);

    public void Consume(int bytes)
    {
        Buffer.BlockCopy(_buffer, bytes, _buffer, 0, _writePos - bytes);
        _writePos -= bytes;
    }
}
```

---

## 7️⃣ Receive Loop (Fragmentation 대응)

```csharp
async Task ReceiveLoopAsync(Socket socket)
{
    var buffer = new ReceiveBuffer();

    while (true)
    {
        int received = await socket.ReceiveAsync(
            buffer.Writable, SocketFlags.None);

        if (received == 0)
            break;

        buffer.Advance(received);
        ProcessPackets(buffer);
    }
}
```

---

## 8️⃣ 멀티 패킷 파싱 (Sticky Packet 처리)

```csharp
void ProcessPackets(ReceiveBuffer buffer)
{
    while (true)
    {
        var span = buffer.Readable;

        if (span.Length < 4)
            return;

        int totalLength =
            BinaryPrimitives.ReadInt32LittleEndian(span[..4]);

        if (span.Length < totalLength)
            return;

        var packetSpan = span[..totalLength];
        var packet = new PacketView(packetSpan);

        HandlePacket(packet);
        buffer.Consume(totalLength);
    }
}
```

✔ partial recv OK
✔ 여러 패킷 OK
✔ 순수 Span 파싱

---

## 9️⃣ 패킷 처리

```csharp
void HandlePacket(PacketView packet)
{
    switch (packet.MessageId)
    {
        case 1001:
            HandleLogin(packet.Payload);
            break;
    }
}
```

---

## 🔟 SocketAsyncEventArgs 기반 서버

```csharp
void StartReceive(Socket socket)
{
    var args = new SocketAsyncEventArgs();
    args.SetBuffer(ArrayPool<byte>.Shared.Rent(8192), 0, 8192);
    args.UserToken = new ReceiveBuffer();
    args.Completed += OnReceive;

    socket.ReceiveAsync(args);
}

void OnReceive(object? sender, SocketAsyncEventArgs e)
{
    if (e.BytesTransferred == 0)
        return;

    var buffer = (ReceiveBuffer)e.UserToken!;
    buffer.Advance(e.BytesTransferred);

    ProcessPackets(buffer);

    e.SetBuffer(e.Offset + e.BytesTransferred, e.Buffer!.Length - e.BytesTransferred);
    ((Socket)sender!).ReceiveAsync(e);
}
```

✔ 고성능 서버 구조
✔ 연결당 버퍼 1개

---

## 1️⃣1️⃣ PipeReader 기반 (대안)

```csharp
var pipe = PipeReader.Create(networkStream);

while (true)
{
    var result = await pipe.ReadAsync();
    var buffer = result.Buffer;

    while (TryReadPacket(ref buffer, out var packet))
    {
        HandlePacket(packet);
    }

    pipe.AdvanceTo(buffer.Start, buffer.End);
}
```

✔ fragmentation 자동 처리
❌ Span 제어는 Socket 방식보다 제한적

---

## 1️⃣2️⃣ 스레드 안전 설계

* Receive → **단일 스레드**
* Handle → Worker Queue
* PacketView → 절대 저장 ❌ (stack only)

```csharp
_channel.Writer.TryWrite(packet.Payload.ToArray()); // 여기서만 복사
```
