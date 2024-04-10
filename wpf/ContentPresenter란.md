# **ContentPresenter 란??**

* ## DataTemplate

  * 데이터를 어떻게 표현할것인가를 위주로 여러 Control을 조합하여 표현
  * ContentPresenter 클래스의 전용 Visual 담당, 무엇이든 상관 없는 그 Content라는 놈을 위한 Visual을 정의하기 위해 사용
* ## ControlTemplate

  * 컨트롤을 어떻게 표현할 것이냐의 차이
  * 스스로 컨트롤을 만들거나, 기존의 컨트롤 모양을 수정하지 않으면 ControlTemplete를 사용
  * TextBox, Label, Button등은 내부적으로 ControlTemplete를 사용하여 구성되어있다.
  * UI Control로써 ***사용자와 상호작용하는 Logic을 위한*** Visual을 정의 하기위해 사용
* ## ContentPresenter

  * 외부의 Content가 그자리에 놓여질떄 사용이 된다.
  * 현재 스타일을 잡은 컨트롤에서 그외 외부에서 컨트롤을 사용시 따로 사용할 예정이라는 의미?
  * ControlTemplate로 내가 어느정도 정의했다 ContentPresenter은 이것을 사용하는 개발자가 알아서 정의해서 사용하라는 의미
  * Content라는 무언가를 **표시** 될때 사

---

## Control 클래스

Control 클래스는 ControlTemplate 속성을 포함하고있다.`<br>`
ItemControl 과 ContentControl은 Control을 상속 하고 있다 그래서 이둘은 Template을 사용할수 있다.
