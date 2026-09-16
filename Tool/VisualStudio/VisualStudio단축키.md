Ctrl-K, Ctrl-H : 바로가기 설정. ( 작업목록 창에서 확인가능 )
Ctrl-K,K : 북마크 설정 / 해제
Ctrl-K,L : 북마크 모두 해제
Ctrl-K,N : 북마크 다음으로 이동
Ctrl-K,P : 북마크 이전으로 이동
Ctrl-K,C : 선택한 블럭을 전부 코멘트
Ctrl-K,U : 선택한 블럭을 전부 언코멘트(코멘트 해제)
Ctrl-F3 : 현재 단어 찾기
-> F3 : 다음 찾기

Ctrl-F7 : 현 파일만 컴파일
: 현 프로젝트만 빌드
Ctrl-Shift-B : 전체 프로젝트 빌드
Ctrl-F5 : 프로그램 시작

Ctrl-i : 일치하는 글자 연속적으로 찾기
Ctrl+i 를 누르면 하단에 자세히보면, “증분검색” 이라는 텍스트가 나온다.
그러면 그때 찾기 원하는 단어를 입력할때마다 일치하는 위치로 바로바로
이동한다. (좋은기능)
타이핑은 “증분검색” 이라는 텍스트옆에 커서는 없지만 입력이된다.
입력하는 문자를 수정하려면, backspace로, 그만 찾으려면 엔터.

줄넘버 보여주기 : 도구 > 옵션 > 텍스트편집기 > 모든언어 > 자동줄번호 선택하면 됨.

Ctrl+ – (대시문자), Ctrl+Shift+ –  :
현재 커서를 기억하는 Ctrl+F3(VS6에서), Ctrl+K,K(VS7에서) 와는 달리
사용자가 별도로 입력을 해주는건 없고, 단지 이전에 커서가 있었던곳으로
위 키를 누를 때마다 이동된다. (shift를 이용하면 역순)

Ctrl-F12 : 커서위치 내용의 선언으로 이동( 즉, 대략 헤더파일 프로토타입으로 이동)

F12 : 커서위치 내용의 정의로 이동( 즉, 대략 CPP파일 구현부로 이동)

Shift+Alt+F12 : 빠른기호찾기 ( 이거 찾기보다 좋더군요. 함수나 define등 아무거나에서 사용)

F12: 기호찾기. (s+a+f12 비교해볼것)

Ctrl-M, Ctrl-L : 소스파일의 함수헤더만 보이기 (구현부는 감추고) (토글 키)
Ctrl-M, Ctrl-M : 현재 커서가 위치한 함수를 접는다/편다. (토글 키)

#include “파일명” 에서 “파일명” 파일로 바로 직접이동
하고 싶을경우 -> Ctrl-Shift-G
<편집>—————————————————————————
Ctrl-F : 찾기 대화상자
Ctrl-H : 바꾸기 대화상자
Ctrl-Shift-F : 파일들에서 찾기 대화상자
Ctrl-Shift-H : 파일들에서 바꾸기 대화상자
Ctrl-G : 해당 줄로 가기 (별로 필요없음)
Ctrl-K,Ctrl-F : 선택된 영역 자동 인덴트 (VS6의 Alt-F8기능)
Ctrl-] :괄호({,}) 쌍 찾기 : 괄호 앞이나 뒤에서 눌러서 닫거나,
여는 괄호이동
Ctrl-Shift-Spacebar : 함수이름편집중 툴팁으로나오는 함수와매개변수설명이 안나올경우, 강제로 나오게

alt-LButton ->Drag : 원하는 영역의 블럭을 세로로 잡기

Ctrl+Shift+R (키보드 레코딩) :
가끔 연속된 연속기만으로는 부족한경우가 있다.
이럴때, 몇번의 키동작으로 레코딩하여, 이것을 반복하고 싶은경우가있다.
이때 Ctrl+Shift+R 을 누르고, 원하는 동작들을 수행후, 다시 Ctrl+Shift+R을
눌러 종료한다.  이 중간동작을 원하는 위치에서 반복하고 싶다면
Ctrl+Shift+P 를 누른다.

