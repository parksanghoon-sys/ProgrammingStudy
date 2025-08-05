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

그러나 CommunityToolkit.Mvvm는 개발자가 원하는 DI 컨테이너를 사용하도록 **Ioc.Default** 를 제공한다. 이는 **System.IServiceProvider** 인터페이스를 구현한 어떤 DI 컨테이너든 등록하여 사용할 수 있게 해줍니다

Ekfktj CommunityToolkit.Mvvm을 사용 시에는 DI를 선택이 가능하다 가장 많이 사용하는 DI 중 하나는 **Microsoft.Extensions.DependencyInjection**이 틀림없으며, **Prism**과 같은 DI를 사용하는 것도 아주 효과적인 조합이다.

#### 3.1.3 직접 DI컨테이너 설계

**IServiceProvider** 인터페이스르 기반으로 DI르 설계하는 방법은 Ioc.Default에 등록이 가능하며 내부 기능적인 연계와 호환이 가능하다, 이처럼 IServiceProvider에서 요구하는 GetService와 같은 최소한의 구현만 하면 되기 떄문에 간단한 DI 구현이 가능하다

**장점**

* 필요한 기능만 포함한 간단한 DI 컨테이너를 구현하여 프로젝트의 복잡성을 낮춘다
* 내부적으로 다양한 기능을 설계하고 제어하고 확정이 가능
* 전체적인 프레임 아키텍쳐와 프로젝트 설계를 직접 정교하게 구축이 가능하다
* 특정 플렛폼에 종속되지 않는 일관된 DI 컨테이너를 제공하여 크로스 플랫폼 개발에 유리하다

예시 코드

```csharp
// IServiceProvider 기반의 DI 컨테이너 구현
public class SimpleServiceProvider : IServiceProvider
{
    private readonly Dictionary<Type, Func<object>> _services = new();

    public void AddService<TService>(Func<TService> implementationFactory)
    {
        _services[typeof(TService)] = () => implementationFactory();
    }

    public object GetService(Type serviceType)
    {
        return _services.TryGetValue(serviceType, out var factory) ? factory() : null;
    }
}

// DI 컨테이너 등록 및 사용
var serviceProvider = new SimpleServiceProvider();
serviceProvider.AddService<IMainViewModel>(() => new MainViewModel());

Ioc.Default.ConfigureServices(serviceProvider);
```

이처럼 **IServiceProvider** 인터페이스를 기반으로 간단한 DI 컨테이너를 구현하면, **CommunityToolkit.Mvvm**의 **Ioc.Default**에 등록하여 내부 기능적인 연계와 호환까지 가능합니다. 일반적으로  **Microsoft.Extensions.DependencyInjection** , **Prism**과 같은 대중적인 DI를 사용하는 것이 무겁다고 느껴진다면, 이를 직접 만드는 것은 아주 매력적인 선택입니다.

 **참고** :

**IServiceProvider**와 같은 **System.ComponentModel** 표준을 따르지 않을 경우 **CommunityToolkit.Mvvm**과의 **Ioc** 호환성은 잃게 될 수 있습니다. 하지만 **CommunityToolkit.Mvvm**을 MVVM 관련 모듈로서만 활용하고, 특정 플랫폼과 프레임워크에 종속되지 않은 더욱 특화되고 일관된 DI 컨테이너를 만들 수 있게 됩니다. 이는 크로스 플랫폼 등 여러 XAML 기반 플랫폼에 공통으로 사용될 수 있는 프레임워크를 만드는 데에도 상당히 적합합니다.

## 4. WPF 기술을 다른 플랫폼에서 효과적으로 사용하기 위한 전략

WPF 강력한 기능을 다른 XAML 기반 플랫폼에서 최대한 활용하기 위해서는 몇 가지 히스토리와 핵심전략을 알아야한다. 또한 WPF 기술을 그대로 사용할 수 있는 플랫폼의 특징을 자세하게 이해할필요가 있다.

### 4.1 플랫폼 간의 특징과 차이점 이해

* **UWP와 WinUI 3의 차이점** : UWP는 Windows 10을 위한 플랫폼으로, 스토어 등록을 위한 가이드라인과 WinAPI 제한 등 WPF나 WinForms 레거시와의 호환이 어려웠습니다. 이에 따라 WinUI 3가 등장하여 UWP의 모든 특장점을 계승하면서도 문제점들을 개선하고, WPF처럼 자유도 높은 플랫폼으로 발전했습니다.
* **Uno Platform 데스크톱과 WinUI 3의 동일성** : Uno Platform의 Windows, macOS, Linux를 아우르는 데스크톱 플랫폼은 WinUI 3의 방식을 그대로 따릅니다. 따라서 UWP의 핵심 라이브러리를 WinUI 3가 그대로 동일하게 사용하고, Uno Platform도 WinUI 3의 방식을 동일하게 사용하기 때문에 `Microsoft.*`로 시작하는 모든 DLL 라이브러리를 그대로 공유합니다.

