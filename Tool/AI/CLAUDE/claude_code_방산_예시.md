WPF 방산 개발자 관점에서 실전 구조를 만들어드리겠습니다. 보안, 품질, 표준 준수가 핵심입니다!

# 🛡️ WPF 방산 시스템 개발을 위한 Claude Code 구성

## 🗂️ 프로젝트 폴더 구조

```
tactical-command-system/              # 전술지휘체계
│
├── CLAUDE.md                         # ⭐ 프로젝트 헌법 (보안 등급: 대외비)
├── SECURITY.md                       # 🔒 보안 가이드라인
├── .claudeignore                     # 민감 정보 제외
│
├── .claude/
│   ├── mcp/
│   │   └── servers.json             # 오프라인 전용 MCP
│   │
│   ├── agents/                      # 방산 특화 서브에이전트
│   │   ├── security-auditor.md     # 보안 감사관
│   │   ├── code-reviewer.md        # 코드 리뷰어
│   │   ├── test-generator.md       # 테스트 생성기
│   │   ├── doc-writer.md           # 문서 작성자
│   │   └── performance-analyzer.md # 성능 분석가
│   │
│   ├── skills/                      # WPF + 방산 특화 스킬
│   │   ├── wpf-mvvm/
│   │   │   ├── SKILL.md
│   │   │   ├── mvvm-patterns.md
│   │   │   ├── binding-optimization.md
│   │   │   └── templates/
│   │   │       ├── viewmodel-template.cs
│   │   │       └── view-template.xaml
│   │   │
│   │   ├── defense-standards/
│   │   │   ├── SKILL.md
│   │   │   ├── mil-std-498.md      # 군사 표준
│   │   │   ├── coding-standards.md # 방산 코딩 표준
│   │   │   └── security-requirements.md
│   │   │
│   │   ├── real-time-systems/
│   │   │   ├── SKILL.md
│   │   │   ├── threading-patterns.md
│   │   │   └── performance-critical.md
│   │   │
│   │   └── gis-mapping/
│   │       ├── SKILL.md
│   │       ├── map-rendering.md
│   │       └── coordinate-systems.md
│   │
│   ├── hooks/
│   │   ├── pre-commit.ps1          # 보안 체크 + 린트
│   │   ├── post-edit.ps1           # 자동 포맷 + 감사 로그
│   │   └── security-scan.ps1       # 민감정보 스캔
│   │
│   └── commands/
│       ├── review-security.md      # 보안 리뷰 명령
│       ├── generate-docs.md        # 문서 자동 생성
│       └── run-integration-test.md # 통합 테스트
│
├── docs/                            # 개발 문서
│   ├── architecture/
│   │   ├── system-overview.md      # 시스템 개요 (보안: 대외비)
│   │   ├── security-architecture.md
│   │   ├── data-flow.md
│   │   └── CLAUDE/
│   │       ├── wpf-architecture.md
│   │       ├── threading-model.md
│   │       └── hardware-interface.md
│   │
│   ├── standards/
│   │   ├── coding-convention.md    # 방산 코딩 규칙
│   │   ├── naming-rules.md
│   │   ├── security-checklist.md
│   │   └── mil-std-compliance.md
│   │
│   ├── testing/
│   │   ├── test-strategy.md
│   │   ├── quality-gates.md
│   │   └── acceptance-criteria.md
│   │
│   └── operations/
│       ├── deployment-guide.md
│       ├── security-procedures.md
│       └── incident-response.md
│
├── src/
│   ├── TacticalCommand.sln
│   │
│   ├── Core/                        # 핵심 도메인
│   │   ├── CLAUDE.md               # Core 레이어 컨텍스트
│   │   ├── Domain/
│   │   ├── Application/
│   │   └── Infrastructure/
│   │
│   ├── Presentation/                # WPF UI
│   │   ├── CLAUDE.md               # Presentation 레이어 컨텍스트
│   │   ├── Views/
│   │   │   └── CLAUDE.md           # View 패턴 가이드
│   │   ├── ViewModels/
│   │   │   └── CLAUDE.md           # ViewModel 패턴 가이드
│   │   ├── Controls/               # 커스텀 컨트롤
│   │   ├── Behaviors/              # Attached Behaviors
│   │   └── Resources/
│   │       ├── Styles/
│   │       └── Templates/
│   │
│   ├── Services/                    # 비즈니스 서비스
│   │   ├── CLAUDE.md
│   │   ├── Mapping/                # GIS/지도 서비스
│   │   ├── Communication/          # 통신 서비스
│   │   ├── Sensor/                 # 센서 인터페이스
│   │   └── Security/               # 보안 서비스
│   │
│   ├── Hardware/                    # 하드웨어 연동
│   │   ├── CLAUDE.md
│   │   ├── Interfaces/
│   │   └── Drivers/
│   │
│   └── Tests/
│       ├── CLAUDE.md
│       ├── Unit/
│       ├── Integration/
│       └── Security/               # 보안 테스트
│
├── tools/                           # 개발 도구
│   ├── security-scanner/
│   ├── code-metrics/
│   └── test-automation/
│
└── scripts/                         # 자동화 스크립트
    ├── build.ps1
    ├── test.ps1
    ├── security-check.ps1
    └── generate-reports.ps1
```

