# Git 명령어 모음

## git init

* git init : 깃 초기화 `.git` 파일이 생성된다.

---

## git clone

* git clone [원격저장소] : 원격저장소 불러오기

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
* git reset --soft HEAD^ 이전 커밋 취소, 해당파일은 워킹 디렉토리에 보관

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
* git show [커밋아이디] : 특정 commit 정보를 확인
* git show [branch명] : 특정 branch의 가장 최근 commit 정보를 확인

---

## git reset [Commit 취소]

* git reset --soft HEAD^ : commit을 취소하고 해당 파일은 스테이징 영역에 보존
* git reset --mixed HEAD^ : commit을 취소하고 해당파일을 스테이징 영역 보준 안함. 수정 파일 삭제는 X
* git reset --hard HEAD^ : commit을 취소하고 해당 파일들의 변경점 삭제 **수정사항 파일까지 삭제된다 주의 하자**

---

## git remote [원격 저장소 관리]

* git remote : 원격 저장소 이름 확인
* git remote -v : 설정된 원격 저장소 보기
* git remote add [저장소명] [원격저장소주소]
* git remote remove [저장소명]

---

## git push [원격 저장소에 변경알림]

* git push [저장소명] [브렌치명] : 기본사용법 그냥 안써도 현재 푸시됨
* git push -u [저장소명] [브렌치명] : 최초 1회 저장소, branch 지정 이 후 생략가능
* git push --set-upstream [저장소명] [브런치명]
* git push [원격 저장소 이름] -d [원격 브랜치 이름] : 원격 브렌치 삭제

---

## `git branch`

* git branch : 로컬 branch 목록 확인
* git branch -a : 원격을 포함한 모든 branch목록 확인
* git branch [만들고싶은브런치명] : 브런치를 만든다 로컬
* git branch -d [브런치명] : 브런치 삭제
* git branch -D [브런치명] : 브런치 강제 삭제

---

## git checkout [브런치 변경]

* git checkout [브런치명] : 브런치를 이동한다.
* git checkout -t [저장소명/브런치명] : remote 브런치로 체크아웃한다.

---

## git switch [브런치 변경]

* checkout 에서 복언하느 기능을 제거
* git switch [브런치명] : 브런치를 이동한다.

---

## git fetch [원격 저장소 데이터 가져온다]

* git fetch [저장소명] : 저장소 명에 있는 데이터를 가져옴
* git fetch --prune : 원격 저장소에서 삭제된 브런치를 로컬에서도 삭제

---

## git pull [브런치 상태 떙겨오기]

* git pull --all : 원격저장소의 데이터를 로컬로 가져온후 병합한다.
* git pull [branch명] : 해당 브런치를 pull 한다.
* git pull origin main --allow-unrelated-histories 관련 기록이 없는 병합시

---

## git stash [작업중인 변경점 임시 저장 및 불러오기]

* git stash save [저장할이름] :현재 변경점 저장하기 저장되고 이전 작성은 삭제된다.
* git stash list : stash 목록 확인하기
* git stash apply [저장이름] : 저장잉름의 변경점 불러오기
* git stash apply [저장이름] : --index : 저장이름 불러와 적용하는데 Staged 상태까지 적용
* git stash pop : 가장최근 stash가져와 적용하고 스택에서 삭제
* git stash drop : 가장최근 stash 삭제

---

## git blame[특정파일 수정 이력 확인]

* git blame [파일이름] : 파일이름의 수정 이력을 확인
* git blame -L [시작 라인, 끝라인] [파일이름] : 파일이름의 시작라인부터 끝라인까지 만 확인
* git blame -C [파일이름] : 파일 이름이 변경시 변경전 파일명 확인
* git blame -w [파일이름] : 공백 변경 무시

---

## git revert [커밋 되돌리기]

* git revert [커밋아이디] : 특정 커밋으로 되돌린다.
* git revert [테그명] : 특정 태그로 되돌리고 커밋
* git revert [커밋아이디] -n : 특정 커밋으로 되돌리진않지만 커밋 안한채로 Staging 상태

---

## git restore [특정 파일 되돌리기]

* git restore [파일명] : 파일이 최근 커밋으로 되돌린다.
* git restore --source [커밋아이디] : 커밋 아이디 시점으로 해당 파일을 복구한다.
* git restore --staged [파일] : 특정 파일 staged 취소

---

## git tag [특정 커밋에 표기하는 기능]

* Lightweight 태그
  * 단순히 버전등의 이름을 남길 때 사용.
* Annotated 태그
  * 만든 사람 이름, 이메일, 날짜 메시지 까지 저장
* git tag [v1.0.0] : 현재 HEAD에 v1.0.0 이라는 Lightweight 태그생성
* git tag -a [v1.0.0]-m [메세지] : 현재 HEAD에 v1.0.0 이라는 Annotated 태그생성
* git tag v1.0.0 [커밋아이디] : 특정 커밋에 v1.0.0이라는 Lightweight 태그 생성
* git push [저장소 이름] v1.0.0 : v1.0.0 태그를 원격 저장소에 푸시하기
* git push origin --tags : 모든 로컬 태그를 원격 저장소에 푸시하기
* git tag -d v1.0.0 : 로컬의 v1.0.0 삭제
* git push -d [저장소 이름] v1.0.0 :원격 저장소의 v1.0.0 태그 삭제

---

## `git merge`

* git merge [합칠브런치명] : 메인 브렌치로 이동후 합칠 브런치와 합친다.
* git merge --abort : 병합 충돌 발생시 취소
* git merge -Xignore-all-sapce : 공백으로 인한 병합 충돌을 무시하고 병합
* git merge --no-ff feature/login : feature 이력을 합쳐서 병합한다.

---

## git rebase

Merge와는 다르게 이름 그대로 브런치의 공통 조상이 되는 base를 다른 브런치의 커밋 지점으로 바꾸는것이다.

1. 수정한 브런치로 체크아웃을 한다.
2. > git rebase master
   >
3. 그럼 완료?

* 모든 충돌을 수동으로 해결해주고, git rebase --continue를 입력해줘
* 이 커밋을 건너뛰려면 git rebase --skip을 입력해
* 중단하고 이전 상태로 돌아가려면 git rebase --abort를 입력해

---

## 브랜치 병함 Merge VS Rebase

Merge의 경우 히스토리란 작업한 내용의 사실을 기록한것이다. Merge로 브런치를 병합하게되면 커밋내역에 Merge commit이 추가로 남는다. 따라서 Merge를 사용하면 브런치가 생기고 병합되는 모든 작업 내용을 그대로 기록하게 된다.

Rebase 경우에는 히스토리를 깔끔하게 유지하기 위해 사용한다.
또한 Merge Commit을 남기지 않으므로, 마치 다른 브랜치는 없던것 처럼 프로젝트의 작업내용이 하나의 흐름을유지된다.
