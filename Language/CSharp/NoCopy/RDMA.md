좋다. 이건 **RDMA를 “개념”이 아니라 “시스템”으로 쓰는 단계**다.
아래는 **실무에서 그대로 써먹는 수준**으로 정리한다.

> 목표
> 1️⃣ **서버 측 Receive + MR 공유 예제**
> 2️⃣ **RDMA Ring Buffer 실전 프로토콜**
> 3️⃣ **C# ↔ RDMA 연동 구조 (현실적인 분리)**

---

# 0️⃣ 전체 그림 (이해 먼저)

```
┌──────────────┐        TCP (Control Plane)
│   C# Client  │ ──────────────────────────────┐
│  Logic only  │                                │
└──────┬───────┘                                ▼
       │ P/Invoke                        ┌───────────────┐
       ▼                                 │   C++ RDMA     │
┌──────────────┐                         │   Wrapper     │
│  C++ Wrapper │                         │ (ND2)         │
│ (Client)     │                         └──────┬────────┘
└──────┬───────┘                                │
       │ RDMA Write                             │
       └──────────────────────────────────────►│
                                                ▼
                                      ┌──────────────────┐
                                      │ Server RDMA MR   │
                                      │ Ring Buffer      │
                                      └──────────────────┘
```

* **TCP** : 제어, 주소·토큰 교환
* **RDMA** : 데이터만
* **C#** : 절대 RDMA 직접 안 만짐

---

# 1️⃣ 서버 측 Receive + MR 공유 예제

## 1-1️⃣ 서버의 역할

서버는:

* **큰 버퍼 1개를 고정(MR)**
* 그 주소와 token(rkey)을 **TCP로 클라이언트에게 전달**
* 이후 **아무 것도 안 함**
  * 클라이언트가 RDMA Write로 메모리 직접 씀

---

## 1-2️⃣ 서버: MR 생성

```cpp
struct SharedMemory
{
    void* addr;
    size_t size;
    IND2MemoryRegion* mr;
};

SharedMemory g_shared;

bool server_create_shared_memory(size_t size)
{
    g_shared.addr = VirtualAlloc(
        nullptr,
        size,
        MEM_COMMIT | MEM_RESERVE,
        PAGE_READWRITE
    );

    HRESULT hr = g_adapter->RegisterMemory(
        g_shared.addr,
        size,
        ND_MR_FLAG_ALLOW_LOCAL_WRITE |
        ND_MR_FLAG_ALLOW_REMOTE_WRITE,
        &g_shared.mr
    );

    return SUCCEEDED(hr);
}
```

---

## 1-3️⃣ TCP로 공유 정보 전달

```cpp
struct MrInfo
{
    uint64_t address;
    uint32_t token;
    uint32_t size;
};
```

```cpp
MrInfo info;
info.address = (uint64_t)g_shared.addr;
info.token   = g_shared.mr->GetToken();
info.size    = (uint32_t)g_shared.size;

// TCP send
send(sock, (char*)&info, sizeof(info), 0);
```

👉 이 순간부터
**클라이언트는 서버 메모리를 직접 쓸 수 있음**

---

## 1-4️⃣ 서버는 데이터 확인만 함

```cpp
void server_poll()
{
    char* p = (char*)g_shared.addr;

    if (p[0] != 0)
    {
        printf("Received: %s\n", p);
        p[0] = 0; // 소비 표시
    }
}
```

> 서버 CPU는 **네트워크 경로에 관여 안 함**

---

# 2️⃣ RDMA Ring Buffer 실전 프로토콜 (🔥 핵심)

TCP처럼 쓰면 **100% 실패**한다.
RDMA는 반드시 **lock-free 프로토콜**이어야 한다.

---

## 2-1️⃣ 메모리 레이아웃 (서버 MR)

```
| WriteIndex | ReadIndex | Data[ ... ] |
|  8 bytes   |  8 bytes  | Ring Buffer |
```

```cpp
struct RingBufferHeader
{
    volatile uint64_t writeIndex;
    volatile uint64_t readIndex;
};
```

---

## 2-2️⃣ 서버 초기화

```cpp
auto* header = (RingBufferHeader*)g_shared.addr;
header->writeIndex = 0;
header->readIndex  = 0;
```

---

