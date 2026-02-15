Claude Code에 대해 자세히 설명해드리겠습니다. 먼저 최신 정보를 확인해볼게요.좋아요! Claude Code의 핵심 기술들에 대해 자세히 설명해드리겠습니다. 검색한 정보를 바탕으로 체계적으로 정리해볼게요.

# Claude Code의 핵심 기술 스택

Claude Code는 단순한 코딩 도구가 아니라 **AI 에이전트를 오케스트레이션하는 프레임워크**입니다. 주요 기술들이 어떻게 함께 작동하는지 설명드릴게요.

## 1. 🔌 MCP (Model Context Protocol)

**MCP는 무엇인가요?**

* AI 에이전트를 외부 도구와 데이터 소스에 연결하는 오픈 표준 프로토콜
* AI를 위한 USB-C 같은 범용 어댑터라고 생각하면 됩니다

**언제 사용하나요?**

* 데이터베이스, API, GitHub, Slack 등 외부 시스템 연결
* OAuth 기반 인증이 필요한 경우
* 실시간 데이터 접근이 필요할 때

**실제 예시:**

```json
// ~/.claude.json 파일에서 MCP 서버 설정
{
  "mcpServers": {
    "github": {
      "command": "npx",
      "args": ["@modelcontextprotocol/server-github"],
      "env": {
        "GITHUB_TOKEN": "your-token"
      }
    },
    "postgres": {
      "command": "npx",
      "args": ["@modelcontextprotocol/server-postgres"],
      "env": {
        "DATABASE_URL": "postgresql://..."
      }
    }
  }
}
```

**사용 시나리오:**

```bash
# Claude에게 요청:
"GitHub에서 최근 이슈 중 'bug' 라벨이 있는 것들을 찾아서 
Postgres 데이터베이스의 bug_tracking 테이블에 저장해줘"

# Claude는 자동으로:
# 1. GitHub MCP로 이슈 검색
# 2. Postgres MCP로 데이터 저장
```

## 2. 📚 Skills (에이전트 스킬)

**Skills는 무엇인가요?**

* 지시사항, 스크립트, 리소스를 포함한 폴더로, Claude가 특정 작업의 성능을 향상시키기 위해 동적으로 로드
* 재사용 가능한 전문성 패키지
* Progressive disclosure 방식: 필요할 때만 로드됨

**구조:**

```
my-skill/
├── SKILL.md              # 메인 문서 (필수)
├── helper-script.py      # 실행 가능한 코드 (선택)
├── examples/             # 예시 파일들
└── reference-docs/       # 참고 자료
```

**SKILL.md 예시:**

```markdown
---
name: react-testing-expert
description: React 컴포넌트 테스트 작성 전문가
---

# React Testing Expert Skill

이 스킬은 React Testing Library와 Jest를 사용한 
컴포넌트 테스트 작성을 도와줍니다.

## 테스트 작성 원칙

1. 사용자 관점에서 테스트
2. Implementation details 피하기
3. Accessibility 쿼리 우선 사용

## 코드 템플릿

```typescript
import { render, screen, userEvent } from '@testing-library/react'
import { MyComponent } from './MyComponent'

describe('MyComponent', () => {
  it('should handle user interaction', async () => {
    const user = userEvent.setup()
    render(<MyComponent />)
  
    const button = screen.getByRole('button', { name: /submit/i })
    await user.click(button)
  
    expect(screen.getByText(/success/i)).toBeInTheDocument()
  })
})
```

```

**실제 사용:**
```bash
# Skills 마켓플레이스 등록 (한 번만)
/plugin marketplace add anthropics/skills

# 사용
"react-testing-expert 스킬을 사용해서 
LoginForm 컴포넌트의 테스트를 작성해줘"
```

**Skills vs MCP 비교:**

| 측면 | Skills                                        | MCP                           |
| ---- | --------------------------------------------- | ----------------------------- |
| 목적 | "어떻게" 할 것인가 (방법론)                   | "무엇을" 연결할 것인가 (도구) |
| 내용 | 지시사항 + 스크립트                           | 외부 시스템 연결              |
| 예시 | "이렇게 코드 리뷰해", "이 방식으로 문서 작성" | GitHub API, Postgres DB       |

## 3. 🤖 Subagents (서브에이전트)

**Subagents는 무엇인가요?**

* 독립된 컨텍스트에서 병렬 작업을 처리하는 전문화된 에이전트
* 각각 특정 도구에만 접근 가능 (권한 격리)
* 백그라운드나 포그라운드에서 실행

**Subagent 정의 예시:**