Ctrl+Shift+V (히스토리 붙이기) :
Ctrl + V와는 달리 클립보드에 있는 복사된내용을 돌아가면서 붙여준다.
따로 복사를 해주거나 할 필요는 없다. 그냥 Ctrl+C로 계속 원하는것을
복사하면 된다.

Ctrl-Z : 이전으로 되돌리기

Ctrl-Shift-Z : 되돌렸다, 다시 복구하기

<디버그/빌드>————————————————————————-
F5 : 디버그 시작
F9 :디버그 브렉포인트 설정
Ctrl-F9 : 현위치 설정된 브렉포인트 해제
Ctrl-Shift-F9 : 모든 브렉포인트 해
Shift-F5 : 디버그 빠져나오기
Ctrl-F10 : 커서가 있는곳까지 실행
Shift-F11 : 현 함수를 빠져나감.

Shift+Ctrl+B :  전체 빌드(프로젝트가 여러개있을경우 모두 빌드)
Alt+B, C : 해당 프로젝트만 정리.
Alt+B, U : 해당 프로젝트만 빌드.

<창관련>————————————————————————-

Shift+Alt+Enter : 전체 창 (토글 됨)
F4 : 속성창 보여준다.
Ctrl+Alt+X : 리소스에디터 툴박스창
Ctrl+Alt+K : 작업목록 창.

비주얼 스튜디오를 쓰다가 단축키를 잊어먹거나 까먹어서 잘 못쓰는 경우가 많아 정리를 해보았다.

