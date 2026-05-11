# C#의 LINQ (Language Integrated Query) 사용 방법 및 자주 사용되는 예제

LINQ는 C#에서 데이터 쿼리를 위한 강력한 기능으로, 다양한 데이터 소스(배열, 컬렉션, XML, 데이터베이스 등)에 대해 일관된 방식으로 쿼리를 작성할 수 있게 해줍니다.

## 기본 문법

LINQ는 두 가지 방식으로 작성할 수 있습니다:

### 1. 쿼리 구문 (Query Syntax)

```csharp
var result = from item in collection
             where item.Property > value
             select item;
```

### 2. 메서드 구문 (Method Syntax)

```csharp
var result = collection.Where(item => item.Property > value).Select(item => item);
```

## 자주 사용되는 LINQ 메서드 및 예제

### 1. Where - 필터링

```csharp
// 숫자 목록에서 짝수만 필터링
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
var evenNumbers = numbers.Where(n => n % 2 == 0);
// 결과: 2, 4, 6, 8, 10

// 쿼리 구문 사용
var evenNumbersQuery = from n in numbers
                      where n % 2 == 0
                      select n;
```

### 2. Select - 프로젝션

```csharp
// 숫자 목록의 각 요소를 제곱
var squares = numbers.Select(n => n * n);
// 결과: 1, 4, 9, 16, 25, 36, 49, 64, 81, 100

// 객체의 특정 속성만 선택
List<Person> people = GetPeople(); // 사람 목록을 반환하는 함수
var names = people.Select(p => p.Name);
```

### 3. OrderBy/OrderByDescending - 정렬

```csharp
// 오름차순 정렬
var ascending = numbers.OrderBy(n => n);

// 내림차순 정렬
var descending = numbers.OrderByDescending(n => n);

// 객체 리스트에서 나이순으로 정렬 후 이름순으로 정렬
var sortedPeople = people.OrderBy(p => p.Age).ThenBy(p => p.Name);
```

### 4. GroupBy - 그룹화

```csharp
// 숫자를 짝수/홀수로 그룹화
var groupedByParity = numbers.GroupBy(n => n % 2 == 0);

// 사람을 나이대별로 그룹화
var groupedByAgeGroup = people.GroupBy(p => p.Age / 10);

// 쿼리 구문 사용
var groupedByAgeGroupQuery = from p in people
                            group p by p.Age / 10 into ageGroup
                            select new { AgeGroup = ageGroup.Key, People = ageGroup };
```

### 5. Join - 조인

```csharp
List<Department> departments = GetDepartments();
List<Employee> employees = GetEmployees();

var employeeDepartments = employees.Join(
    departments,
    e => e.DepartmentId,
    d => d.Id,
    (e, d) => new { EmployeeName = e.Name, DepartmentName = d.Name }
);

// 쿼리 구문 사용
var query = from e in employees
            join d in departments on e.DepartmentId equals d.Id
            select new { EmployeeName = e.Name, DepartmentName = d.Name };
```

### 6. First/FirstOrDefault, Single/SingleOrDefault

```csharp
// 첫 번째 짝수 가져오기 (없으면 예외 발생)
var firstEven = numbers.First(n => n % 2 == 0);

// 첫 번째 짝수 가져오기 (없으면 기본값 반환)
var firstEvenOrDefault = numbers.FirstOrDefault(n => n % 2 == 0);

// 조건에 맞는 요소가 정확히 하나만 있는지 확인 (여러 개면 예외 발생)
var singleTen = numbers.Single(n => n == 10);

// 조건에 맞는 요소가 정확히 하나만 있는지 확인 (없으면 기본값 반환)
var singleOrDefault = numbers.SingleOrDefault(n => n == 100);
```

### 7. Any, All, Contains

```csharp
// 짝수가 하나라도 있는지 확인
bool hasEven = numbers.Any(n => n % 2 == 0);

// 모든 숫자가 양수인지 확인
bool allPositive = numbers.All(n => n > 0);

// 특정 값이 포함되어 있는지 확인
bool containsFive = numbers.Contains(5);
```

### 8. Count, Sum, Average, Min, Max

```csharp
// 짝수의 개수 계산
int evenCount = numbers.Count(n => n % 2 == 0);

// 합계 계산
int sum = numbers.Sum();

// 평균 계산
double average = numbers.Average();

// 최소값
int min = numbers.Min();

// 최대값
int max = numbers.Max();

// 객체 리스트에서의 집계 함수 사용
int totalAge = people.Sum(p => p.Age);
int maxAge = people.Max(p => p.Age);
```

### 9. Skip, Take - 페이징

```csharp
// 처음 3개 요소 건너뛰기
var skipped = numbers.Skip(3);

// 처음 5개 요소 가져오기
var taken = numbers.Take(5);

// 페이징 구현 (페이지 크기가 10인 경우)
int pageNumber = 2; // 2페이지
int pageSize = 10;
var page = people.Skip((pageNumber - 1) * pageSize).Take(pageSize);
```

### 10. ToList, ToArray, ToDictionary

```csharp
// 결과를 List로 변환
List<int> evenList = numbers.Where(n => n % 2 == 0).ToList();

// 결과를 배열로 변환
int[] evenArray = numbers.Where(n => n % 2 == 0).ToArray();

// 결과를 Dictionary로 변환 (키는 Id, 값은 Person 객체)
Dictionary<int, Person> personDict = people.ToDictionary(p => p.Id);
```

## 고급 LINQ 기법

### 1. 집합 연산

```csharp
List<int> list1 = new List<int> { 1, 2, 3, 4, 5 };
List<int> list2 = new List<int> { 4, 5, 6, 7, 8 };

// 합집합 (중복 제거)
var union = list1.Union(list2); // 1, 2, 3, 4, 5, 6, 7, 8

// 교집합
var intersect = list1.Intersect(list2); // 4, 5

// 차집합
var except = list1.Except(list2); // 1, 2, 3
```

### 2. 중첩 쿼리

```csharp
var query = from d in departments
            select new
            {
                Department = d.Name,
                Employees = (from e in employees
                           where e.DepartmentId == d.Id
                           select e.Name).ToList()
            };
```

### 3. Let 키워드 (쿼리 구문에서 임시 변수 정의)

```csharp
var query = from p in people
            let ageGroup = p.Age / 10
            where ageGroup >= 2 && ageGroup <= 5
            orderby ageGroup
            group p by ageGroup into g
            select new { AgeGroup = g.Key * 10 + "대", Count = g.Count() };
```

LINQ는 데이터 처리를 간결하고 효율적으로 만들어주는 C#의 핵심 기능입니다. 복잡한 데이터 처리 로직을 단순화하고 코드의 가독성을 높여줍니다.