# WPF 프로젝트에서 Git Submodule로 라이브러리 사용하기

Git submodule을 사용하여 WPF 프로젝트에 자체 제작 라이브러리를 통합하는 방법을 단계별로 안내해드리겠습니다.

## 1. Git Submodule 추가하기

```bash
# WPF 프로젝트 디렉토리로 이동
cd 내WPF프로젝트

# Git 저장소가 아직 초기화되지 않았다면 초기화
git init

# 라이브러리를 submodule로 추가 (Libraries 폴더에 MyLib 이름으로 추가)
git submodule add https://github.com/사용자명/내라이브러리저장소.git Libraries/MyLib

# Submodule 초기화 및 업데이트
git submodule init
git submodule update
```

## 2. WPF 프로젝트에 라이브러리 참조 추가하기

1. Visual Studio에서 WPF 프로젝트를 엽니다
2. 솔루션 탐색기에서 프로젝트를 우클릭하고 "기존 프로젝트 추가"를 선택합니다
3. Libraries/MyLib 경로에서 .csproj 파일을 선택합니다
4. WPF 프로젝트를 우클릭하고 "참조 추가"를 선택한 후 "프로젝트" 탭에서 라이브러리 프로젝트를 선택합니다

## 3. .csproj 파일에 직접 참조 추가하기 (대안)

WPF 프로젝트의 .csproj 파일을 편집하여 다음과 같이 참조를 추가할 수도 있습니다:

```xml
<ItemGroup>
  <ProjectReference Include="..\Libraries\MyLib\MyLib.csproj" />
</ItemGroup>
```

## 4. Submodule 관리

```bash
# 모든 submodule을 최신 버전으로 업데이트
git submodule update --remote

# 특정 submodule만 업데이트
git submodule update --remote Libraries/MyLib
```

# Git 서브모듈 상태 확인 방법

Git 서브모듈의 상태를 확인하는 다양한 명령어입니다:

## 기본 상태 확인

```bash
# 모든 서브모듈의 상태 확인
git submodule status
```

이 명령어는 다음과 같은 형식으로 결과를 보여줍니다:

- : 정상 상태 (공백으로 시작)
- `+`: 서브모듈 내 변경사항 존재
- `-`: 초기화되지 않은 서브모듈
- `U`: 충돌 상태

## 상세 정보 확인

```bash
# 서브모듈 변경사항 요약
git submodule summary

# 각 서브모듈 내부의 상태 자세히 보기
git submodule foreach git status
```

## 특정 서브모듈 확인

```bash
# 특정 서브모듈만 상태 확인
git submodule status [서브모듈경로]
```

## 서브모듈 설정 확인

```bash
# 등록된 모든 서브모듈 목록 보기
git config --file .gitmodules --get-regexp path

# 특정 서브모듈의 URL 확인
git config --file .gitmodules --get submodule.[이름].url

# 특정 서브모듈의 브랜치 확인
git config --file .gitmodules --get submodule.[이름].branch
```

## 문제 확인 및 해결

문제 발견 시 다음 명령어로 서브모듈을 업데이트할 수 있습니다:

```bash
# 서브모듈 초기화 및 업데이트
git submodule init
git submodule update

# 또는 한 번에 실행
git submodule update --init
```

Git 상태 명령어(`git status`)도 서브모듈 변경사항을 간략히 표시하므로 일반적인 워크플로우에서 참고할 수 있습니다.