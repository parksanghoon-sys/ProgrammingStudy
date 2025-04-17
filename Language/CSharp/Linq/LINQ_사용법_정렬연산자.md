# Linq 정렬 연산자

> LINQ 에서 OrderBy는 출력 시퀀스를 기준에 따라 정렬하는 연산자이다

### Query Structure

예제) 알파벳 순서로 정렬

```csharp
            string[] words = { "cherry", "apple", "blueberry" };

            var sortedWords = from word in words
                              orderby word
                              select word;

               // sortedWords = { "apple", "blueberry", "cherry" };
```

예제) 내림차순 정렬

```csharp
            double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

            var sortedDoubles = from d in doubles
                                orderby d descending
                                select d;

            // sortedDoubles = { 4.1, 2.9, 2.3, 1.9, 1.7 }
```

### 정렬 뒤집기

```csharp
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var reversedIDigits = (
                from digit in digits
                where digit[1] == 'i'
                select digit)
                .Reverse();

            // reversedIDigits = { "nine", "eight", "six", "five" };
```

### 비교연산자를 구현해 활용하기

1. 비교연산자 구현

    ```csharp
            public class CaseInsensitiveComparer : IComparer<string>
            {
                public int Compare(string x, string y) => 
                    string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
            }
    ```

2. 적용

    ```csharp
                string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

                var sortedWords = words.OrderBy(a => a, new CaseInsensitiveComparer());

                // sortedWords = { "AbAcUs", "aPPLE", "BlUeBeRrY", "bRaNcH", "cHeRry", "ClOvEr" }
    ```