---

## 📝 루트 CLAUDE.md (방산 특화)

```markdown
# 전술지휘체계 (Tactical Command System) - 개발 가이드

> 🔒 **보안 등급: 대외비**
> 
> ⚠️ **CRITICAL SECURITY NOTICE**
> - 이 시스템은 국방 정보를 처리합니다
> - 모든 코드는 보안 검토 필수
> - 민감 정보 하드코딩 절대 금지
> - 개발 중 보안 사고 발생 시 즉시 보고

## 프로젝트 개요 (WHY)

### 목적
- 실시간 전술 상황 모니터링 및 지휘통제
- 다중 센서 데이터 통합 표시
- 안전하고 신뢰성 높은 의사결정 지원

### 핵심 가치
1. **보안 (Security)**: 기밀성, 무결성, 가용성
2. **신뢰성 (Reliability)**: 99.9% 가동률
3. **성능 (Performance)**: 실시간 응답 (<100ms)
4. **표준 준수 (Compliance)**: MIL-STD-498, DO-178C

## 기술 스택 (WHAT)

### 필수 기술
- **언어**: C# 10, .NET 6 LTS
- **UI 프레임워크**: WPF (XAML)
- **아키텍처**: Clean Architecture + MVVM
- **DI**: Microsoft.Extensions.DependencyInjection
- **메시징**: MediatR
- **매핑**: AutoMapper
- **테스트**: xUnit, Moq, FluentAssertions
- **보안**: BouncyCastle, SecurityDriven.NET

### WPF 특화
- **UI 패턴**: MVVM (엄격 적용)
- **커맨드**: ICommand, RelayCommand
- **바인딩**: INotifyPropertyChanged, ObservableCollection
- **스레딩**: Dispatcher, async/await
- **GIS**: MapSuite, custom rendering

### 방산 특화
- **통신**: MIL-STD-1553, RS-422
- **암호화**: AES-256, RSA-2048
- **인증**: CAC/PIV 카드 인증
- **로깅**: 감사 로그 (모든 작업 기록)

## 개발 워크플로우 (HOW)

### 1. 보안 우선 개발

#### 절대 금지 사항 ❌
```csharp
// ❌ 절대 금지: 하드코딩된 자격증명
private const string API_KEY = "secret123";
private const string PASSWORD = "admin";

// ❌ 절대 금지: 민감정보 로깅
Logger.Info($"User password: {password}");

// ❌ 절대 금지: 취약한 암호화
var des = new DESCryptoServiceProvider();

// ❌ 절대 금지: SQL Injection 취약점
var query = $"SELECT * FROM Users WHERE Username = '{username}'";
```

#### 필수 사항 ✅

```csharp
// ✅ 환경변수 또는 보안 저장소 사용
var apiKey = _configuration["SecureStore:ApiKey"];

// ✅ 민감정보 마스킹
Logger.Info($"User logged in: {username} [REDACTED]");

// ✅ 강력한 암호화
using var aes = Aes.Create();
aes.KeySize = 256;

// ✅ Parameterized Query
var query = "SELECT * FROM Users WHERE Username = @username";
```

### 2. MVVM 패턴 (엄격 적용)

```
View (XAML)
    ↕ DataBinding
