# LINQ 분할연산자

> LINQ 에서 분할 연산자는 Take, Skip, TakeWhile, SkipWhile을 분할 연산자라고 한다. 왜냐하면 분할 연산자는 출력 시퀀스를 분할시켜 반환한다

### Take 분할연산자

예제) Take 메서드로 numbers 배열에서 처음 3개의 원소만 꺼내 출력한다

```csharp
int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

var first3Numbers = numbers.Take(3);
```

출력은 IEnumerable 인터페이스로 정의된다.

### Skip 분할 연산자

Skip 메서드로 numbers 배열에서 처음 4개의 원소를 건너 뛰고 출력한다.

```csharp
int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
var allButFirst4Numbers = numbers.Sikp(4);
```

출력은 IEnumerable 인터페이스로 정의된다.

### TakeWhile 분할 연산자

Take와 Skip과 달리, 주어진 조건에 도달하기 전까지만 출력 시퀀스에 포함된다.

TakeWhile 메서드로 numbers 배열원소 중 6 보다 작은 원소가 나오기 전까지 출력 시퀀스에 포함된다.

```csharp
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var firstNumbersLessThan6 = numbers.TakeWhile(n => n < 6);

            // firstNumbersLessThan6 = { 5, 4, 1, 3 }

```

numbers 배열중 원소의 index 값보다  큰원소가 나오기 전만 출력 시퀀스에 포함된다.

```csharp
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var firstSmallNumbers = numbers.TakeWhile((n, index) => n >= index);

            // firstSmallNumbers = { 5, 4 }
```

### SkipWhile 분할 연산자

numbers 배열 첫 원소부터 시작해서 3으로 나누어지지 않는 원소는 건너뛰고, 3으로 나누어지는 원소 출력 시퀀스를 만든다

```csharp

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var allButFirst3Numbers = numbers.SkipWhile(n => n % 3 != 0);

            // allButfirst3Numbers = { 3, 9, 8, 6, 7, 2, 0 };
```
