# 닷넷 디버거

1. .NET SDK 설치
2. dotnet tool 설치

   > dotnet tool install -g dotnet-[counters]
   >
3. 

## dotnet-counters

동작중인 프로세스의 정보가 *실시간* 으로 필요 시

> dotnet-counters monitor -p [PID]
>
> or
>
> dotnet-counters monitor -p $(pgrep PROCESS_NAME)

위 명령어를 통해 원하는 프로세스의 정보를 실시간으로 획득

>  dotnet-counters ps 

해당 명령어는 모니터링 가능한 닷넷 프로세스의 목록을 보여준다.

* dotent-counter에 대한 고찰
  * EXCEPTION COUNT
    * 초당 발생하는 오류 수
  * GC
    * Fragmentation => 일반적인 경우 30~50
    * Gen => 1~3세대
      * 0 세대가 늘어나면 안됨 => 지역변수가 쌓여있음
  * MONITOR LOCK CONTENTION COUNT
    * 뭔가를 가지기 위해 싸우는것 => 경합
    * 멀티 스레드에서 싸우는것
  * NUMBER OF ACTIVE TIMES
    * 현재 활성화된 타이머의 갯수 => 시간을 측정할떄
  * THREADPOOL
    * Completed work Item Count => 초당완료된 작업 수
    * Que Length => 처리 대기중인 thread
    * count => 현재 Threadpool의 갯수

## dotnet-stack

모든 쓰레드의 스택 트레이스가 *실시간으로 필요 시*

>  dotenet-stack report -p [PID]

> dotenet-stack ps

해당 명령어는 모니터링 가능한 닷넷 프로세스의 목록을 보여준다.


## dotnet-trace

프로세스의 트레이스를 *기록* 하여 *분석 하고 싶을 시*

> dotnet-trace collect -p [pid]
>
> * 기록된 파일을 출력할 경로를 지정 시
>
> dotnet-trace collect -p PID -o [OUTPUT_PATH]

Tool

* window
  * microsoft/perfview
* 다른 플랫폼
  * Rider

일정 시간의 프로세스에서 각 쓰레드의 평균시간을 측정한다.



## dotnet-dump

프로세스의 메모리 정보를 기록하여 분석하고 싶다면

> dotnet-dump collect -p [PID]
>
> * 기록된 파일을 출력할 경로를 지정 시
>
> dotnet-dump collect -p PID -o [OUTPUT_PATH]

덤프파일은 바이너리 이므로 분석 툴을 이용하여 가능하다

Tool

* Window
  * DM 파일 dotnetmoemory?
* 모든 플랫폼
  * dotnet-trace analyze -p [DUMP_PATH]

프로세스 메모리에서 메모리를 얼마나 잡아먹는지 표현이 가능하다.

만일 어떠한 메모리가 공간을 의도치 하지않게 많이 차지하면 의심을한다

GC 상태를 확인할 수 있다. GEN0 ~ GEN2
