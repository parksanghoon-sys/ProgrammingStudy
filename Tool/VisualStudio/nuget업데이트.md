좋아요 👍 지금 상황을 정리하면:

* 이미 **NuGet 패키지**를 만들어서 올려둠 ✅
* 지금 **현재 소스 코드 기준으로 버전 업**해서 새 버전을 배포하고 싶음

즉, 기존 패키지에서 **업데이트된 코드 → 새로운 NuGet 패키지 버전**으로 만드는 과정이 필요합니다.

---

## 1. csproj에서 버전 변경

NuGet 패키지의 버전은 `.csproj`에 지정되어 있습니다.

예:

```xml
<PropertyGroup>
  <TargetFramework>net8.0-windows</TargetFramework>
  <Version>1.0.0</Version>       <!-- 기존 버전 -->
  <Authors>박상훈</Authors>
  <PackageId>CoreMvvmLib.Design</PackageId>
</PropertyGroup>
```

새 버전으로 변경:

```xml
<Version>1.1.0</Version>   <!-- 예: 마이너 버전 업 -->
```

버전 규칙은 [SemVer](https://semver.org/)를 따릅니다:

* `1.0.0` → `1.0.1` (패치)
* `1.0.0` → `1.1.0` (마이너)
* `1.0.0` → `2.0.0` (메이저)

---

## 2. NuGet 패키지 빌드

```bash
dotnet pack -c Release
```

* `-c Release` → Release 빌드로 패키지 생성
* 기본적으로 `bin/Release` 폴더 안에 `.nupkg` 파일이 생성됩니다.

옵션으로 출력 경로 지정 가능:

```bash
dotnet pack -c Release -o ../nupkg
```

---

## 3. NuGet 배포

### (1) 기존 NuGet.org 업로드

```bash
dotnet nuget push ./bin/Release/CoreMvvmLib.Design.1.1.0.nupkg \
  --api-key <YOUR_API_KEY> \
  --source https://api.nuget.org/v3/index.json
```

* `<YOUR_API_KEY>` → NuGet.org에서 발급한 API 키
* `--source` → 배포할 NuGet 소스 URL

### (2) 사내 NuGet 서버 사용 시

```bash
dotnet nuget push ./bin/Release/CoreMvvmLib.Design.1.1.0.nupkg \
  --source "MyCompanyNuget"
```

---

## 4. 패키지 참조 업데이트 (메인 프로젝트)

메인 프로젝트에서 기존 패키지를 참조하고 있다면, 새 버전으로 업데이트:

```bash
dotnet add package CoreMvvmLib.Design --version 1.1.0
```

---

## 5. 체크리스트

1. csproj `<Version>` 변경 ✅
2. `dotnet pack`로 nupkg 생성 ✅
3. NuGet.org 또는 사내 서버에 push ✅
4. 메인 프로젝트에서 새 버전 참조 ✅

---

💡 참고:

* 버전 규칙을 지키지 않고 같은 버전을 다시 push하면 **NuGet.org는 거부**합니다.
* 미리 **pre-release** 패키지 (`1.1.0-beta1`)로 올려서 테스트 가능

---

원하면 제가 **현재 구조 기준으로 MvvmLib 전체를 패키지 버전업 → 메인 프로젝트에서 참조까지 자동화 스크립트** 형태로 정리해드릴 수도 있어요.

그걸 해드릴까요?
