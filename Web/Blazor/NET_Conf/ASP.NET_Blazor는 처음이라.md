# Blazor란 무엇인가

## 공식 문서

* Blazor은 Net을 이용하여 대화영 클라이언트 측 웹 UI를 빌드하기 위한 프레임워크
* JavaScript 대신 C#을 사용하여 풍부한 대화형 UI를 만든다
* UI를 HTML 및 CSS로 렌더링 한다
* Net으로 작성된 서버 측 클리언트 측 앱로직을 공유한다.

## Components

* Blazor 앱은 Components를 기반으로 한다
  * Components는 페이지, 대화상자 또는 데이터 입력 양식과 같은 UI 요소이다
* 유연한 UI 렌더링 로직을 정의한다.
* 사용자 이벤트를 처리
* 중첩되고 재사용 될 수 있다
* Components는 특히 클라이언트 쪽 UI 논리 및 구성에 사용

## Blazor Server

* Components를 이용하여 페이지를 Body 안에서 구성을 변경하며 호출해준다.
* Server 에서 Signal R을 통해서 Client와 통신을하며 DOM 페이지를 그려준다
* @bind 를 이용하여 변수를 Binding을 시켜준다
* 서버와 지속적인 통신이 필요하므로 연결이 끊어질 경우 UI도 멈추게 된다
* 서버자원을 이용하기 떄문에 static으로 선언시 모든 클라이언트에서 해당 변수값을 사용할 수 있다.
* 양방향 통신이 필요하다면 Signal Hub가 필요하다

## Blazor WebAssembly App

* Asp.Net Core 서버가 반드시 필요하지않는다. 외에 다른 서버접속이 가능하다.
* 웹앱 기능이 가능하다. 즉 서버에서 독립적으로 돌아갈 수 있다.
* 브라우져 내에 배포가 된다 서버에 의존적이지 X
* 기존 서버는 웹페이지 시작시 메인페이지만 다운로드 그러나 Assembly 같은경우에는 필요한 자원을 모두 다운로드
* 서버는 db 커넥션 정상 생성 WebAssembly는 웹부라우져가 주체가 되어 WepSocket 및 Http만 지원

## 결론

* Blazor Server의 경우 ASP.Net Webform이 발전되어 SPA 프레임워크 또는 라이브러리의 장점을 이어받음
* Blazor WebAssembly의 경우 Silberlight 시절의 RIA가 발전된 느낌
* Signal R을 이용해 DOM을 제어하므로, HTTP 통신이 아님을 주의해야함(GET,POST와 같은 개념이 아니다)
* React, Angular, Vue와 같은 SPA 프레임워크를 대체할수 있으나 현재 Javascript로 되어 있기떄문에 JSRuntime을 이용해 Javascript와의 많은 상호작용이 필요
