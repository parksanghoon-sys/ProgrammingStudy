# XAML 기반 프레임워크와 크로스 플랫폼 프로젝트 아키텍처 설계를 위한 심층 기술 분석

## 1. XAML기반 플렛폼과 크로스플랫폼 개요

XAML은 UI를 선언적으로 정의하기위한 마크업 언어로, 다양한 플랫폼에서 사용되어 진다, XAML은 객체, 즉 클래스로 이루어진 계층 구조를 가지고 있어 개발자가 UI를 객체 지향적으로 설계하고 관리할 수 있게 해준다, 이러한 구조는 개발자가 XAML을 직접다루는게 유리하다

주요 XAML 기반 프레임워크

* **WPF (Windows Presentation Foundation)** : Windows 데스크톱 애플리케이션 개발을 위한 강력한 프레임워크로, 풍부한 UI와 그래픽 기능을 제공합니다.
* **Silverlight** : 웹 브라우저에서 실행되는 인터넷 애플리케이션을 만들기 위한 플랫폼으로, 현재는 지원이 중단되었습니다. WPF의 라이트 버전으로, 플러그인 방식으로 동작했습니다. 웹 표준 정책의 변화로 플러그인 기반의 웹 플랫폼이 사라지는 계기가 되었으며, Trigger의 단점을 보완하기 위해 **VisualStateManager(VSM)**가 Silverlight 2에서 도입되었습니다.
* **Xamarin.Forms** : iOS, Android, Windows를 지원하는 모바일 앱 개발 플랫폼입니다. Mono 기반으로 시작된 최초의 XAML 기반 크로스 플랫폼으로, Microsoft가 전략적으로 인수하여 .NET Core의 기반이 되기도 했습니다.
* **UWP (Universal Windows Platform)** : Windows 10 이상에서 실행되는 애플리케이션 개발을 위한 플랫폼입니다. Microsoft Store 등록이 필요하며, WinAPI 사용 제한 등 제약이 있습니다. WPF와 동일한 커스텀 컨트롤 설계를 지원합니다.
* **WinUI 3** : Windows용 네이티브 UI 프레임워크로, 최신 Windows 앱 개발을 위한 차세대 UI 플랫폼입니다. UWP의 모든 것을 계승하면서 제약을 해소하고, WPF의 확장성을 수용했습니다.
* **MAUI (.NET Multi-platform App UI)** : .NET 6부터 도입된 크로스 플랫폼 UI 프레임워크로, 모바일과 데스크톱 앱을 단일 프로젝트로 개발할 수 있습니다.
* **Uno Platform** : UWP와 WinUI의 API를 다양한 플랫폼에서 사용할 수 있게 해주는 프레임워크로, 웹(WebAssembly), 모바일, 데스크톱을 지원합니다. 거의 모든 플랫폼을 지원하며, WPF와 동일한 커스텀 컨트롤 설계를 제공합니다.
* **Avalonia UI** : WPF 스타일의 XAML을 크로스 플랫폼으로 사용할 수 있게 해주는 오픈 소스 UI 프레임워크입니다. WPF와 동일한 커스텀 컨트롤 설계를 지원하며, 독자적인 기술 확장을 통해 다양한 플랫폼을 지원합니다.
* **OpenSilver** : 과거 Silverlight를 OpenSilver로 마이그레이션하는 데 최적화된 오픈 소스 플랫폼입니다. Silverlight와 거의 동일한 방식으로 동작하며, WPF 개발자에게도 친숙한 환경을 제공합니다.

## 2. 뷰와 뷰모델의 연결 전략 분석

MVVM 패턴에서 뷰와 뷰모델의 연결은 핵심적인 부분이다, 어떻게 연결하냐에 따라 MVVM을 사용하는 방식이 완전 달라진다, 따라서 우리는 MVVM을 어떻게 사용할 것인가에 대한 목적에 맞게 DataContext 할당 방식을 결정해야한다.

