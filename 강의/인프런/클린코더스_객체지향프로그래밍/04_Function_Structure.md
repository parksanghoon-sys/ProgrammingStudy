# Function Structure

---

### Arugument (함수 및 클래스 인자값 규칙)

* 인자가 많아지면 복잡도가 증가
* 3개의 인자가 최대로 설정하는 것이 좋다.
* 여러인자가 사용될때는 클래스를 만들어서 넘기자
* 생성자에 인자를 많이 주입할때는 생성자의 빌더 패턴을 이용하거나  getter, setter 을 이용하여 그냥 Set 을 넣어주자.
* Boolean 인자 사용금지
  * 2가지 이상이 일을 하는것일떄 2개의 함수로 분리하자.
* Innies not Outes
  * Output 인자를 사용하지 말자, 즉, argument로 들어와서 argument가 변경되어 사용이 되어지지말자
  * Argument는 함수로 전달되는것이지 함수로 부터 변경이 되어 나오는것이라 생각하지 말자.
  * out argument 대신 return value로 처리
* Null defense
  * Null을 전달/ 기대하는 함수 즉 null 조사 if != null
  * defensive progrmming을 지양하라
    * Null 체크로 코드를 더럽히지 말자
    * 단위 테스트에서 검증해야한다.
  * public api의 경우에는 defensie 하게 해도 괜찮다.

---

### The Stepdown Rule

* 모든 public 은 위에, 모든 private는 아래에
* public part만 사용자에게 저달하면 됨
* 중요한부분은 위로, 상세한 부분은 밑으로
* Top down으로 읽을 수 있어야 한다, 상위 메소드가 위로가자

 C# 필드·속성 배치 정리 (propfull 포함)

* 클래스 내부는 **일관된 선언 순서** 유지
  1. `const`, `static` 필드
  2. 인스턴스 private 필드
  3. 생성자
  4. public 속성
  5. public 메서드
  6. protected/internal/private 메서드
* **propfull(backing field) 규칙**
  * 필드는 해당 속성 바로 위에 둔다
  * `_camelCase` 네이밍 사용
  * propfull은 검증·이벤트·처리 로직 필요할 때만
* **접근 제한자 정렬**
  * `public → protected → internal → private` 순서 권장
* **지역 변수**
  * 사용하는 지점 가까이에 선언
* **전역 상태**
  * C#에는 없음 → `static`은 최소 사용
* **목표**
  * 공개 API는 위쪽, 구현 세부사항은 아래쪽
  * 필드는 상단에 정리해 클래스 상태를 빠르게 파악 가능

요약하면

**“외부에 보여줄 것 위로, 내부 구현 아래로, propfull은 필드와 붙여서.”**

---

### Switches and cases

* Switch 문장을 꺼리자
* 객체지향을 사용하여 interface 로 사용시 이점
  * runtime 의존성은 그대로 둔체 source code의 의존성을 역전 시킨다 (DI)
  * 본래의 의존성을 제거
  * 모듈 A는 인터페이스에 의존하고 모듈 B는 인터페이스로 부터 Derive한다
  * B의 Souce code 의존성은 runtime 의존성과 반대가 된다
  * 독립적인 배포가 가능하다, 독립적으로 컴파일도 가능, 유닛테스트도 가능해진다.
* Swich 문장은 독립적인 배포에 방해가 된다.
  * 각 case 문장은 외부 모듈에 의존성을 갖는ㄴ다
  * 다수의 다른 모듈에 의존성을 갖는다.
  * Switch 문장에서 souce code 의존성은 flow of control 방향이 같다.
    * 모든 외부 모듈에 영향을 받는다.
    * 외부 모듈중 하나라도 변경이 일어나면 switch 문장이 영향을 받는다.
    * 독립적인 배포를 불가능하게 하는 의존성을 만든다.
* switch 문장 제거 절차
  * swich 문장을 interface 호출로 변환한다.
  * case에 있는 문장을 별도의 클래스로 호출하여 변경 영향이 발생하지 않도록 한다.

### 함수들이 순서를 지키며 호출되어야 한다.

* 메소드들이 순서를 지키게 되어야한다.
  * ex) file open, excute, close

### CQS

* Command는 상태의 변경을 유발
  * 아무것도 반환하지 않는다.
  * Side effect를 갖는다
* query 는 상태의변경은 없어야한다.
  * 계산 값이나 시스템상태를 반환
  * side effect가 없다.
* 상태를 변경하는 함수는 값을 반환해선 안된다
* 값을 반환하는 함수는 상태를 변겨하면 안된다.

### Tell Don't Ask

* Command 와 Query를 함께 사용하지 말도록한다.
* 책임을 갖는 객체가 모든 처리를 하는것이 좋다.
  * 예를 들면 로그인 되었는지 아닌지 아는것은 user 객체이다.
* 내상태를 다른 객체에게 준다면 다른객체가 어떻게 처리할지 모르기 떄문에 피해야한다.
* 쿼리 메시지는 줄이는게 좋다.

### Early returns

* early return 이나 guared return 은 허용
* 루프의 중간에서 리턴은 문제이다.
  * break, 루프 중간에서의 return은 loop를 복잡하게함
* 코드가 동작하도록 하는 것보다 이해할 수 있게하는 것이 더 중요

### Error Handling

* 에러 처리를 위해 pop은 null을 반환하고,  push는 false를 반환할 수있다.

### Null is not an a error

* top를 호출하는 사람은 아무도 null을 기대하지 ㅇ낳는다.
* null 을 반환하다기 보다는 exception을 발행하여 오류를 발행하자

### Null is value

* null 값은 적절한 값일  수 있다.
  * 어떨 때는 find가 실패할 것임을 기대하기도 한다.

### Try도 하나의 역할./ 기능이다.

* try 도 그앤에 한문장만 있어야한다.
* 함수내에 try문이 있다면 try는 첫번째이다.
* finally는 반드시 마지막이여야한다.