ViewModel (C#)
    ↕ Commands/Properties
Model (Domain)
```

**YOU MUST 규칙:**

* View는 절대 비즈니스 로직 포함 금지
* ViewModel은 View 참조 금지
* Code-behind는 최소화 (UI 로직만)
* Model은 UI 독립적

상세: @.claude/skills/wpf-mvvm/SKILL.md

### 3. 코드 작성 전 체크리스트

```powershell
# 보안 스캔
.\scripts\security-check.ps1

# 빌드
dotnet build

# 단위 테스트
dotnet test

# 코드 분석
dotnet format --verify-no-changes
```

### 4. 성능 요구사항

**CRITICAL 성능 기준:**

* UI 응답: < 100ms
* 지도 렌더링: 60 FPS
* 센서 데이터 처리: < 50ms
* 메모리 누수: 0 tolerance

성능 측정:

```csharp
// 모든 critical path에 성능 측정 추가
using (PerformanceMonitor.Measure("SensorDataProcessing"))
{
    // 성능 critical 코드
}
```

### 5. 스레딩 규칙

```csharp
// ✅ UI 스레드에서 UI 업데이트
Application.Current.Dispatcher.Invoke(() =>
{
    StatusText = "Updated";
});

// ✅ 백그라운드 작업
await Task.Run(() => ProcessLargeDataset());

// ✅ ConfigureAwait 사용
await GetDataAsync().ConfigureAwait(false);

// ❌ UI 스레드에서 블로킹 작업 금지
Thread.Sleep(1000); // 절대 금지!
```

## 파일 구조 규칙

### Naming Convention (엄격)

```
Views/
  ├── TacticalMapView.xaml        # View suffix
  └── TacticalMapView.xaml.cs

ViewModels/
  └── TacticalMapViewModel.cs      # ViewModel suffix

Models/
  └── TacticalUnit.cs              # 도메인 모델

Services/
  └── ITacticalDataService.cs      # I prefix for interface
```

### 파일 크기 제한

* ViewModel: 최대 500 라인
* View (XAML): 최대 300 라인
* Service: 최대 400 라인
* 초과 시 분리 필수

## 보안 체크리스트

모든 PR은 다음을 통과해야 함:

### A. 인증/인가

* [ ] 모든 민감 작업에 권한 체크
* [ ] 세션 타임아웃 구현
* [ ] 재인증 메커니즘

### B. 데이터 보호

* [ ] 저장 데이터 암호화
* [ ] 전송 데이터 암호화 (TLS 1.3+)
* [ ] 메모리 내 민감정보 즉시 제거

### C. 입력 검증

* [ ] 모든 사용자 입력 검증
* [ ] SQL Injection 방어
* [ ] XSS 방어 (WebView 사용 시)

### D. 로깅/감사

* [ ] 모든 보안 관련 이벤트 로깅
* [ ] 민감정보 마스킹
* [ ] 로그 무결성 보호

상세: @docs/standards/security-checklist.md

## 테스트 요구사항

### 최소 커버리지

* 단위 테스트: 85%
* 통합 테스트: 70%
* 보안 테스트: 100% (critical path)

### 필수 테스트 케이스

```csharp
[Fact]
public async Task Login_WithInvalidCredentials_ShouldFail()
{
    // Arrange
    var authService = new AuthenticationService();
  
    // Act
    var result = await authService.LoginAsync("user", "wrong");
  
    // Assert
    result.Should().BeFail();
    result.Error.Should().Be(AuthError.InvalidCredentials);
}

[Fact]
public void TacticalMap_WhenDataUpdated_ShouldNotify()
{
    // INotifyPropertyChanged 테스트 필수
}
```

## 문서화 요구사항

### 필수 문서

1. **API 문서** : XML 주석 필수
2. **아키텍처 문서** : 모든 주요 결정 기록
3. **보안 문서** : 위협 모델링, 대응 방안
4. **운영 매뉴얼** : 배포, 모니터링, 장애 대응

### XML 주석 예시

```csharp
/// <summary>
/// 전술 상황 데이터를 처리하고 지도에 표시합니다.
/// </summary>
/// <param name="tacticalData">전술 상황 데이터</param>
/// <returns>처리 결과</returns>
/// <exception cref="SecurityException">권한이 없는 경우</exception>
/// <remarks>
/// 보안 등급: 대외비
/// 성능 요구사항: 50ms 이내 처리
/// </remarks>
public async Task<ProcessResult> ProcessTacticalDataAsync(
    TacticalData tacticalData)
{
    // 구현
}
```

## 품질 게이트

커밋 전 필수 통과:

1. ✅ 보안 스캔 (0 high/critical)
2. ✅ 정적 분석 (0 warning)
3. ✅ 단위 테스트 (85%+)
4. ✅ 코드 리뷰 (2명 승인)
5. ✅ 성능 벤치마크

## MIL-STD 준수

### MIL-STD-498 (소프트웨어 개발 표준)

* SDP: Software Development Plan
* SRS: Software Requirements Specification
* SDD: Software Design Description
* STD: Software Test Description

문서 템플릿: @docs/standards/mil-std-498/

## 참고 문서

### 자주 참조

* WPF MVVM: @.claude/skills/wpf-mvvm/SKILL.md
* 보안 가이드: @docs/standards/security-checklist.md
* 코딩 표준: @docs/standards/coding-convention.md

### 필요시 참조

* 스레딩 모델: @docs/architecture/CLAUDE/threading-model.md
* GIS 렌더링: @.claude/skills/gis-mapping/SKILL.md
* 하드웨어 인터페이스: @docs/architecture/CLAUDE/hardware-interface.md

## 긴급 연락처

* 보안 담당자: [REDACTED]
* 시스템 아키텍트: [REDACTED]
* QA 리드: [REDACTED]

```

---

## 🛠️ WPF MVVM Skill

**.claude/skills/wpf-mvvm/SKILL.md:**

```markdown
---
name: wpf-mvvm-expert
description: WPF MVVM 패턴 전문가 (방산 시스템 특화)
---

# WPF MVVM Expert Skill

## 역할
방산 시스템에 최적화된 WPF MVVM 패턴을 구현합니다.

## ViewModel 표준 패턴

### 기본 구조
```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace TacticalCommand.Presentation.ViewModels
{
    /// <summary>
    /// 전술 지도 화면 ViewModel
    /// </summary>
    public partial class TacticalMapViewModel : ObservableObject
    {
        private readonly ITacticalDataService _tacticalDataService;
        private readonly ISecurityService _securityService;
        private readonly ILogger<TacticalMapViewModel> _logger;

        // Observable Properties
        [ObservableProperty]
        private string _title = "전술 상황도";

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private ObservableCollection<TacticalUnit> _units = new();

        // Commands
        [RelayCommand]
        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
              
                // 권한 체크 (방산 필수)
                if (!await _securityService.CheckPermissionAsync("VIEW_TACTICAL_MAP"))
                {
                    _logger.LogSecurityWarning("Unauthorized map access attempt");
                    return;
                }

                var data = await _tacticalDataService
                    .GetTacticalDataAsync()
                    .ConfigureAwait(false);

                // UI 스레드에서 컬렉션 업데이트
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    Units.Clear();
                    foreach (var unit in data)
                    {
                        Units.Add(unit);
                    }
                });

                _logger.LogInformation("Tactical data loaded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load tactical data");
                // Error handling...
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand(CanExecute = nameof(CanSelectUnit))]
        private void SelectUnit(TacticalUnit unit)
        {
            SelectedUnit = unit;
            _logger.LogAudit($"Unit selected: {unit.Id}");
        }

        private bool CanSelectUnit() => !IsLoading;

        public TacticalMapViewModel(
            ITacticalDataService tacticalDataService,
            ISecurityService securityService,
            ILogger<TacticalMapViewModel> logger)
        {
            _tacticalDataService = tacticalDataService;
            _securityService = securityService;
            _logger = logger;
        }
    }
}
```

## View 표준 패턴 (XAML)

```xaml
<UserControl x:Class="TacticalCommand.Presentation.Views.TacticalMapView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:vm="clr-namespace:TacticalCommand.Presentation.ViewModels"
             xmlns:i="http://schemas.microsoft.com/xaml/behaviors"
             d:DataContext="{d:DesignInstance Type=vm:TacticalMapViewModel}">
  
    <UserControl.Resources>
        <Style x:Key="TacticalButtonStyle" TargetType="Button">
            <!-- 방산 UI 표준 스타일 -->
        </Style>
    </UserControl.Resources>

    <Grid>
        <!-- Loading Indicator -->
        <ProgressBar IsIndeterminate="True"
                     Visibility="{Binding IsLoading, 
                                  Converter={StaticResource BoolToVisibilityConverter}}"
                     Height="4"
                     VerticalAlignment="Top" />

        <!-- Main Content -->
        <DockPanel>
            <!-- Toolbar -->
            <ToolBar DockPanel.Dock="Top">
                <Button Content="데이터 로드"
                        Command="{Binding LoadDataCommand}"
                        Style="{StaticResource TacticalButtonStyle}" />
                <Separator />
                <TextBlock Text="{Binding Title}"
                           FontWeight="Bold"
                           Margin="10,0" />
            </ToolBar>

            <!-- Tactical Map Display -->
            <ItemsControl ItemsSource="{Binding Units}"
                          VirtualizingPanel.IsVirtualizing="True"
                          VirtualizingPanel.VirtualizationMode="Recycling">
                <ItemsControl.ItemTemplate>
                    <DataTemplate>
                        <Border BorderBrush="Navy" BorderThickness="1" Margin="2">
                            <StackPanel>
                                <TextBlock Text="{Binding Name}" FontWeight="Bold" />
                                <TextBlock Text="{Binding Status}" />
                            </StackPanel>
                        </Border>
                    </DataTemplate>
                </ItemsControl.ItemTemplate>
            </ItemsControl>
        </DockPanel>
    </Grid>
