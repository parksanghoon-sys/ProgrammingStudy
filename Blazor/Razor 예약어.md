# Razor 예약어

## 명시적 Razor 표현식

* @ HTML 에서 C# 으로 전환하는 키워드
  * ```csharp
    @{
        var joe = new Person("Joe", 33);
    }

    <p>Age@(joe.Age)</p>
    ```

## 제어 구조

* 조건부

  * > `@if, else if, else, and @switch`
    >
* 루핑

  * > `@for, @foreach, @while, and @do while`
    >
* lock

  * > @lock
    >
* 

@attribute

* 생성된 페이지 또는 보기 클래스에 지정된 속성을 추가 할수 있따

- @page "/counter"

+ @attribute [Route(Constants.CounterRoute)]
  + @atribute Razor 경로가 Constans.CounterRoute/conter 경로로 url이 변경딘다

@Page

* Blazor에 대한 경로 템플릿을 제공하는 지시어이다. 런타임 시 블레이져는 이 템플릿을 사용자가 요청한 URL과 일치시켜 렌더링 페이지를 찾는다
* 예를들면 @page "/index" 의 경우 main url + /index 페이지를 렌더링 할 수 있다

@code

* @code 다음 블록의 텍스트는 c# 코드다 여기는 컴포너트에 필요한 만큼의 코드 블록을 넣는다
* 이 코드 블럭에서는 컴포넌트 클래스 구성원을 정의하고, 계산, 데이터 조회 작업 또는 기타 소스에서 해당 값을 설정할 수 있다.
* 멤버 엑세스 지시문 -> 렌더링 논리에 멤버값을 포함하기 위해서는 맴버 @ 이름뒤에 C# 표현식을 사용된다.

@functions

* 생성된 클래스에 c# 맴버를 추가 할 수 있다.
* 
* ```csharp
  @functions {
      public string GetHello()
      {
          return "Hello";
      }
  }

  <div>From method: @GetHello()</div>
  ```

@implements

* 생성된 클래스에 대한 인터페이스를 구현한다.
* ```csharp
  @implements IDisposable

  <h1>Example</h1>

  @functions {
      private bool _isDisposed;

      ...

      public void Dispose() => _isDisposed = true;
  }
  ```

@inherits

* 상속하는 클래스에 대한 전체 제어를 제공한다

@inject

* Dependency Injection 사용이 가능해 서비스 를 주입받을 수 있다.

@layout

* Razor 구성요소에만 적용이 가능
* 라우팅이 가능한 Razor 구성요소에 대한 레이아웃을 지정한다.

@model

* MVC에만 적용된다
* 뷰나 페이지에 전달되는 모델 유형을 지정한다

@bind

* 데이터 바인딩을 통해 UI에 표시하는 기능
* 양방향 바인딩
  * ```csharp
    <p>
        <input @bind:event="oninput" @bind:get="inputValue" @bind:set="OnInput" />
    </p>

    <p>
        <code>inputValue</code>: @inputValue
    </p>

    @code {
        private string? inputValue;

        private void OnInput(string value)
        {
            var newValue = value ?? string.Empty;

            inputValue = newValue.Length > 4 ? "Long!" : newValue;
        }
    }
    ```
  * 양방향 바인딩을 사용 시 위와같은 올바른 접근 방시을 구성하라 @bind:get @bind:set