## 2-3️⃣ 클라이언트 Write 알고리즘

### Step 1️⃣ Read writeIndex (RDMA Read)

```cpp
uint64_t writeIndex;
rdma_read(
    &writeIndex,
    remoteAddr + offsetof(RingBufferHeader, writeIndex),
    remoteToken
);
```

---

### Step 2️⃣ 데이터 쓰기

```cpp
uint64_t offset =
    sizeof(RingBufferHeader) +
    (writeIndex % DATA_SIZE);

rdma_write(
    data,
    length,
    remoteAddr + offset,
    remoteToken
);
```

---

### Step 3️⃣ writeIndex 증가

```cpp
uint64_t newIndex = writeIndex + length;

rdma_write(
    &newIndex,
    sizeof(uint64_t),
    remoteAddr + offsetof(RingBufferHeader, writeIndex),
    remoteToken
);
```

👉 **atomic 없음**
👉 **순서 보장 = NIC**

---

## 2-4️⃣ 서버 Read 알고리즘

```cpp
void server_consume()
{
    auto* header = (RingBufferHeader*)g_shared.addr;
    auto* data   = (char*)(header + 1);

    while (header->readIndex < header->writeIndex)
    {
        char* msg = data + (header->readIndex % DATA_SIZE);

        process(msg);

        header->readIndex += messageLength;
    }
}
```

---

## 2-5️⃣ 이 방식의 특징

| 항목           | 결과     |
| -------------- | -------- |
| Lock           | ❌       |
| Syscall        | ❌       |
| Context switch | ❌       |
| Copy           | ❌       |
| Latency        | NIC 수준 |

---

# 3️⃣ C# ↔ RDMA 연동 구조 (현실적인 답)

## 3-1️⃣ 절대 하면 안 되는 것

* C#에서 RDMA API 호출
* byte[]를 RDMA로 보내기
* unsafe Span 남발

---

## 3-2️⃣ C#의 책임

* 메시지 정의
* 직렬화
* 흐름 제어
* 비즈니스 로직

---

## 3-3️⃣ C++ Wrapper 공개 API

```cpp
extern "C"
{
    void* RdmaAlloc(size_t size);
    bool  RdmaWrite(
        void* localBuffer,
        size_t length,
        uint64_t remoteAddr,
        uint32_t remoteKey
    );
}
```

---

## 3-4️⃣ C# P/Invoke

```csharp
internal static class RdmaNative
{
    [DllImport("rdma_win.dll")]
    public static extern IntPtr RdmaAlloc(ulong size);

    [DllImport("rdma_win.dll")]
    public static extern bool RdmaWrite(
        IntPtr buffer,
        ulong length,
        ulong remoteAddr,
        uint remoteKey
    );
}
```

---

## 3-5️⃣ C#에서 쓰는 방식

```csharp
IntPtr buf = RdmaNative.RdmaAlloc(4096);

// C#은 그냥 "쓰기"
Marshal.Copy(
    Encoding.UTF8.GetBytes("HELLO"),
    0,
    buf,
    5
);

RdmaNative.RdmaWrite(
    buf,
    5,
    remoteAddr,
    remoteKey
);
```

👉 **C#은 RDMA를 “메모리 복사”처럼 인식**

---

# 4️⃣ 전체 책임 분리 (정리)

| 계층 | 책임                   |
| ---- | ---------------------- |
| C#   | 메시지, 상태, 비즈니스 |
| C++  | RDMA, 메모리, NIC      |
| TCP  | 제어, 핸드셰이크       |
| RDMA | 데이터                 |

---

# 5️⃣ 실전에서 반드시 추가할 것

* Memory Fence (Write 후 Index)
* Wrap-around 처리
* Backpressure (readIndex 확인)
* Multi-writer 시 slot 분리
* 장애 시 TCP 재동기화

---

# 6️⃣ 한 줄 결론

> **RDMA는 “네트워크”가 아니라
> “분산 공유 메모리”로 설계해야 한다.**

---

다음으로 더 깊게 갈 수 있는 것들:

* **Multi-producer RDMA Ring Buffer**
* **Sequence + Ack 없는 신뢰 모델**
* **TCP Zero-Copy ↔ RDMA 성능 비교**
* **Windows RDMA 실제 튜닝 포인트**