### 2.1 전통적인 DataContext 할당방식

코드 비하인드에서 뷰모델을 생서아혹 뷰의 DataContext에 직접할당 하는 방식

```csharp
public MainWindow()
{
    InitializeComponent();
    DataContext = new MainViewModel();
}
```

#### 장점

* 구현이 간단, 직관적
* 뷰모델의 생성 시점을 명확하게 제어 가능
* 필요한 인자를 생성자에 전달이 가능

#### 단점

* 뷰와 뷰모델간의 강한 결합
* 단위 테스트시 뷰모델의 모킹(Mock)이 어렵다
* DI를 활요하기 어렵다
* DataContext 할당 시점을 직접 정해야하며, 그 시점의 일관성을 유지하기 어렵다

### 2.2 XAML에서 뷰모델인스턴스 생성

XAML 에서 DataContext를 생서하여 뷰모델에 인스턴스하는 방식

```csharp
<Window x:Class="MyApp.MainWindow"
        xmlns:local="clr-namespace:MyApp.ViewModels">
    <Window.DataContext>
        <local:MainViewModel />
    </Window.DataContext>
    <!-- Window content -->
</Window>
```

#### 장점

* XAML에서의 인텔리센스 지원으로 바인딩 오류를 줄일 수 있다.
* 디자이너에서 실제 데이터 바인딩을 확인할 수 있다.
* 뷰와 뷰모델의 관계를 명시적으로 표현

#### 단점

* 뷰모델 생성시 의존성 주입이 어렵다
* 복잡한 초기화 로직이나 매개변수의 전달이 제한
* DataContext할당 시점이 강제화 되어 유연성이 떨어짐

### 2.3 뷰모델 직접 생성 및 의존성 전달

코드 비하인드에서 뷰모델을 생성하면서 필요한 의존성을 전달하는 방식

```csharp
public MainWindow()
{
    InitializeComponent();
    var dataService = new DataService();
    var loggingService = new LoggingService();
    DataContext = new MainViewModel(dataService, loggingService);
}
```

#### 장점

* 뷰모델 생성 시 의존성을 명시적으로 전달
* 복잡한 초기화 로직 구현
* 런타임에 따라 뮤모델의  인스턴스를 유연하게 생성

#### 단점

* 뷰가 뷰모델의 의존성에대해 알아야 한다.
* 의존성이 늘어날수록 코드 비하인드가 복잡
* 뷰와 뷰모델의 결합이 높음
* DataContext 할당 시점을 직접 정해야 하며, 그 시점의 일관성을 유지하기 어려움
* DI로 관리하지 않는 항목을 마음대로 전달가능하여 무분별하게 난발

### 2.4 의존성 주입(DI) 활용

DI 컨테이너를 사용하여 뷰모델과 그 의존성을 관리하면 뷰와 뷰모델 간의 결합도를 낮출 수 있습니다.

```csharp
public MainWindow()
{
    InitializeComponent();
    DataContext = ServiceProvider.GetService<MainViewModel>();
}
```

#### **장점** :

* 뷰와 뷰모델 간의  **결합도를 낮춥니다** .
* 의존성 관리가 중앙화되어 **유지보수성이 향상**됩니다.
* **단위 테스트**가 용이해집니다.
* 런타임에 의존성을 유연하게 변경할 수 있습니다.

#### **단점**:

* DI 컨테이너 설정이 초기에는  **복잡할 수 있습니다** .
* 팀원들이 **DI 패턴에 대한 이해**가 필요합니다.
* 여전히 **DataContext**에 직접 뷰모델을 생성해야 하는 부분과 할당 시점을 계속해서 정해야 하기 때문에 일관성을 유지하기 어려울 수 있습니다.
* 뷰모델을 **싱글톤**으로 관리할지, **인스턴스**로 관리할지 결정해야 하며, 뷰의 라이프사이클까지 고려해야 합니다. 정확하고 확실한 규칙을 정해놓고 사용해야 합니다.

