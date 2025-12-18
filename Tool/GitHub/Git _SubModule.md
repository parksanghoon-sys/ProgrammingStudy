
# Git 서브모듈(Submodule) 완벽 가이드 🚀

대규모 프로젝트나 모노레포 구조에서는 하나의 저장소 안에서 다른 저장소를 **하위 모듈**처럼 포함하고 싶을 때가 있습니다. 이럴 때 사용하는 기능이 바로 **Git Submodule**입니다.

예를 들어, 여러 프로젝트에서 공통으로 사용하는 **라이브러리**를 따로 관리하고, 필요할 때 각 프로젝트에 포함시키고 싶을 수 있습니다.

---

## 1. 서브모듈이란?

* **서브모듈**은 깃 저장소 안에 다른 깃 저장소를 포함시키는 기능입니다.
* 단순히 소스 코드를 복사하는 것이 아니라, **특정 커밋을 가리키는 포인터**처럼 동작합니다.
* 각 프로젝트는 서브모듈의 최신 버전을 강제하지 않고, 원하는 시점의 커밋을 지정할 수 있습니다.

---

## 2. 서브모듈 추가하기

### 예제:

프로젝트 구조

```
MainProject/
└── ExternalLib/   (공통 라이브러리를 서브모듈로 추가)
```

명령어:

```bash
# 메인 프로젝트 클론
git clone https://github.com/my-org/MainProject.git
cd MainProject

# 공통 라이브러리를 서브모듈로 추가
git submodule add https://github.com/my-org/ExternalLib.git ExternalLib

# 변경 사항 커밋
git commit -m "Add ExternalLib as submodule"
```

결과:

* `.gitmodules` 파일이 생성됩니다.
* `ExternalLib/` 폴더는 독립적인 깃 저장소입니다.

`.gitmodules` 예시:

```ini
[submodule "ExternalLib"]
    path = ExternalLib
    url = https://github.com/my-org/ExternalLib.git
```

---

## 3. 서브모듈 초기화 및 업데이트

프로젝트를 새로 클론하면 서브모듈은 자동으로 받아오지 않습니다.

```bash
# 메인 프로젝트 클론
git clone https://github.com/my-org/MainProject.git
cd MainProject

# 서브모듈 초기화 & 업데이트
git submodule init
git submodule update
```

혹은 한 번에:

```bash
git clone --recurse-submodules https://github.com/my-org/MainProject.git
```

---

## 4. 서브모듈에서 작업하기

서브모듈 디렉토리는 **별도의 깃 저장소**입니다.

즉, `cd ExternalLib` 후 일반적으로 작업 가능.

```bash
cd ExternalLib
git checkout -b feature/new-api
# 코드 수정
git commit -am "Add new API"
git push origin feature/new-api
```

---

## 5. 리모트 변경사항 반영하기 (서브모듈 업데이트 & 머지)

서브모듈의 원격 저장소에서 새로운 변경이 생겼다면, 메인 프로젝트에 반영해야 합니다.

### 단계별 예제:

1. **서브모듈 디렉토리로 이동**

```bash
cd ExternalLib
```

2. **리모트 변경사항 가져오기**

```bash
git fetch origin
git checkout main
git merge origin/main
```

3. **메인 프로젝트로 돌아가기**

```bash
cd ..
git status
```

→ `ExternalLib (new commits)` 라고 표시됨

4. **메인 프로젝트에서 커밋하기**

```bash
git add ExternalLib
git commit -m "Update ExternalLib to latest main"
git push
```

즉, 메인 프로젝트는 서브모듈이 가리키는 **커밋 해시**만 업데이트해서 관리합니다.

---

## 6. 자주 쓰는 서브모듈 명령어 정리

```bash
# 서브모듈 초기화
git submodule init

# 서브모듈 업데이트 (최신 커밋 가져오기 아님, 포인터 동기화만)
git submodule update

# 서브모듈과 메인 프로젝트 모두 최신으로 업데이트
git submodule update --remote --merge

# 서브모듈 전체 초기화 + 업데이트
git submodule update --init --recursive

# 서브모듈 삭제
git submodule deinit -f ExternalLib
rm -rf .git/modules/ExternalLib
git rm -f ExternalLib
```

---

## 7. 정리

* 서브모듈은 다른 프로젝트를 **특정 커밋 단위로 참조**할 수 있는 기능
* 공통 라이브러리, 외부 프로젝트를 재사용할 때 유용
* 단점: **조금 번거로운 워크플로우** (init, update, commit 필요)
* 리모트 업데이트는 반드시 **서브모듈 내부 → pull/merge → 메인 프로젝트 커밋** 절차 필요

---

👉 이렇게 하면 블로그 글 하나로 서브모듈을 완전히 이해하고 실무에 적용할 수 있습니다.

혹시 이 글을 블로그에 올리실 때, 실무에서 자주 겪는 **실수/트러블슈팅 (예: 서브모듈이 detached HEAD 상태가 되는 문제, push 누락 문제)** 도 같이 정리해드릴까요?