</UserControl>
```

## 성능 최적화 패턴

### 1. 가상화 (Virtualization)

```xaml
<!-- 대용량 리스트 처리 -->
<ListBox ItemsSource="{Binding LargeDataSet}"
         VirtualizingPanel.IsVirtualizing="True"
         VirtualizingPanel.VirtualizationMode="Recycling"
         VirtualizingPanel.CacheLengthUnit="Item"
         VirtualizingPanel.CacheLength="20,20">
</ListBox>
```

### 2. 업데이트 배치 처리

```csharp
// ❌ 나쁜 예: 개별 업데이트
foreach (var item in newItems)
{
    Units.Add(item); // UI 스레드 여러 번 업데이트
}

// ✅ 좋은 예: 배치 업데이트
await Application.Current.Dispatcher.InvokeAsync(() =>
{
    using (var deferral = Units.DeferRefresh())
    {
        Units.Clear();
        foreach (var item in newItems)
        {
            Units.Add(item);
        }
    } // 여기서 한 번만 UI 업데이트
});
```

### 3. WeakEventManager 사용

```csharp
// 메모리 누수 방지
public class SensorDataViewModel : ObservableObject
{
    public SensorDataViewModel(ISensorService sensorService)
    {
        // ❌ 강한 참조로 메모리 누수
        // sensorService.DataReceived += OnDataReceived;

        // ✅ 약한 참조 사용
        WeakEventManager<ISensorService, DataReceivedEventArgs>
            .AddHandler(sensorService, nameof(sensorService.DataReceived), 
                        OnDataReceived);
    }
}
```

## 스레딩 패턴

### UI 업데이트

```csharp
// ConfigureAwait(false) 적극 사용
public async Task<TacticalData> GetDataAsync()
{
    // 백그라운드에서 실행
    var rawData = await _repository
        .FetchDataAsync()
        .ConfigureAwait(false);
  
    var processedData = ProcessData(rawData);
  
    // UI 업데이트 시에만 Dispatcher 사용
    await Application.Current.Dispatcher.InvokeAsync(() =>
    {
        CurrentData = processedData;
    });
  
    return processedData;
}
```

## Dependency Property (커스텀 컨트롤)

```csharp
public class TacticalMapControl : Control
{
    // Dependency Property 정의
    public static readonly DependencyProperty ZoomLevelProperty =
        DependencyProperty.Register(
            nameof(ZoomLevel),
            typeof(double),
            typeof(TacticalMapControl),
            new FrameworkPropertyMetadata(
                1.0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnZoomLevelChanged,
                CoerceZoomLevel),
            ValidateZoomLevel);

