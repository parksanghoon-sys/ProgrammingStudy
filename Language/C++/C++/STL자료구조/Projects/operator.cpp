#include <iostream>
#include <string>
#include <vector>
#include <algorithm>

using namespace std;

class Student
{
public:
    string name;
    int age;
public:
     Student(string name, int age): name(name),age(age){}
    ~ Student() = default;

    bool operator<(Student s) const{
         if(this->name == s.name){
            return this->age < s.age;
        }else{
            return this->name < s.name;
        }
    }
};
void Print(vector<Student> &v){
    cout << "Student overloading : " ;
    for(int i=0; i<5; i++){
        cout << "[" << v[i].name << ", " << v[i].age << "]";
    }
    cout << endl;
}
int main(void){
    vector<Student> v;
    
    v.push_back(Student("cc", 10));
    v.push_back(Student("ba", 24));
    v.push_back(Student("aa", 11));
    v.push_back(Student("cc", 8));  //cc는 이름이 같으니 나이 기준 오름차순 예시
    v.push_back(Student("bb", 21));
    
    Print(v); //정렬 전 출력
    sort(v.begin(), v.end()); //[begin, end) 연산자 오버로딩 이용한 정렬.
    Print(v); //정렬 후 출력
    
    return 0;
}
