
## 1. 현재 네트워크 인터페이스 상태 확인

먼저 현재 설정된 네트워크 인터페이스를 확인합니다:

```bash
-> ifShow
```

이 명령으로 현재 활성화된 인터페이스들을 볼 수 있습니다.

## 2. 방법 1: ifconfig 명령 사용 (권장)

가장 간단한 방법으로 WindSh에서 직접 설정:

```bash
# 두 번째 인터페이스에 IP 설정
-> ifconfig "gei1", "192.168.1.100", "up"

# 또는 서브넷 마스크와 함께
-> ifconfig "gei1", "192.168.1.100", "netmask", "255.255.255.0", "up"
```

**인터페이스 이름 예시:**

- `gei0`, `gei1` (Gigabit Ethernet)
- `fei0`, `fei1` (Fast Ethernet)
- `mottsec0`, `mottsec1` (Freescale TSEC)
- `motfcc0`, `motfcc1` (Motorola FCC)

## 3. 방법 2: 코드에서 프로그래밍 방식

C 코드에서 직접 설정하는 방법:

```c
#include <ipnet_config.h>
#include <ipcom_shell.h>

STATUS setupSecondInterface(void)
{
    STATUS status;
    
    /* 두 번째 인터페이스 설정 */
    status = ipcom_shell_run("ifconfig gei1 192.168.1.100 netmask 255.255.255.0 up");
    
    if (status != OK)
    {
        printf("Failed to configure second interface\n");
        return ERROR;
    }
    
    printf("Second interface configured successfully\n");
    return OK;
}
```

## 4. 방법 3: 부트 파라미터에 추가 설정

부팅 시 자동으로 두 번째 인터페이스를 설정하려면:

```c
/* usrAppInit.c 또는 사용자 초기화 함수에서 */
void usrAppInit(void)
{
    /* 시스템 초기화 후 */
    setupSecondInterface();
}
```

## 5. 인터페이스별 게이트웨이 설정

각 인터페이스에 다른 게이트웨이가 필요한 경우:

```bash
# 기본 게이트웨이 (첫 번째 인터페이스용)
-> routeAdd "0.0.0.0", "192.168.0.1"

# 특정 네트워크용 라우트 (두 번째 인터페이스용)
-> routeAdd "10.0.0.0/8", "192.168.1.1"
```

## 6. 고급 설정: DHCP 사용

두 번째 인터페이스에서 DHCP를 사용하려면:

```bash
-> dhcpcBind "gei1", TRUE
```

## 7. 설정 확인 명령어

설정 후 확인할 수 있는 명령어들:

```bash
# 인터페이스 상태 확인
-> ifShow

# 라우팅 테이블 확인
-> routestatShow

# 네트워크 연결 테스트
-> ping "192.168.1.1"

# ARP 테이블 확인
-> arpShow
```

## 8. 문제 해결

### 인터페이스가 보이지 않는 경우:

```bash
# 사용 가능한 모든 네트워크 장치 확인
-> devs

# MUX 장치 확인
-> muxShow
```

### 드라이버가 로드되지 않은 경우:

```c
/* BSP에서 드라이버 로드 확인 */
muxDevLoad (unit, endLoad, initString, loopback, pMemPool);
```

## 9. 실제 설정 예시

일반적인 시나리오:

```bash
# 첫 번째 인터페이스 (부팅시 설정됨): 192.168.0.100
# 두 번째 인터페이스 설정:
-> ifconfig "gei1", "10.0.1.100", "netmask", "255.255.255.0", "up"

# 라우팅 설정
-> routeAdd "10.0.0.0/8", "10.0.1.1"

# 설정 확인
-> ifShow
```

## 주의사항

1. **인터페이스 이름**: 실제 하드웨어에 따라 인터페이스 이름이 다를 수 있습니다
2. **드라이버 로드**: BSP에서 해당 이더넷 드라이버가 로드되어 있어야 합니다
3. **IP 충돌**: 두 인터페이스가 같은 서브넷에 있지 않도록 주의하세요
4. **부팅 순서**: 시스템 초기화가 완료된 후에 설정해야 합니다

이 방법들 중 `ifconfig` 명령을 사용하는 것이 가장 간단하고 직관적입니다.