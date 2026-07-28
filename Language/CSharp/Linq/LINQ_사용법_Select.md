# Linq Select

> LINQ 에서 Select 느 입력 시퀀스를 원하는 출력 시퀀스로 변환한다.

### Query Structure

예제) 배열 내에 5보다 낮은 숫자 출력

```csharp
int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

var numsPlusOne = form num in numbers select num + 1;

// numsPlusOne = { 6, 5, 2, 4, 10, 9, 7, 8, 3, 1 };
```

예제) 다른 타입으로 출력 바꾸기

```csharp
int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

var textNums = from n in numbers
                           select strings[n];

// textNums = { "five", "four", "one", "three", "nine", "eight", "six", "seven", "two", "zero" };

```

예제 ) 출력에 익명 형식 사용하기

```csharp
string[] words = {"aPPle", "BlueBerY" , "cHeRry"};
var upperLowerWords = from w in words
select new { Upper = w.ToUpper(), Lower = w.ToLower() };

// upperLowerWords = {
//     { APPLE, apple },
//     { BLUEBERRY, blueberry },
//     { CHERRY, cherry },
// }
```

7.0 부터는 Tuple로 출력을 투영 할 수 있다.

```csharp

            // numsInPlace = {
            // [ Num : 5, InPlace : false ],
            // [ Num : 4, InPlace : false ],
            // [ Num : 1, InPlace : false ],
            // [ Num : 3, InPlace : true ],
            // ...
            // }
출처: https://ibocon.tistory.com/97 [김예건:티스토리]\\192.168.0.150\nas\00.DevPart1\공군관리\조승현\20240520_SIT-Chk-008 디버깅int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

var digitOddEvens = from n in numbers
select (Digit : strings[n], Even : (n % 2 == 0));

// digitOddEvens = {
// [ Digit : "five", Even : false ],
// [ Digit : "four", Even : true ],
// [ Digit : "one", Even : false ],
// ...
// }
```

예제 ) 인덱스를 활용해 출력 만들기

numbers 배열의 속성중 하나인 index 속성을 select 절에 활용하여 숫자가 자신의 수넛에 맞는 위치를 학인하는 시퀀스

```csharp
int numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

var numberInplace = numbers.Select((num, index) => (Num : num, InPlace : (num == index)));

            // numsInPlace = {
            // [ Num : 5, InPlace : false ],
            // [ Num : 4, InPlace : false ],
            // [ Num : 1, InPlace : false ],
            // [ Num : 3, InPlace : true ],
            // ...
            // }

```
