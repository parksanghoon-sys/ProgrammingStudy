해당 메시지는 **CMD.EXE**가 **UNC 경로**를 **현재 디렉터리**로 직접 지원하지 않기 때문에 발생합니다.

> '\shpark_Nas\Hoon\01.Study\00.ProgrammingStudy'
>
> 위의 경로를 현재 디렉터리로 하여 CMD.EXE가 실행되었습니다. UNC 경로는 지원되지 않습니다. Windows 디렉터리를 기본으로 합니다

UNC (Universal Naming Convention) 경로는 `\\서버이름\공유이름\폴더...` 와 같은 네트워크상의 자원 경로를 나타냅니다. CMD.EXE는 전통적으로 드라이브 문자(예: `C:`, `D:`)로 표시되는 **로컬 경로**나 **매핑된 드라이브**를 현재 디렉터리로 사용하는 것을 기본으로 합니다.

---

## 💡 해결 방법 및 대안

UNC 경로에서 CMD 명령을 실행하거나 해당 경로에 접근해야 할 경우, 다음과 같은 방법을 사용할 수 있습니다.

### 1. **`PUSHD` 명령 사용 (권장)**

CMD에서 UNC 경로를 임시 드라이브 문자로 매핑하여 사용할 수 있게 해주는 명령입니다.

* **사용법:**
  **DOS**

  ```
  PUSHD "\\shpark_Nas\Hoon\01.Study\00.ProgrammingStudy"
  ```
* **작동 방식:** 시스템에서 사용되지 않는 임시 드라이브 문자(예: `Z:`, `Y:`)를 자동으로 할당하고 해당 드라이브로 현재 디렉터리를 이동시킵니다.
* **작업 완료 후:** **`POPD`** 명령을 사용하여 할당된 드라이브 문자를 해제하고 원래 디렉터리로 돌아갈 수 있습니다.

### 2. **네트워크 드라이브 매핑**

UNC 경로를 영구적인 드라이브 문자(예: `N:`)로 매핑하여 사용합니다.

* **사용법 (PowerShell에서):**
  **PowerShell**

  ```
  New-PSDrive -Name "S" -PSProvider FileSystem -Root "\\shpark_Nas\Hoon" -Persist
  ```

  (이렇게 하면 `\\shpark_Nas\Hoon`이 `S:` 드라이브로 매핑되며, CMD에서는 `S:\01.Study\00.ProgrammingStudy`로 접근할 수 있습니다.)
* **장점:** CMD.EXE뿐만 아니라 다른 Windows 애플리케이션에서도 드라이브 문자로 쉽게 접근할 수 있습니다.

### 3. **PowerShell 계속 사용**

**PowerShell** 자체는 UNC 경로를 현재 디렉터리로 **완벽하게 지원**합니다.

* 현재 사용하고 계신 터미널이 **PowerShell**이라면, 명령 프롬프트(CMD) 대신 **PowerShell 명령어**를 사용하거나 `code .`와 같은 명령 대신 `Start-Process cmd.exe -ArgumentList '/k cd /d "\\shpark_Nas\Hoon\01.Study\00.ProgrammingStudy"'`와 같이 **CMD를 실행하면서 초기 디렉터리를 설정**하는 방식을 고려해 볼 수 있습니다.

---

## 📌 상황 분석: `code .` 명령

사용하신 명령어 `code .`는 일반적으로 **Visual Studio Code**를 현재 디렉터리 경로로 실행하는 명령입니다. 이 명령이 내부적으로 어떻게 동작하는지에 따라 UNC 경로 문제가 발생할 수 있습니다.

* **PowerShell**에서 `code .`를 실행하면, VS Code가 실행되면서 VS Code의 **터미널**이 **CMD**로 설정되어 있거나, VS Code가 현재 경로를 **CMD에 전달**하는 과정에서 이 메시지가 출력되었을 가능성이 높습니다.
* PowerShell 자체에서는 UNC 경로가 문제없지만, VS Code가 내부적으로 CMD를 사용하여 경로를 처리할 때 UNC 경로 제한에 걸리는 것입니다.

---

## 📝 다음 단계

UNC 경로로 VS Code를 실행하는 것이 목적이라면, **1번 (`PUSHD`)** 방법이 가장 간편합니다.

현재 PowerShell 창에서 다음을 실행하여 드라이브를 임시 매핑하고 VS Code를 실행할 수 있습니다.

**PowerShell**

```
PUSHD "\\shpark_Nas\Hoon\01.Study\00.ProgrammingStudy"
code .
POPD
```

혹은, **VS Code의 설정**에서 기본 터미널을 **PowerShell**로 변경하면 이 문제를 피할 수도 있습니다.

어떤 방법으로 이 문제를 해결하고 싶으신가요, 아니면 다른 질문이 있으신가요?
