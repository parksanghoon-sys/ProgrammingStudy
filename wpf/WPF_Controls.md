# WPF Template란?
<br></br>
* ## Template란?
  * `틀` 의 역할을 해주는것 WPF의 <span style="color:yellow">Control의 껍데기</span> 같은 개념이다
  * 예시
    * ***Button***
      * ButtonBase
    * ContentControl 
      * DataTemplate
        ```
         데이터의 틀을 수정
    * Content
    * Control
      * ControlTemplate
        ```
        * XAML을 통해서 저 ControlTempalte에 들어갈 내용을 만들수 있다.
        * Control의 외형을 수정 하는 기능
        * TargetType 변수
          * 어떤컨트롤인지 확인하기위한 타입을 받는다.
    * ***ListView***
    * ListBox
      ```
       View는 멀티선택이 되고 BOx는 멀티선택이 안됨
    * Selector
      * Selector 클래스는 데이터 선택에 대한 기능을 담당
    * ItemControl     
       * ItemsPanelTemplate 
         > 위에 DataTemplate를 통해 데이터가 어떻게 표현될지 지정 <span style="color:yellow">아이템들이 나오는 전체적인 틀을 조정할수 있는 Template</span>
    * Control    
* ## ControlTemplate
  * 컨트롤의 이미지를 사용자 지정에 맞게 변화할수 있도록 하는것
  * 컨트롤의 컨텐츠가 아닌 컨트롤의 외관을 꾸밀떄 사용된다<br></br>
* ## DataTemplate
  * 컨트롤의 안의 리스트의 내용을 스타일링할때 사용된다.
  * 

