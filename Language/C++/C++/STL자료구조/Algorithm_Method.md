# include\<algorithm>

## 정렬 Sort

```c++
#include<iostream>
using namespace std;

int main() {

	int a[10] = { 9,3,5,4,1,10,8,6,7,2 };
	sort(a, a + 10);
    //내림차순 시 sort(a, a + 10, greater<int>());
	for (int i = 0; i < 10; i++) {
		cout << a[i] << " ";
	}


	return 0;
}
```

## 최대/최소값 max_element,min_element

```c++
#include<iostream>
#include<algorithm>
using namespace std;

int main() {

	int a[10] = { 9,3,5,4,1,10,8,6,7,2 };
	cout << *max_element(a, a + 10) << endl;    // 10
	cout << *min_element(a, a + 10) << endl;    // 1

	return 0;
}
```

## 순열 next_permutation

서로다른 **n 개의 원소에서 r 개를 뽑아 한줄로 나열하는 경우의 수**이다.

```c++
// default
bool next_permutation (BidirectionalIterator first,
                         BidirectionalIterator last);
 
// custom
bool next_permutation (BidirectionalIterator first,
                         BidirectionalIterator last, Compare comp);
```

순열을 구할 시작과 끝(iterator)을 인자로 받는다. 다음 숮열이 존재시 원소 순서를 바꾸고 true 반환, 없을시 false 반환.

주의사항

* 오름차순으로 정렬이 되어야한다.
* 중복을 제외하고 순열을 만들어야한다.

```c++
#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;
 
int main() {
    vector<int> v{ 1, 2, 3};
 
    sort(v.begin(), v.end());
 
    do {
        for (auto it = v.begin(); it != v.end(); ++it)
            cout << *it << ' ';
        cout << endl;
    } while (next_permutation(v.begin(), v.end()));
}
```

### 순열을 이용한 조합 구하기

**조합이란 n개의 원소중 r개를 뽑는 경우의 수**이다 순열은 순서가 중요하지만, 조합은 r개를 뽑기 떄문에 순서는 중요하지 않는다.

*조합 구하기*배열 s의 n개의 원소 중 r개의 원소를 택하는방법

1. 크기가 n 개인 배열 temp를 만들어 r개의 원소는 1로 (n-r)개의 원소는 0으로 초기화한다.
2. temp의 모든 순열을 구한다.
3. temp의 순열에서 원소값이 1인 인덱스 배열 s 에서 가져온다.

**prev_permutation은 내림차순 정렬된 데이터를 받아서 이전 순열로 바꿔줍니다.**

사용 예시 - {1, 2, 3, 4} 중 2개의 원소를 고르는 모든 경우의 수 출력하기

```c++

#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;
 
int main() {
    vector<int> s{ 1, 2, 3, 4 };
    vector<int> temp{ 1, 1, 0, 0 };
 
    do {
        for (int i = 0; i < s.size(); ++i) {
            if (temp[i] == 1)
                cout << s[i] << ' ';
        }
        cout << endl;
    } while (prev_permutation(temp.begin(), temp.end()));
}
```
