# LSB 와 MSB

LSB (Least Significant Bit/ Byte) 와 MSB (Most Significant Bit/ Byte)는 숫자의 가장 낮은 비트/바이트와 가장 높은 비트/바이트를 각각 나타낸다 이 개념은 숫자표현과 컴퓨터 메모리 구조에서 중요한 역할을한다.

## LSB

* Least Significant Bit (최하위 비트):
  * 숫자의 이진수 변형중 *가장 오른쪽에 있는비트* 이다.
  * 숫자의 짝수/ 홀수 여부를 결정한다
* Least Significant Byte (최하위 바이트)
  * 숫자의 바이트 배열에서 가장 작은 값을 담은 바이트이다
  * 리틀 엔디안 방식에서 가장 작은 메모리 주소에 저장된다.

### MSB

* **Most Significant Bit (최상위 비트):**
  * 숫자의 이진수 표현중 *가장 왼쪽에 있는비트 이다.*
  * 부호 비트로 사용되는 경우가 많다 (예 : 1이면 음수, 0 이면 양수)
* Most Significant Byte (최상위 바이트)
  * 숫자의 바이트 배열에서 **가장 큰 값을 담는 바이트**입니다.
  * 빅 엔디안 방식에서 가장 작은 메모리 주소에 저장됩니다.

### **예제: 16진수 숫자 `0x1234`**

이 숫자를 이진수로 표현하면 `0001 0010 0011 0100`입니다.

#### LSB와 MSB

* **LSB (비트):** `0001 0010 0011 0100`에서 **맨 오른쪽 비트** → `0`
* **MSB (비트):** `0001 0010 0011 0100`에서 **맨 왼쪽 비트** → `0`

#### LSB와 MSB (바이트 기준)

* **LSB (바이트):** `0x34`
* **MSB (바이트):** `0x12`


### **LSB와 MSB의 주요 차이점**

| **특징**                      | **LSB (Least Significant)** | **MSB (Most Significant)** |
| ----------------------------------- | --------------------------------- | -------------------------------- |
| **위치 (비트 기준)**          | 이진수의 가장 오른쪽              | 이진수의 가장 왼쪽               |
| **위치 (바이트 기준)**        | 숫자의 가장 작은 값               | 숫자의 가장 큰 값                |
| **메모리 주소 (리틀 엔디안)** | 가장 작은 메모리 주소에 저장      | 가장 큰 메모리 주소에 저장       |
| **메모리 주소 (빅 엔디안)**   | 가장 큰 메모리 주소에 저장        | 가장 작은 메모리 주소에 저장     |
| **역할**                      | 세부적인 값이나 변화 감지         | 숫자의 크기나 부호 결정          |
