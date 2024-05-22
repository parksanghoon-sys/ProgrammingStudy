# Linq Where

>  LINQ 에서 Where 절은 질의 결과를 제한하는 절이다. 오직 Where 절 조건에 맞는 요소만 결과 시퀀스에 추가되어 출력된다.

### Query Structure

예제) 배열 내에 5보다 낮은 숫자 출력

```csharp
int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

var lowNums = form num in numbers where num < 5 select num;

// lowNums = { 4, 1, 3, 2, 0 };
```
