# Commit

## 좋은 커밋 메세지의 7가지 규칙

---

* 제목과 본문을 한줄 띄어 분리
* 제목은 영문 기준 50자 내외
* 제목 첫글자를 대문자로
* 제목 끝에 . 금지
* 제목은 명령조로
* Github 제목에 이슈번호 붙이기
* 본문은 연문 기준 72자 마다 줄 바꾸기
* 본문은 어떻게보다 무엇을, 왜에 맞춰 작성하기

## 커밋 메세지 구조

```
type: Subject
body
footer
```

## Type 종류

---

* feat : 새로운 기능 추가
* fix : 버그 수정
* docs : 문서수정
* style(UI변경부분): 코드 포맷팅 (세미콜론, 들여쓰기 등) , 기능외 UI만 변경시
* refactor : 코드 리펙토리
* test : 테스트 코드, 리펙토링 테스트 코드 추가 (프로덕션 코드 변경 하지 않는다)
* chore : 빌드 업무 수정, 패키지 매니저 수정
* design : 디자인 변경
* comment : 필요한 주석 추가 및 변경
* rename : 파일 혹은 폴더명을 수정하거나 옮기는 작업 만인 경우
* remove : 파일을 삭제하는 경우
* !BREAKING CHANGE : 커다란 변경의 경우
* !HOTFIX : 급하게 치명적인 버그를 고칠경우
* perf: 성능 개선
* ci: CI/CD 관련 변경

## 예시

---

fix :버그 수정(#203040)
...본문
==============================

# Commit

## 좋은 커밋 메세지의 7가지 규칙

---

* 제목과 본문을 한줄 띄어 분리
* 제목은 영문 기준 50자 내외
* 제목 첫글자를 대문자로
* 제목 끝에 . 금지
* 제목은 명령조로
* Github 제목에 이슈번호 붙이기
* 본문은 연문 기준 72자 마다 줄 바꾸기
* 본문은 어떻게보다 무엇을, 왜에 맞춰 작성하기

## 커밋 메세지 구조

```
type: Subject
body
footer
```

## Type 종류

---

* feat : 새로운 기능 추가
* fix : 버그 수정
* docs : 문서수정
* style : 코드 formatting, 세미클론 누락, 코드변경이 없는경우
* refactor : 코드 리펙토리
* test : 테스트 코드, 리펙토링 테스트 코드 추가 (프로덕션 코드 변경 하지 않는다)
* chore : 빌드 업무 수정, 패키지 매니저 수정
* design : 디자인 변경
* comment : 필요한 주석 추가 및 변경
* rename : 파일 혹은 폴더명을 수정하거나 옮기는 작업 만인 경우
* remove : 파일을 삭제하는 경우
* !BREAKING CHANGE : 커다란 변경의 경우
* !HOTFIX : 급하게 치명적인 버그를 고칠경우

## 예시

---

fix :버그 수정(#203040)
...본문

[[GitHub명령어]]

>>>>>>> ae3be484e8d676e9969f98b0ea229e3c4f51d0fb
>>>>>>>
>>>>>>
>>>>>
>>>>
>>>
>>