이러한 각 플랫폼 간의 특징을 이해하면 **Uno Platform 데스크톱의 활용 가치**가 얼마나 효과적이고 매력적인 플랫폼인지 알 수 있습니다. 따라서 WPF와 Uno Platform 간의 기술 공유와 전환은 WinUI 3와 UWP까지 연계되기 때문에 더욱 효과적이고 효율적인 전략입니다.

### 4.2 VisulStateManager (VSM)의 적극적인 활용

모든 플랫폼에서 WPF의 Trigger를 직접 사용할 수 없기에, 이를 대처하기위해 VSM을 활요하여 문제를 해결할 수있다.

커스텀 컨틀롤과 XAML간의 상태처리에 최적화 되어 있다, WPF의 모든 CostomControl의 내부 설계도 Trigger에서 VSM으로 변경이 되고 있다.

**장점**

* Trigger를 직접 사용하지 못하는 플랫폼에서 VSM을 통해 동일한 기능을 구현이 가능하다
* UI 상태관리와 애니메이션을 효과적으로 구현이 가능
* 플랫폼 별 다른 동작을 VSM을 통해 통일 가능

### 4.3 IValueConverter의 유연한 활용

**IValueConverter**는 데이터 바인딩 시 값의 변환을 가능하게 해주는 인터페이스로, 플랫폼 간의 차이를 추상화하는 데 유용합니다.

 **전략적 활용** :

* Trigger와 거의 동일한 기능을 구현하고 대체할 수 있으며, 사용하기 쉽고 효과적인 소스 코드를 작성할 수 있습니다.
* 매번 Converter를 만들어야 하고 재사용성에 기준이 모호하기 때문에, 재사용성에 너무 얽매이지 않고 유연하게 사용하는 것이 좋습니다.
* 재사용성이 없더라도 직관적으로 사용하는 것이 중요하며, 이름을 명확하게 해서 분기를 최대한 줄이고 특화되게 사용하는 것이 좋습니다.

 **한계와 보완** :

* 모든 것을 **IValueConverter**로만 사용하는 것은 한계가 있습니다.
* **IValueConverter**는 단순한 변환에 사용하고, 복잡한 시나리오를 관리하는 것은 부담이 되기 때문에 **VSM**을 통해 해결합니다.
* 복잡한 상태 처리는 **VisualStateManager**를 사용하는 것이 좋습니다.

결국 **IValueConverter**는 VSM의 부족한 부분을 채워주고, 간단하고 단순한 변환 작업을 직관적이고 재사용에 너무 집착하지 말고 유연하게 사용하는 것이 중요합니다.

## 5. 분산된 프로젝트 관리를 위한 Bootstrapper 설계 방법

앱이 복잡해지고 모듈화 될수록 초기화 과정과 의존성 관리가 중요해진다 Bootstrapper 패턴은 이러한 초기화 로직을 중앙화 하고 관리하는데 유용하다

모든 플랫폼은 App 설계를 기본으로 하지만, 플랫폼마다 성격과 방식 등이 다 다르기 떄문에 App 설계가 제각각 이다, 따라서 모든 플랫폼에서 동일한 개발 방식을 유지하기 위해서 Bootstrapper 구조설계가 효과적일 수 있다.

### 5.1 Bootstrapper 역활과 필요성

**Bootstrapper**의 기능

* 의존성 주입 설정 : DI 컨테이너를 초기화 하고 필요한 서비스와 뷰, 뷰모델을 등록
* 뷰모델과 뷰의 연결 관리
* 중앙화된 설정 관리 : App의 설정을 Bootstrapper에서 관리하고, 애플리케이션 프로젝트는 해당 역할만 수행, 나머지 기능은 프로젝트 분산화와 모듈화를 통해 관리
* 그 밖에도 중앙화 관리를 위한 항목을 유여한게 늘려나갈 수 있고 App에 영향을 주지 안흔다

**장점**

* App의 초기화 로직을 분리하여 코드의 가독서오가 유지보수성을 높인다
* 프로젝트 분산화와 모듈화를 통해 기능 구현을 독립적으로 개발 가능
* 플랫폼 간의 구조적인 차이를 최소화하여 일관된 아키텍쳐를 유지한다.

예시코드

```csharp
namespace Jamesnet.Core;

public abstract class AppBootstrapper
{
    protected readonly IContainer Container;
    protected readonly ILayerManager Layer;
    protected readonly IViewModelMapper ViewModelMapper;

    protected AppBootstrapper()
    {
        Container = new Container();
        Layer = new LayerManager();
        ViewModelMapper = new ViewModelMapper();
        ContainerProvider.SetContainer(Container);
        ConfigureContainer();
    }

    protected virtual void ConfigureContainer()
    {
        Container.RegisterInstance<IContainer>(Container);
        Container.RegisterInstance<ILayerManager>(Layer);
        Container.RegisterInstance<IViewModelMapper>(ViewModelMapper);
        Container.RegisterSingleton<IViewModelInitializer, DefaultViewModelInitializer>();
    }

    protected abstract void RegisterViewModels();
    protected abstract void RegisterDependencies();

    public void Run()
    {
        RegisterViewModels();
        RegisterDependencies();
        OnStartup();
    }

    protected abstract void OnStartup();
}
```