| 단축키                           | 설명                                                              |
| -------------------------------- | ----------------------------------------------------------------- |
| Ctrl + Tab                       | Edit하고 있는 Child Window 간의 이동                              |
| Ctrl + F4                        | 현재 Edit하고 있는 Child Window를 닫기                            |
| Ctrl + I                         | 문자열 입력 점진적으로 문자열 찾기 (Incremental Search)           |
| Ctrl + F3                        | 현재 커서에 있는 문자열 찾기 fowared (블록 지정 안 해도 됨)       |
| Shift + F3                       | 현재 커서에 있는 문자열 찾기 backward                             |
| F3                               | 찾은 문자열에 대한 다음 문자열로 이동 (Next Search)               |
| Ctrl + H                         | 문자열 찾아 바꾸기 (Replace)                                      |
| Ctrl + Left/Right Arrow          | 단어 단위로 이동                                                  |
| Ctrl + Delete 또는 Backspace     | 단어 단위로 삭제                                                  |
| Ctrl + F2                        | 현재 라인에 북마크 지정/해제                                      |
| F2                               | 지정된 다음 북마크로 이동                                         |
| Ctrl + Shift + F2                | 지정된 모든 북마크를 해제                                         |
| F9                               | 현재 라인에 Breakpoint를 지정/해제                                |
| Ctrl + Shift + F9                | 현재 Edit하고 있는 소스파일에 지정된 모든 Breakpoint 해제         |
| Ctrl + ] 또는 E                  | ‘{‘괄호의 짝을 찾아줌 (‘{‘에 커서를 놓고 눌러야 함}           |
| Ctrl + J, K                      | #ifdef 와 #endif의 짝을 찾아줌                                    |
| Ctrl + L                         | 한 라인을 클립보드로 잘라내기 (Cut)                               |
| Ctrl + Shift + L                 | 한 라인을 삭제                                                    |
| Alt + Mouse                      | 블록 설정 세로로 블록 설정하기 (마우스로)                         |
| Ctrl + Shift + F8                | 세로로 블록 설정하기 (키보드로), 취소할 때는 Esc키를 눌러야 함    |
| 블록 설정 -> Tab                 | 선택된 블록의 문자열을 일괄적으로 들여쓰기(Tab) 적용              |
| 블록 설정 -> Shift + Tab         | 선택된 블록의 문자열을 일괄적으로 내어쓰기 적용                   |
| Alt + F8 -> Tab 또는 Shift + Tab | 들여쓰기 자동 조정 (Tab:들여쓰기, Shift + Tab : 내어쓰기)         |
| Ctrl + T                         | 현재 커서에 있는 변수/함수에 대한 Type이 Tooltip 힌트 창에 나타남 |
| Ctrl + Alt + T                   | 멤버 변수/함수 목록에 대한 팝업 창이 나타남                       |
| Ctrl + Shift + T                 | 공백/콤마/파이프/괄호 등을 기준으로 좌우 문자열을 Swap시킴        |
| Ctrl + Shift + 8                 | 문단기호 표시/감추기 : Tab은 ^, Space는 .으로 표시                |
| Ctrl + D                         | 툴바의 찾기 Editbox로 이동                                        |
| Ctrl + Up/Down Arrow             | 커서는 고정시키고 화면만 스크롤 시키기                            |

# 프로잭트/파일/창 생성 및 열기

> * 새프로잭트 생성
>   * **Ctrl + shift + n**
> * 새 파일 추가
>   * **Ctrl + shift + a**
> * 여러 유형의 새 파일 만들기
>   * **ctrl + n**
> * 현재 문서 닽기
>   * **ctrl + F4**
> * 기존 프로젝트 열기
>   * **ctrl + shift + o**
> * 전체 화면으로 보기
>   * **shift + alt + enter**
> * 솔루션 탐색기 열기
>   * **ctrl + alt + L**

# 편집 관련 단축키

> * 자동정렬
>   * **ctrl + k + f**
> * 현재라인 복사해서 아래라인에 붙이기
>   * **ctrl+d**
> * 현재 라인 잘리내기
>   * **ctrl + x, ctrl + L** :현재라인 잘라내기
>   * **ctrl + v:** 붙이고 싶은 위치로 이동한 후 복사~
> * 네모 박스편집 열모드편집
>   * *alt + 드래그 /  alt + shift + **↑/↓***
> * 주석처리하기
>   * **ctrl + k + c** : 주석처리
>   * **ctrl + k + u :** 주석해제
> * 대소문자 변환
>   * **ctrl + u :** 소문자변환
>   * **ctrl + shift + u :** 대문자변환
> * 함수 점고 펴기
>   * **ctrl + m + m**
> * 이름 변경

# 단축키

1. 코드정리

   * ctl  K F
2. 디자인에서 코드이동

   * F7
3. 이름바꾸기

   * Ctrl + R, R-
4. 라인 잘라내기

   * Shift + Del
5. 전체 코드정렬

   * ctrl + k + d
6. 선택영역정렬

   * ctrl +  k + f
7. 클래스만들기

   * cla + tap 2번
8. 메인함수만들기

   * svm  + tab2번
9. 생성자생성

   1. ctor +  텝2번
10. ConsoleWrite

    * cw 텝2번
11. DependencyProperty 만들기

    1. propdp + Tab 2번
12. 다중커서

    * ctrl + alt + 마우스클릭
    * alt +
      shift + 키보드방향키
13. region  펼치기, 접기

    * Ctrl  M M => 줄인거 피기
    * Ctrl M O => 줄인거 줄이기
14. 전체 펼치기

    1. Ctrl M L
15. 창이동

    1. ctrl  +-
16. 주석처리

    1. Ctrl +K C
17. 주석해재

    1. Ctrl K U
18. 리전

    1. Ctrl K S
19. 동일 문서 텍스트 전체 선택

    1. shift + alt + K
20. 동일문서 텍스트 찾기

    1. shift + alt + .
21. Dependency
    Property 만들기

    1. Ctrl+K,X -> NetFX30
       -> DependencyProperty, AttachedProperty 2개중에하나를선택하면기본골격이만들어집니다.

sudo apt**-**get update **&&** sudo apt**-**get install
    apt**-**transport**-**https
    ca**-**certificates
    curl
    software**-**properties**-**common**[출처]** [WSL2 설치 및 Docker 환경 구축](https://blog.naver.com/ilikebigmac/222007741507)|**작성자** [isnt](https://blog.naver.com/ilikebigmac)

apt-**get remove docker docker**-**engine docker**.**io**[출처]** [WSL2 설치 및 Docker 환경 구축](https://blog.naver.com/ilikebigmac/222007741507)|**작성자** [isnt](https://blog.naver.com/ilikebigmac)
