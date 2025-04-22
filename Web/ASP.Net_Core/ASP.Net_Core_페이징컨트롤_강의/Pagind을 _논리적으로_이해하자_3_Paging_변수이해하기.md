[[1.1.Asp.NetCore개요]]
# Paging에 사용되는 변수들 이해하기

## 전체 데이터 수
* TotalEntries : 검색 전 전체 데이터 수
* TotalItems : 검색 후 전체 데이터 수
## Paging Control과 관련
* CurrentPage : 현재 페이지
* TotalPage : 총 페이지수
* PagingGroup : 페이지 목록의 인덱스 ex ) 1페이지 ~ 5페이지 가 페이지 목록의 1 번이라면 `PagingGroup => 1`
* StartPage : 페이지 목록에서 가장 앞의 인덱스
* EndPage : 페이지 목록의 마지막 인덱스
## 데이터 갯수와 관련
* PagingGroup : 아이템 리스트 들의 페이지 넘버 ex) 1~5 개의 아이템은 1페이지라면 `PagingGroup => 1`
* FirstItem : 첫 데이터 번호
* LastItem : 마지막 데이터 번호
## Linq.Enumerable class
* Skip method : 건너 뛰라
  * 1페이지 같은경우 1~7 데이터가 보여져야한다. => skip 0
  * 2페이지 8~ 7 번 데이터가 보여져야한다  => skip 7
* Take method : 가져 와라
  * ItemsPerPage 7 개의 데이터를 가져와라
  * 333333