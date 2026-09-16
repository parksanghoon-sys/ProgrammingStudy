# DependencyProperty 란?

## DependencyProperty 개요

일반 속성을 static으로 등록하여 사용한다. WPF에서 UI 속성이 DependencyProperty를 사용하여 이를 통해 ViewModel과 바인딩이 가능한 프로퍼티를 구현한다. 예를 들면 DependencyProperty.Register 메서드의 ToggleButton의 IsChecked 들수 있다.

```csharp
public static readonly DependencyProperty IsCheckedProperty =
    DependencyProperty.Register(
        "IsChecked",
        typeof(bool?),
        typeof(ToggleButton),
        new FrameworkPropertyMetadata(...,
            new PropertyChangedCallback(OnIsCheckedChanged)));

public bool? IsChecked
{
    get => ...
    set => ...
}

private static void OnIsCheckedChanged(DependencyObject d,  
    DependencyPropertyChangedEventArgs e)
{
    ToggleButton button = (ToggleButton)d;
    bool? oldValue = (bool?) e.OldValue;
    bool? newValue = (bool?) e.NewValue;
    ...
}
```

## DependencyProperty 구조
크게 3개의 요소로 구성되어 있따.
1. **static DependencyProperty** : Register 등록
2. **bool? IsChecked**: Property 속성
3. **static OnIsCheckedChanged** : 콜백 메서드

콜백 메서드는 사용목적에 따라 구현 하지 않아도된다.
### Register 등록
DependencyProperty 는 sealed 접근제한자를 통해 봉인되어 있으며 생산자도 private로 제한되어있어 Register 내부에서만 DependencyProperty 인스턴스 생성이 가능하다. 생성 과정 시 `RegisterPropertyList` 라는 static 컬랙션 객체에 DependencyProperty 자신을 추가하여 DependencyProperty 수집하여 관리할수 있다.

## Property 선언
DependencyProperty 를 사용하는 프로퍼티는 내부 get, set에 의해 사용되는 field가 존재하지 않는다. 대신 `GetValue` , `SetValue` 메서드를 이용해 관리한다.
```csharp
public bool IsChecked
{
    get => (bool)GetValue(ContentNameProperty);
    set => SetValue(ContentNameProperty, value);
}
```

`GetValue()` 와 `SetVlaue()` 메서느는 `DpendencyObject` 를 통해 사용된다. 모든 WPF UI 클래스가 `DependencyObject` 를 상속 받아 어디든 DependencyProperty 를 등록 사용가능.
### 의존 속성의 개념
DependencyProperty 는 DependencyProperty 와 DependencyObject, 내부 로직에 의해 모든값이 *static 으로 관리된다*, 해당 이유로는 DP가 자신의 값이 없는경우 VisualTree 계층구조 관점에서 자기보다 상위에 위치한 가장 가까운 직계 부모의 DP값을 물려받는다. 이러한 구조로 메모리 관리에 이점이 있다. 

예를들면 WPF 컨트로에서 FontSize DependencyProperty 속성이 사용됫다 하자
```xml
<Window FontSize="15">
    <StackPanel FontSize="15">
        <Button FontSize="15"/>
        <TextBlock FontSize="15"/>
        <MyCustomControl FontSize="15"/>
    </Grid>
</Window>
```
만약 Window를 제외한 나머지 컨트롤에서 FontSIze를 제거할시 아래와 같다.
```xml
<Window FontSize="15">
    <StackPanel>
        <Button/>
        <TextBlock/>
        <MyCustomControl/>
    </Grid>
</Window>
```

이경우 각 컨트롤은 부모의 값을 참조 받기때문에 모두 동일하게 FontSize가 15로 설정도니다. 이건 DP 프로퍼티는 값을 따로 갖는것처럼 동작한다. 이게 DependencyProperty의 개념이다.

WPF 모든 컨트롤은 `DataContext DP` 속성을 가지고 있어 **자신의 DataContext를 변경하지 않는한 상위 컨트롤의 DataContext 를 사용하게된다.** 이러한 기능 덕에 WPF에서 데이터 바인딩이 강력하게 동작한다.

```xml
<Window DataContext="{Binding MyViewModel}">
    <StackPanel>
        <Button Command="{Binding MyCommand}"/>
        <TextBlock Text="{Binding MyText}"/>
        <MyCustomControl SomeProperty="{Binding MyProperty}"/>
    </Grid>
</Window>
```
위 **Window의 하위 각 컨트롤은 Window의 DataContext를 참조하게된다** 덕분에 별도의 DataContext를 설정하지 않아도 `ViewModel`의 Binding 이 가능해진다.