    public double ZoomLevel
    {
        get => (double)GetValue(ZoomLevelProperty);
        set => SetValue(ZoomLevelProperty, value);
    }

    private static void OnZoomLevelChanged(
        DependencyObject d, 
        DependencyPropertyChangedEventArgs e)
    {
        var control = (TacticalMapControl)d;
        control.UpdateMapZoom((double)e.NewValue);
    }

    private static object CoerceZoomLevel(DependencyObject d, object value)
    {
        var zoom = (double)value;
        return Math.Max(0.1, Math.Min(10.0, zoom));
    }

    private static bool ValidateZoomLevel(object value)
    {
        var zoom = (double)value;
        return zoom > 0;
    }
}
```

## Attached Behavior

```csharp
public static class TextBoxBehaviors
{
    // 숫자만 입력 가능한 TextBox
    public static readonly DependencyProperty NumericOnlyProperty =
        DependencyProperty.RegisterAttached(
            "NumericOnly",
            typeof(bool),
            typeof(TextBoxBehaviors),
            new PropertyMetadata(false, OnNumericOnlyChanged));

    public static bool GetNumericOnly(DependencyObject obj)
        => (bool)obj.GetValue(NumericOnlyProperty);

    public static void SetNumericOnly(DependencyObject obj, bool value)
        => obj.SetValue(NumericOnlyProperty, value);

