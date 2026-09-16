# RoutedEvent란?

CustomControl 또는 UserControl을 만들 때, 커스텀 이벤트가 필요한 경우나 상속 받은 컨트롤에 대한 이벤트를 처리할 필요가 종종 있다  
*의존성 속성에 등록된 이벤트라고 생각하면 쉽다*
컨트롤 버튼에 사용되는 Click, MouseDown 이벤트등은 RoutedEvent 예시 중 일부이다

## RountedEvent의 3가지 방식
1. **터널링**
    * 루트 요소에서 클릭했던 요소까지 순차적인 이벤트가 처리되는 방식
2. **버블링**
   * 클릭된 요소에서 루트 요소까지 순차적인 이벤트가 처리되는 방식
3. **직접**
   * 버블링과 터널링은 시작된 요소에서 끝 요소까지 이벤트가 순차적으로 처리되지만, 직접은 클릭했던 요소만 처리하는 방식을 말한다.  

RoutedEvnet는 순차적으로 이벤트가 처리되는데 중간에 이벤트를 종료시키고 싶다면 RoutedEvnetArgs개체에서 Handled를 true로 설정시 중간에 이벤트가 종료된다.  
예를들면 겹쳐진 컨트롤중 3번쨰 이벤트를 처리할때 Handled 속성을 true로 할경우 그 4,5,6...이벤트는 처리되지 않느다
## RoutedEvent 선언하기
EvnetManager 객체에 정적인 함수인 RegisterRoutedEvent를 통해서 만들어야한다 등록된 RoutedEvnet는 식별할 수 있게 이름을 가지게 된다  
### **코드구현**
```c#
public sealed class RoutedEvent
{
        public string Name { get; }
        public Type OwnerType { get; }
        public RoutedEvent AddOwner(Type ownerType);
        public override string ToString();
}

public static readonly RoutedEvent ChangedFilterEvent = 
EventManager.RegisterRoutedEvent("ChangedFilter", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FilterDataGrid));
```
### **RoutedEvent 핸들러 등록**
CustomControl내 아래 코드를 사용하여 이벤트 핸들러를 등록할 수 있다.
```c#
public event RoutedEventHandler ChangedFilter
{
  add { AddHandler(ChangedFilterEvent, value); }
  remove { RemoveHandler(ChangedFilterEvent, value); }
}
```
ChangedFilterEvnet는 이벤트를 등록한 컨트롤의 정보
ChangedFilter은 이벤트 핸들러 정보를 가진다.  
### **등록한 이벤트 호출**
예를들어 사용자가 필터를 변경시, 내부적으로 이벤트를 호출하여 시작을 알린다.  
다른곳에서 이벤트 핸들러를 추가 했다면 체인식을 호출,  
기본적으로 이벤트는 RaiseEvnet한수를 호출
```c#
RoutedEventArgs args = new RoutedEventArgs(FilterDataGrid.ChangedFilterEvent);
RaiseEvent(args);
```
### **EventTrigger & CallMethodAction**
내부적으로 이벤트를 등록 및 호출 했으니 ViewModel에서 해당 이벤트를 처리할수있다 EvetnTrigger를 이용하여 처리한다
```xml
xmlns:i="clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity"
xmlns:ei="http://schemas.microsoft.com/expression/2010/interactions"
      
<i:EventTrigger EventName="ChangedFilter">
	<ei:CallMethodAction TargetObject="{Binding}" MethodName="OnFilterChanged"/>
</i:EventTrigger>
```
ViewModel에서는 이벤트를 바인딩 받아서 처리한다.