### 2.5 뷰의 자동 뷰모델 생성 전략

위 문제를 보안하기 위해 뷰가 생성될 때 약속된 시점에 뷰모델의 의존성을 주입으로 생성하고 **DataContext**를 할당하는 방법을 고려, 예를 들어 ContentControl 기반의 뷰모델을 자동으로 생성해주는 뷰를 설계하는 것이 효과적이다.

```csharp
public class UnoView : ContentControl
{
    public UnoView()
    {
        this.Loaded += (s, e) =>
        {
            var viewModelType = ViewModelLocator.GetViewModelType(this.GetType());
            DataContext = ServiceProvider.GetService(viewModelType);
        };
    }
```

#### 장점

* DatContext 할당 시점의 일관성을 유지가 자능하다
* 뷰와 뷰모델의 결합도가 낮아진다
* 뷰모델의 생성과 의존성 주입이 자동으로 처리
* 뷰는 자신이 어떤 뷰모델을 가져야하는지 알 필요가 없다

**단일 뷰**를 관리함으로써 정확한 시점과 처리 로직을 단일화하여 관리가능, 다만 뷰와 뷰모델의 매핑이 반드시 필요하며, 이를 위해 딕셔너리 매핑테이블을 활용이 가능하다, 이렇게 하면 뷰와 뷰모델의 연결정보를 중앙에서 관리할 수 있따, 연결을 관리하기 위한 매핑 방법으로는 후반의 **Bootstrapper 설계방법론**에서 확인이 가능하다

## 3. 프레임 워크 설계를 위한 필수 기능 및 구현 방안

앱의 아키텍쳐를 설계할 때는 **재사용성과 확장성**을 고려해야한다, 이를 위해서는 의존성 주입(DI)컨테이너의 활용이 필수 적이다

### 3.1 의존성 주입 (DI) 컨테이너의 활용

DI는 현대 소프트웨어 개발에서 필수의 패턴으로, 의존성 관리와 결합도 감소에 큰 도움을 준다 그러나 WPF와 같은 데스트탑 에플리케이션에는 웹 어플리케이션에서 사용하는 DI 컨터이너가 맞지 않을 수있다.

#### 3.1.1 Microsoft.Extensions.DependencyInjection의 활용과 고려 사항

**Microsoft.Extensions.DependencyInjection**은 .NET에서 공식적으로 제공하는 DI 컨테이너로, .NET Foundation의 표준과도 같습니다 ASP, EF, MAUI등 다양한 플랫폼과 프레임워크에서 사용되며 **Transient** ,  **Scoped** , **Singleton** 등의 라이프사이클 관리 기능을 제공합니다, 하지만 WPF에서는 이 표준 DI의 라이프 사이클이 실제로 WPF와 맞지 않을 수이 있다.

**고려사항**

* WPF와 같은 앱에서는 **Scoped**라이프 사이클이 필요하지 않는다
* Transient 나 Singleton 개념이 서비스나 웹 어플리케이션에 적합하게 설계되어 있어, WPF에는 일부 기능이 맞지 않는다
* 불필요한 복잡성을 가져올 수 있으며, WPF의 사용 용도에 맞게 간다면 가벼운 DI 컨테이너가 더 적합하다

#### 3.1.2 **CommunityToolkit.Mvvm** DI

**CommunityToolkit.Mvvm**은 Microsoft.Extensions.DependencyInjection와 같은 DI를 직접적으로 제공하지 않는다. 그 이유는 **Microsoft.Extensions.DependencyInjection**이 WPF의 라이프사이클 성격과 정확하게 맞지 않기 때문으로 볼 수 있습니다.

그러나 CommunityToolkit.Mvvm는 개발자가 원하는 DI 컨테이너를 사용하도록 **Ioc.Default** 를 제공한다. 이는 S
