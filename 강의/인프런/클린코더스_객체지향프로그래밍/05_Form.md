# Foms

코딩 Standard 가 필요하다

## Comment should be Rare

코딩의 표준이 커멘트 작성을 강요한다면 이건 무의미하다.

커맨트는 특별한 경우에 작성되어야하고, 프로그램의 의도에 의해 반드시 필요할때, 그 문서를 읽는 모든 사람들이 감사해야 한다.

## Comment are Failures

작성자의 의도가 잘 나타나게 프로그램을 작성한다면 커멘트가 불필요함

코드의 의도를 나타낼수 있는가

내코드가 표현을 잘하지못하는것을 상징한다.

## Good Comment

* 저작권 관련
* 정보 적인것 예를들면 정규식 매칭된다는거
* 경고성 문구
* TODO 커맨드
* Public API 문서

## Bad Comment

* 중얼거리는 주석
* 중복 설명적인 주석

## Vertical Formatting

* 공란을 함부로 사용하지 마라
  * 메소드 사이
  * private 메서드와 public 변수 사이
  * 변수 선언과 실행 나머지 사이
* 서로 관련된것은 수직적으로 작성하는 것이 좋다.

## Classes

* private 변수를 작성함으로써 클래스르 작성한다. 그리고 그 private 변수들을 public 함수들로 조작한다, 외부에서는 private 변수들이 없는 것처럼 보인다.
* 왜 변수는 private 로 선언하고 getter/setter를 제공하는가
* Tell Don't Ask
  * 객체가 관찰 상태가 아니면, 무엇을 하라고 시키기 쉽고 요청할 가치가 없다
  * getter가 많지 않게하면 setter도 많지 않다.
* getter/ setter는 응집도가 낮다
  * 하나의 변수만 사용되기 떄문에
  * getter/setter는 최소화한다 그래야 응집도를 높인다
  * getter를 쓸때 본래변수를 노출하지 마라
* 상세 구현 노출을 줄이자.
* 객체 지향은 타입에 확장성은 높으나 코드 변경에는 어려움이 크다.

## The Impedance Mismatch

* OOP에서 RDB를 사용시 ㅂ발생하는 일련의 개념적/기술적 어려움

  * 객체나 클래스의 정의가 데이터 베이스  테이블이나 스키마에 직접 매핑될때 발생함
* Db Table은 DataStructure 이다.

  * data를 노출하고, 메소드는 없다
* DB는 도메인 객체, 비지니스 객체, 어떤 객체도 포함 할 수없다, 오직 DS 객체만 포함한다.
* ORM

  * DB row와 객체간의 직접적인 매핑이 없기에 object-relational mapper는 아니다.
    * 하나는 DS이고 하나는 객체이기 때문이다.
  * 이런 orm 객체는 DS로의 매퍼이다.
  * RDB 테이블 정보로를 메모리의 정보로 변경한것이다.
* DB 는 앱을 위해 존재하지 않는다. Enterprise를 위해 최적화 되어 있다., 즉 여러 요구사항을 위해 사용된다.
* App boundary 측면에서는 enterprise 스키마를 실제 객체 설계를 통해 분리가 가능하다.

  * 이렇게해야 테이블 row를 조작하는 대신 비지니스 객체를 조작함으로써 어플을 보다 자연스롭게하고 이해하기 쉽게한다.