```markdown
<!-- ~/.claude/agents/code-reviewer.md -->
---
name: code-reviewer
description: 코드 품질과 보안을 검토하는 전문가
tools: 
  - Read
  - Grep
  - Glob
disallowedTools:
  - Write
  - Edit
model: sonnet
permissionMode: auto-approve
---

당신은 코드 리뷰 전문가입니다.

## 검토 항목

1. **보안 취약점**
   - SQL Injection
   - XSS 공격 가능성
   - 하드코딩된 시크릿

2. **코드 품질**
   - 네이밍 컨벤션
   - 함수 복잡도
   - 중복 코드

3. **성능**
   - N+1 쿼리
   - 불필요한 재렌더링
   - 메모리 누수

검토 결과는 마크다운 형식으로 작성하고,
심각도(Critical/High/Medium/Low)를 표시합니다.
```

**실제 워크플로우:**

```bash
# 메인 Claude 세션에서:
"src/ 디렉토리의 모든 TypeScript 파일을 리팩토링하고 
code-reviewer를 통해 검토받아줘"

# Claude는 자동으로:
# 1. 코드 리팩토링 (Write 권한 사용)
# 2. code-reviewer subagent 호출 (Read 전용)
# 3. 리뷰 결과 통합
# 4. 필요시 수정 재적용
```

**병렬 실행 예시:**

세 개의 서브에이전트가 동시에 작동 - OAuth 2.0 모범 사례 연구, 현재 인증 흐름 문서화, 새 구현을 위한 테스트 작성 - 그런 다음 결과를 상위 에이전트에 보고

```bash
# Claude에게 요청:
"새로운 인증 시스템을 설계하고 구현해줘"

# Claude가 자동으로 3개의 subagent 병렬 실행:
# Subagent 1: OAuth 2.0 베스트 프랙티스 조사
# Subagent 2: 현재 인증 플로우 문서화
# Subagent 3: 새 구현을 위한 테스트 작성
```

## 4. 🎯 Hooks (실행 제어)

**Hooks는 무엇인가요?**

* 에이전트 실행의 특정 시점에서 자동으로 실행되는 커스텀 로직
* 작업 전/후 자동화 가능

**Hook 예시:**

```yaml
---
name: auto-linter
description: 코드 변경 전 자동으로 린트 실행
hooks:
  beforeToolUse:
    - tool: Write
      action: lint-and-format
---

# Auto Linter Hook

파일을 Write하기 전에 자동으로 ESLint와 Prettier를 실행합니다.
```

## 5. 🔧 실전 통합 시나리오

**복잡한 작업 예시: 경쟁사 분석 리포트**

```bash
# 단일 명령으로 모든 것을 조율
"경쟁사 분석 리포트를 작성해줘. 
최근 3개월 데이터를 기반으로 해야 해"
```

**Claude가 자동으로 실행하는 워크플로우:**

```
1. Project Context 로드
   └─ 업로드된 리서치 문서 접근

2. MCP 연결 활성화
   ├─ Google Drive MCP: 경쟁사 브리프 검색
   └─ GitHub MCP: 기술 구현 분석

3. Skills 활용
   └─ competitive-analysis Skill로 분석 프레임워크 적용

4. Subagents 병렬 실행
   ├─ market-researcher: 산업 데이터 수집
   └─ technical-analyst: 기술 구현 검토

5. 결과 통합 및 리포트 생성
   └─ 모든 데이터를 종합해 최종 문서 작성
```

## 6. 💡 베스트 프랙티스

**MCP 사용 시:**

* 최소 권한 원칙 적용 - read-only 접근이 가능한 경우 write 권한 주지 않기
* 프로덕션과 개발 환경을 분리된 MCP로 설정
* OAuth 인증 적극 활용

**Skills 작성 시:**

* 이름과 설명에 특별히 주의 - Claude의 관점에서 생각하기
* SKILL.md가 복잡해지면 별도 파일로 분리
* 코드를 실행용/참고용으로 명확히 구분

**Subagents 설계 시:**

* 명확한 책임 분리
* 필요한 최소한의 도구만 허용
* 결과물 형식 미리 정의

## 7. 🎬 실제 개발 시나리오

**시나리오: Android 앱 기능 개발**

```bash
# Mumu씨의 실제 워크플로우
"Hanatour 앱에 새로운 해외 간편홈 화면을 만들어줘.
Jetpack Compose로 구현하고, 
기존 XML 코드는 마이그레이션해야 해."

# Claude Code가 실행:

1. android-compose-expert Skill 로드
   └─ Compose 베스트 프랙티스 적용

2. MCP 연결
   ├─ GitHub: 기존 코드 분석
   └─ Android Docs: 최신 API 참조

3. Subagent 병렬 작업
   ├─ xml-to-compose-migrator
   │  └─ XML → Compose 변환
   ├─ ui-designer
   │  └─ 새 화면 UI 구현
   └─ code-reviewer
      └─ 품질 검토 + 보안 체크

4. Hook 실행
   └─ Git commit 전 ktlint 자동 실행

5. 최종 통합
   └─ PR 생성 + 테스트 코드 포함
```

이렇게 Claude Code는 여러 기술을 조합해서 복잡한 개발 작업을 자동화할 수 있습니다!

궁금한 부분이나 특정 시나리오에 대해 더 자세히 알고 싶으신 부분이 있나요?
