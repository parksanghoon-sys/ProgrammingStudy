# 명령어 모음
## git init
* git init : 깃 초기화 `.git` 파일이 생성된다.
 ---
## git status
* git status : 깃의 상태를 보여준다.
 ---
## git add
* git add -A : 모든 변경점 추가
* git add [FileName1] [FileName2] : 원하는 파일 staging 등록
* git add . : 수정한거 staging 전체 올림
---
## git rm
* git rm [파일이름] : 파일을 지우거나 스테이지에서 해제 할떄 사용
* git rm --cached [파일이름] : git 에서 파일이 추적되지 않도록한다.
---
## git commit
* git commit -m : '[내용]' : git Commit
* git commit -a : 신규 파일을 제외한 변경사항을 Staging 후 커밋
* git commit --amend : 이전 커밋 내용 변경

---
## git diff [이전 커밋과의 차이 확인]
* git diff : 수정한 파일과 수전 전의 파일 차이를 본다. 
* git difftool : 툴을 사용하여 차이를 확인한다.

---
## git log [Log 기록 보기]
* git log : 로그 기록을 확인. J,k 로 CLI 스크롤 
* git log --all --graph : 깃 모든 로그를 그래프로본다
* git log --all --online : 모든 브런치의 커밋 히스토리를 보여준다
* git log [branc이름] : 특정 브런치의 로그만 알고싶을때
* git log -S [코드내용] : 특정 코드의 수정내역을 알고싶을때

---
## git show [Commit 정보 확인]
* git show : 현재 branch의 가장 최근 commit 정보를 확인
* git show [commit해시값] : 특정 commit 정보를 확인
* git show [branch명] : 특정 branch의 가장 최근 commit 정보를 확인

---
## git reset [Commit 취소]
* git reset --soft HEAD^ : commit을 취소하고 해당 파일은 스테이징 영역에 보존
* git reset --mixed HEAD^ : commit을 취소하고 해당파일을 스테이징 영역 보준 안함. 수정 파일 삭제는 X
* git reset --hard HEAD^ : commit을 취소하고 해당 파일들의 변경점 삭제 **수정사항 파일까지 삭제된다 주의 하자**

---
## git remote [원격 저장소 관리]
* git remote -v : 설정된 원격 저장소 보기
* git remote add [저장소명] [원격저장소주소]

---
## git push [원격 저장소에 변경알림]
* git push [저장소명] [브렌치명] : 기본사용법 그냥 안써도 현재 푸시됨
* git push -u [저장소명] [브렌치명] : 최초 1회 저장소, branch 지정 이 후 생략가능
* git push --set-upstream [저장소명] [브런치명]
* git branch [만들고싶은브런치명] : 브런치를 만든다 로컬
* git switch [브런치명] : 브런치를 이동한다  `checkout`과 비슷
* git merge [합칠브런치명] : 메인 브렌치로 이동후 합칠 브런치와 합친다.
