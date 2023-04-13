# 명령어 모음
* git add [FileName1] [FileName2] : 원하는 파일 staging 등록
* git add . : 수정한거 staging 전체 올림
* git commit -m '[내용]' : git Commit
* git diff : 수정한 파일과 수전 전의 파일 차이를 본다. 
* git difftool : 툴을 사용하여 차이를 확인한다
* git log : 로그 기록을 확인. J,k 로 CLI 스크롤 
* git log --all --graph : 깃 모든 로그를 그래프로본다
* git log --all --online : 모든 브런치의 커밋 히스토리를 보여준다
* git log [branc이름] : 특정 브런치의 로그만 알고싶을때
* git log -S [코드내용] : 특정 코드의 수정내역을 알고싶을때
* git branch [만들고싶은브런치명] : 브런치를 만든다 로컬
* git switch [브런치명] : 브런치를 이동한다  `checkout`과 비슷
* git merge [합칠브런치명] : 메인 브렌치로 이동후 합칠 브런치와 합친다.
* 