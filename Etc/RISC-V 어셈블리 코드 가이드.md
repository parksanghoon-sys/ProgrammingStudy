

## 아키텍처 개요

RISC-V는 오픈 소스 명령어 집합 아키텍처(ISA)로, 단순성과 모듈성을 특징으로 합니다.

## 레지스터

- **x0-x31**: 32개의 범용 레지스터
- **x0**: 항상 0 값을 가짐(zero)
- **x1/ra**: 반환 주소(Return Address)
- **x2/sp**: 스택 포인터(Stack Pointer)
- **x3/gp**: 전역 포인터(Global Pointer)
- **x4/tp**: 스레드 포인터(Thread Pointer)
- **x5-x7/t0-t2**: 임시 레지스터
- **x8-x9/s0-s1**: 저장 레지스터
- **x10-x17/a0-a7**: 함수 인자/반환 값
- **x18-x27/s2-s11**: 저장 레지스터
- **x28-x31/t3-t6**: 임시 레지스터

## 명령어 형식

RISC-V 명령어는 크게 6가지 형식으로 나뉩니다:

- **R-type**: 레지스터-레지스터 연산 (rd, rs1, rs2)
- **I-type**: 즉시값 연산과 로드 (rd, rs1, imm)
- **S-type**: 스토어 연산 (rs1, rs2, imm)
- **B-type**: 분기 연산 (rs1, rs2, imm)
- **U-type**: 상위 즉시값 연산 (rd, imm)
- **J-type**: 점프 연산 (rd, imm)

## 주요 명령어 그룹

### 1. 산술 연산

```
add rd, rs1, rs2    # rd = rs1 + rs2
addi rd, rs1, imm   # rd = rs1 + imm
sub rd, rs1, rs2    # rd = rs1 - rs2
mul rd, rs1, rs2    # rd = rs1 * rs2 (RV32M 확장)
div rd, rs1, rs2    # rd = rs1 / rs2 (RV32M 확장)
```

### 2. 논리 연산

```
and rd, rs1, rs2    # rd = rs1 & rs2
andi rd, rs1, imm   # rd = rs1 & imm
or rd, rs1, rs2     # rd = rs1 | rs2
ori rd, rs1, imm    # rd = rs1 | imm
xor rd, rs1, rs2    # rd = rs1 ^ rs2
xori rd, rs1, imm   # rd = rs1 ^ imm
```

### 3. 시프트 연산

```
sll rd, rs1, rs2    # 논리적 좌측 시프트
slli rd, rs1, imm   # 즉시값으로 논리적 좌측 시프트
srl rd, rs1, rs2    # 논리적 우측 시프트
srli rd, rs1, imm   # 즉시값으로 논리적 우측 시프트
sra rd, rs1, rs2    # 산술적 우측 시프트
srai rd, rs1, imm   # 즉시값으로 산술적 우측 시프트
```

### 4. 로드/스토어

```
lw rd, offset(rs1)  # rd = Memory[rs1 + offset], 워드 로드
sw rs2, offset(rs1) # Memory[rs1 + offset] = rs2, 워드 스토어
lb rd, offset(rs1)  # 바이트 로드 (부호 확장)
lbu rd, offset(rs1) # 바이트 로드 (부호 없음)
sb rs2, offset(rs1) # 바이트 스토어
```

### 5. 분기/점프

```
beq rs1, rs2, offset    # rs1 == rs2이면 pc += offset으로 분기
bne rs1, rs2, offset    # rs1 != rs2이면 pc += offset으로 분기
blt rs1, rs2, offset    # rs1 < rs2이면 분기
jal rd, offset          # rd = pc+4; pc += offset
jalr rd, rs1, offset    # rd = pc+4; pc = rs1 + offset
```

## 어셈블리 코드 패턴 예제

### 함수 프롤로그/에필로그

```
function:
    addi sp, sp, -16    # 스택 공간 확보
    sw ra, 12(sp)       # 반환 주소 저장
    sw s0, 8(sp)        # 보존 레지스터 저장
    addi s0, sp, 16     # 새 프레임 포인터 설정
    
    # 함수 본문
    
    lw s0, 8(sp)        # 보존 레지스터 복원
    lw ra, 12(sp)       # 반환 주소 복원
    addi sp, sp, 16     # 스택 공간 해제
    ret                 # jalr x0, ra, 0과 동일
```

### 루프 구현

```
    li t0, 0           # 카운터 초기화
    li t1, 10          # 최대값
loop:
    beq t0, t1, done   # 카운터 == 최대값이면 종료
    # 루프 내용
    addi t0, t0, 1     # 카운터 증가
    j loop             # 루프 시작으로 점프
done:
```

### 배열 접근

```
    la t0, array       # 배열 기본 주소 로드
    li t1, 4           # 인덱스
    slli t1, t1, 2     # 인덱스 * 4 (워드 크기)
    add t0, t0, t1     # 요소 주소 계산
    lw t2, 0(t0)       # 배열[인덱스] 값 로드
```

RISC-V 어셈블리 작성 시 특정 타겟 플랫폼과 도구체인에 따라 문법과 지시자가 약간 다를 수 있습니다.