    private static void OnNumericOnlyChanged(
        DependencyObject d, 
        DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBox textBox)
        {
            if ((bool)e.NewValue)
            {
                textBox.PreviewTextInput += OnPreviewTextInput;
            }
            else
            {
                textBox.PreviewTextInput -= OnPreviewTextInput;
            }
        }
    }

    private static void OnPreviewTextInput(
        object sender, 
        TextCompositionEventArgs e)
    {
        e.Handled = !IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string text)
        => text.All(char.IsDigit);
}
```

```xaml
<!-- 사용 -->
<TextBox local:TextBoxBehaviors.NumericOnly="True" />
```

## 보안 고려사항

### 1. 민감정보 바인딩

```csharp
// ❌ 나쁜 예: 평문 비밀번호 바인딩
public string Password { get; set; }

// ✅ 좋은 예: SecureString 사용
[ObservableProperty]
private SecureString _securePassword;

// PasswordBox는 바인딩 대신 이벤트 사용
private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
{
    var passwordBox = (PasswordBox)sender;
    SecurePassword = passwordBox.SecurePassword;
}
```

### 2. 감사 로깅

```csharp
[RelayCommand]
private void ExecuteCriticalAction(object parameter)
{
    _logger.LogAudit(
        $"User {_securityService.CurrentUser} " +
        $"executed critical action at {DateTime.UtcNow}");
  
    // 실제 작업...
}
```

## 참고 자료

* Microsoft WPF Docs: https://docs.microsoft.com/wpf
* MVVM Toolkit: https://aka.ms/mvvmtoolkit
* 방산 UI 가이드: @docs/standards/ui-guidelines.md

```

---

## 🔒 보안 감사 Subagent

**.claude/agents/security-auditor.md:**

