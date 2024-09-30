
# Dictionary

딕셔너리는 Key Value 의 집합이다. 그러나 NET 은 Dictionary 데이터를 KVP 로 저장하지 않느다.

실제로는 `두 개의 배열`에 저장된다 하나는 'buckets'에 대한것이고 다른 하나는 값 'entries'에 대한것이다

buckets은 항목에 연결된다. 버킷에는 값이 포함이며 충돌 시 동일한 버킷의 다음 항목에대한 포인터가 포함된다.

buket에 key 가저장시 hascode를 기반으로 탐색을 진행후 Equals 키를 비교후 값을 도출해준다. 

기본크기는 소수로 3으로 설정되면 Remove 시 Key 검색을 통해 삭제된다. 그래고 다음에 넣을 index가 변수로 저장된다. 그리고 add 시 빈공간에 넣는다 만약 크기가 부족 시 2배로 늘어난다 List와 동일
