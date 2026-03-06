Git 저장소의 커밋 내역을 메모장으로 옮기는 방법은 다음과 같습니다:

1. 명령 프롬프트(CMD) 또는 Git Bash를 열고 해당 Git 저장소 디렉토리로 이동합니다.
    
2. 다음 명령어를 사용하여 커밋 내역을 파일로 저장합니다:
    
    ```
    git log > commit_history.txt
    ```
    
3. 생성된 `commit_history.txt` 파일을 메모장에서 열면 됩니다.
    

더 상세한 커밋 내역을 원하신다면 다음과 같은 옵션을 활용할 수 있습니다:

- 간단한 형식으로 보기: `git log --oneline > commit_history.txt`
- 변경된 파일 목록 포함: `git log --stat > commit_history.txt`
- 특정 기간 내 커밋만 보기: `git log --since="2023-01-01" > commit_history.txt`
- 특정 작성자의 커밋만 보기: `git log --author="이름" > commit_history.txt`

필요한 옵션을 조합하여 원하는 형태의 커밋 내역을 추출하실 수 있습니다.