```markdown
---
name: security-auditor
description: 방산 시스템 보안 감사 전문가
tools:
  - Read
  - Grep
  - Glob
disallowedTools:
  - Write
  - Edit
  - Bash
model: opus
permissionMode: manual-approve
maxTurns: 20
---

# Security Auditor (보안 감사관)

당신은 방산 소프트웨어 보안 전문가입니다.

## 감사 항목

### 1. 하드코딩된 자격증명 검색
```regex
- password\s*=\s*['"]\w+['"]
- apikey\s*=\s*['"]\w+['"]
- secret\s*=\s*['"]\w+['"]
- connectionstring\s*=\s*['"]\w+['"]
```

### 2. 취약한 암호화 검색

* DES, 3DES 사용
* MD5, SHA1 해싱
* 하드코딩된 암호화 키
* 약한 랜덤 생성 (Random 대신 RandomNumberGenerator)

### 3. SQL Injection 취약점

```csharp
// 위험 패턴 검색
string.Format("SELECT * FROM {0}", userInput)
$"DELETE FROM Users WHERE Id = {userId}"
```

### 4. 민감정보 로깅

```csharp
// 금지된 패턴
Logger.Info($"Password: {password}")
Logger.Debug($"API Key: {apiKey}")
```

### 5. 불안전한 역직렬화

* BinaryFormatter 사용
* XmlSerializer with untrusted data

### 6. 권한 체크 누락

```csharp
// 모든 critical 작업에 권한 체크 필요
public async Task DeleteCriticalData(int id)
{
    // ❌ 권한 체크 없음!
    await _repository.DeleteAsync(id);
}
```

## 보고서 형식

```markdown
# 보안 감사 보고서

## 날짜: [현재 날짜]
## 감사 범위: [파일 경로들]

---

## 🔴 CRITICAL (즉시 수정 필수)

### [파일명:라인] 하드코딩된 비밀번호
**위치**: `Services/AuthService.cs:45`
**코드**:
\`\`\`csharp
private const string AdminPassword = "admin123";
\`\`\`
**위험도**: CRITICAL
**영향**: 
- 소스코드 노출 시 전체 시스템 장악 가능
- 기밀 정보 유출
**해결방안**:
\`\`\`csharp
var adminPassword = _configuration.GetSecureValue("Admin:Password");
\`\`\`

---

## 🟠 HIGH (우선 수정 권장)

### [파일명:라인] SQL Injection 취약점
...

---

## 🟡 MEDIUM (개선 권장)

### [파일명:라인] 취약한 난수 생성
...

---

## 🟢 LOW (참고사항)

...

---

## ✅ 통과 항목

- 모든 API 엔드포인트에 인증 적용됨
- HTTPS 강제 사용
- 입력 검증 적절히 수행

---

## 📊 요약

| 심각도 | 발견 건수 |
|--------|----------|
| Critical | 2 |
| High | 5 |
| Medium | 8 |
| Low | 3 |

**전체 보안 점수**: 65/100 (개선 필요)
```

## 참조 문서

* OWASP Top 10
* CWE/SANS Top 25
* 방산 보안 가이드: @docs/standards/security-checklist.md

```

---

## 🪝 보안 체크 Hook

**.claude/hooks/security-scan.ps1:**

```powershell
#!/usr/bin/env pwsh

Write-Host "🔒 Security Scan Starting..." -ForegroundColor Cyan

$criticalIssues = 0
$highIssues = 0

# 1. 하드코딩된 자격증명 검사
Write-Host "`n[1/5] Checking for hardcoded credentials..."
$patterns = @(
    'password\s*=\s*["'']',
    'apikey\s*=\s*["'']',
    'connectionstring\s*=\s*["'']'
)

foreach ($pattern in $patterns) {
    $results = Get-ChildItem -Path src -Recurse -Include *.cs | 
        Select-String -Pattern $pattern -CaseSensitive
  
    if ($results) {
        $criticalIssues += $results.Count
        Write-Host "  ❌ Found hardcoded credentials:" -ForegroundColor Red
        $results | ForEach-Object {
            Write-Host "     $($_.Path):$($_.LineNumber)" -ForegroundColor Yellow
        }
    }
}

# 2. 취약한 암호화 검사
Write-Host "`n[2/5] Checking for weak cryptography..."
$weakCrypto = @('DESCryptoServiceProvider', 'TripleDESCryptoServiceProvider', 'MD5CryptoServiceProvider')

foreach ($crypto in $weakCrypto) {
    $results = Get-ChildItem -Path src -Recurse -Include *.cs | 
        Select-String -Pattern $crypto
  
    if ($results) {
        $highIssues += $results.Count
        Write-Host "  ⚠️  Found weak cryptography ($crypto):" -ForegroundColor Yellow
        $results | ForEach-Object {
            Write-Host "     $($_.Path):$($_.LineNumber)"
        }
    }
}

# 3. SQL Injection 취약점 검사
Write-Host "`n[3/5] Checking for SQL injection vulnerabilities..."
$sqlPatterns = @(
    'string\.Format.*SELECT',
    '\$".*SELECT.*\{',
    '\$".*DELETE.*\{'
)

foreach ($pattern in $sqlPatterns) {
    $results = Get-ChildItem -Path src -Recurse -Include *.cs | 
        Select-String -Pattern $pattern
  
    if ($results) {
        $criticalIssues += $results.Count
        Write-Host "  ❌ Potential SQL injection:" -ForegroundColor Red
        $results | ForEach-Object {
            Write-Host "     $($_.Path):$($_.LineNumber)" -ForegroundColor Yellow
        }
    }
}

# 4. 민감정보 로깅 검사
Write-Host "`n[4/5] Checking for sensitive data in logs..."
$logPatterns = @(
    'Log.*password',
    'Log.*apikey',
    'Log.*secret'
)

foreach ($pattern in $logPatterns) {
    $results = Get-ChildItem -Path src -Recurse -Include *.cs | 
        Select-String -Pattern $pattern -CaseSensitive:$false
  
    if ($results) {
        $highIssues += $results.Count
        Write-Host "  ⚠️  Sensitive data in logs:" -ForegroundColor Yellow
        $results | ForEach-Object {
            Write-Host "     $($_.Path):$($_.LineNumber)"
        }
    }
}

# 5. TODO/FIXME 보안 관련 검사
Write-Host "`n[5/5] Checking for security TODOs..."
$todoResults = Get-ChildItem -Path src -Recurse -Include *.cs | 
    Select-String -Pattern "TODO.*security|FIXME.*security" -CaseSensitive:$false

if ($todoResults) {
    Write-Host "  ℹ️  Found security TODOs:" -ForegroundColor Cyan
    $todoResults | ForEach-Object {
        Write-Host "     $($_.Path):$($_.LineNumber): $($_.Line.Trim())"
    }
}

# 결과 요약
Write-Host "`n" + ("=" * 60) -ForegroundColor Cyan
Write-Host "Security Scan Summary" -ForegroundColor Cyan
Write-Host ("=" * 60) -ForegroundColor Cyan
Write-Host "Critical Issues: $criticalIssues" -ForegroundColor $(if ($criticalIssues -gt 0) { "Red" } else { "Green" })
Write-Host "High Issues: $highIssues" -ForegroundColor $(if ($highIssues -gt 0) { "Yellow" } else { "Green" })
Write-Host ("=" * 60) -ForegroundColor Cyan

# 실패 조건
if ($criticalIssues -gt 0) {
    Write-Host "`n❌ CRITICAL security issues found! Commit blocked." -ForegroundColor Red
    exit 1
}

if ($highIssues -gt 5) {
    Write-Host "`n⚠️  Too many HIGH security issues. Please review." -ForegroundColor Yellow
    exit 1
}

Write-Host "`n✅ Security scan passed!" -ForegroundColor Green
exit 0
```

---

## 🎯 실전 워크플로우

### 시나리오 1: 새 화면 개발

```powershell
# Claude Code 시작
claude

# Claude에게:
"새로운 '표적 추적' 화면을 만들어야 해.
MVVM 패턴으로 WPF View와 ViewModel을 생성하고,
실시간 표적 데이터를 지도에 표시해야 해.

먼저 다음을 확인해줘:
1. @docs/architecture/CLAUDE/wpf-architecture.md 읽기
2. @.claude/skills/wpf-mvvm/SKILL.md 패턴 따르기
3. @.claude/skills/gis-mapping/SKILL.md 지도 렌더링 방법

그 다음 구현 계획을 세워줘."

# Claude는 Plan Mode에서 계획 수립

# 계획 승인 후:
"좋아, 구현해줘. 
단, 보안 요구사항을 반드시 고려해:
- 모든 표적 데이터 접근에 권한 체크
- 민감정보 로깅 금지
- UI 스레드 블로킹 금지"

# 구현 완료 후 보안 감사:
"/agents security-auditor
방금 작성한 코드를 보안 감사해줘"

# 테스트 생성:
"/agents test-generator
TargetTrackingViewModel의 단위 테스트를 작성해줘.
특히 권한 체크와 에러 처리를 중점적으로 테스트해"
```

### 시나리오 2: 성능 문제 해결

```powershell
claude

"TacticalMapView에서 1000개 이상의 유닛을 표시할 때
렌더링이 느려지는 문제가 있어.

@.claude/skills/wpf-mvvm/SKILL.md의 성능 최적화 섹션을 참고해서
문제를 분석하고 해결해줘."

# Claude 분석 및 수정:
# - VirtualizingPanel 적용
# - UpdateSourceTrigger 최적화
# - 배치 업데이트 구현

# 성능 검증:
"성능 벤치마크를 실행해서 개선 효과를 측정해줘.
목표는 60 FPS 유지야."
```

### 시나리오 3: 레거시 코드 리팩토링

```powershell
claude

"LegacyMapControl.cs 파일이 2000 라인이 넘어서
유지보수가 어려워. 

다음 순서로 리팩토링해줘:
1. 코드 분석 및 책임 분리
2. SOLID 원칙 적용
3. 단위 테스트 작성
4. 점진적 리팩토링

단, 기존 기능은 절대 깨지면 안 돼."

# Claude의 안전한 리팩토링:
# - Extract Method
# - Extract Class
# - 각 단계마다 테스트 실행
# - 문서 업데이트
```

---

## ✅ 방산 프로젝트 체크리스트

### 프로젝트 초기 설정

* [ ] CLAUDE.md 작성 (보안 등급 명시)
* [ ] SECURITY.md 작성
* [ ] .claudeignore 설정 (민감 파일 제외)
* [ ] 보안 체크 Hook 설정
* [ ] MIL-STD 문서 템플릿 준비

### 개발 프로세스

* [ ] 모든 PR에 2명 이상 코드 리뷰
* [ ] 보안 감사 통과 필수
* [ ] 단위 테스트 85% 이상
* [ ] 성능 벤치마크 통과
* [ ] 문서 업데이트

### 보안 요구사항

* [ ] 모든 자격증명 환경변수/보안저장소
* [ ] AES-256 이상 암호화
* [ ] 모든 민감 작업 감사 로그
* [ ] 입력 검증 철저
* [ ] 권한 체크 누락 없음

### 품질 기준

* [ ] 코딩 컨벤션 100% 준수
* [ ] 정적 분석 0 warning
* [ ] 메모리 누수 0
* [ ] 성능 기준 충족
* [ ] 접근성 기준 충족

---

방산 시스템은 **보안과 품질이 생명**입니다. Claude Code를 사용하더라도 모든 출력은 반드시 검증하고, 보안 감사를 거쳐야 합니다.

더 구체적인 시나리오나 특정 문제에 대한 가이드가 필요하시면 말씀해주세요! 🎖️
