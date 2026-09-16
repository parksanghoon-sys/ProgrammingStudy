#include <iostream>
#include <queue>
#include <vector>
using namespace std;

int number = 9;
int visit[9];
vector<int> a[10];

void bfs(int start){
    queue<int> q;
    q.push(start);
    visit[start] = true;
    
    while(!q.empty()){
        // 큐에 값이 있을경우 계속 반복 실행
        // 큐에 값이 있다. => 아직 방문하지 않은 노드가 존재 한다. 
        int x = q.front();
        q.pop();
        printf("%d ", x);
        for(int i=0; i< a[x].size(); i++){
            int y = a[x][i];
            if(!visit[y]){
                // 방문하지 않았다면..
                q.push(y);
                visit[y] = true; 
            }
        }
    }
}

int main(void){
    
    // 1과 2를 연결 
    a[1].push_back(2);
    a[2].push_back(1);
    
    // 1과 3을 연결 
    a[1].push_back(3);
    a[3].push_back(1);
    
    // 2와 4를 연결 
    a[2].push_back(4);
    a[4].push_back(2);

    // 2와 5를 연결 
    a[2].push_back(5);
    a[5].push_back(2);
    
    // 4와 8을 연결 
    a[4].push_back(8);
    a[8].push_back(4);
    
    // 5와 9를 연결 
    a[5].push_back(9);
    a[9].push_back(5);
    
    // 3과 6을 연결 
    a[3].push_back(6);
    a[6].push_back(3);
    
    // 3과 7을 연결 
    a[3].push_back(7);
    a[7].push_back(3);
    
    // 1번 노드부터 bfs 탐색 실행 
    bfs(1);
    
    return 0;
} 