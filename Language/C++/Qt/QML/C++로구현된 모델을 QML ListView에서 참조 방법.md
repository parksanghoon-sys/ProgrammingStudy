# C++로 구현된 모델을 QML의 ListView에서 참조
QAbstractItemModel 는 C++로 구현된 모델을 QML에서 참조 할 수 있는 좋은 방법이다. 즉 **QAbstractItemModel의 서브 클래스는 ModelView(ListView, GridView, Repeater등과 같은)에서 사용 가능하다**. C++ 데이터 세트 조작 방법은 성능에 큰 영향을 미치지 않고 QML UI에 업데이트가 가능하다.  
<br>
QAbstractItemModel는 추상 클래스이므로 인터페이스르 준수하도록 사용자가 구현을 해줘야한다, 아래와 같은 메서드를 구현 해 주어야 한다.
> int `rowCount();`  
> QVariant `data();`  
> QHash<int, QByteArray> `roleNames();`

* **rowCount**
  * 이 method는 단순히 데이터의 세트 수 이므로 컬레선 변수의 아이템 수를 반환한다.
  * ```C++
    int MyDataModel::rowCount(const QModelIndex &p) const
    {
        Q_UNUSED(p)
        return m_data.size();
    }
    ```
* **roleNames**
  * 이 method는 사용자 정의 role enum을 key로 문자열 값을 매핑하고 매핑된 QHash를 리턴한다. roleNames() 메소드에 대한 호출이 될때 맵을 유지하기위해 정적 QHash 변수를 사용한다.
  * ```c++
    QHash<int, QByteArray> MyDataModel::roleNames() const
    {
        static QHash<int, QByteArray> roles;
        roles[TitleRole] = "title";
        roles[ArtistNameRole] = "artistName";
        roles[DurationRole] = "duration";
        return roles;
    }
    ```
* data
  * 이 method는 모델을 참조하는 ListView가 개별 아이템 및 속성에 엑세스 할 수 있는 방법이다. 인수 index는 목록에 있는 요소의 인덱스이고 role은 roleNames 메소드로 매핑된 열거 값이다. 구현은 단순히 데이터를 저장하는 변수(ex m_data) 목록의 인덱스에서 항목을 반환하는 것이다.
  * ```c++
    QVariant MyDataModel::data(const QModelIndex &index, int role) const
    {
        Q_UNUSED(role)

        QVariant value;

        switch(role)
        {
        case TitleRole:
            value = m_data[index.row()]->property("title");
            break;
        case ArtistNameRole:
            value = m_data[index.row()]->property("artistName");
            break;
        case DurationRole:
            value = m_data[index.row()]->property("duration");
            break;
        default:
            break;
        }

        return value;
    }
    ```
## 예제

이 모델에 count속성 (Q_PROPERTY)를 추가해준다,
```C++
Q_PROPERTY(int count READ count NOTIFY countChanged)
```
mymodel.h
```c++
#include <QAbstractListModel>

class MyDataModel : public QAbstractListModel
{
    Q_OBJECT
    Q_PROPERTY(int count READ count NOTIFY countChanged)
public:
    enum ModelRoles {
        TitleRole = Qt::UserRole + 1,
        ArtistNameRole,
        DurationRole
    };

    explicit MyDataModel(QObject * parent = nullptr);

    int rowCount(const QModelIndex &p) const;
    QVariant data(const QModelIndex &index, int role) const;
    QHash<int, QByteArray> roleNames() const;

    int count() const;

public slots:
    void append(QObject *o);
    void insert(QObject *o, int i);
    void remove (int idx);

signals:
    void countChanged(int count);

private:
    QList<QObject*> m_data;
};
```
안에 클래스 내용은 이전의 함수소개시 사용했던 메서드를 사용하면 된다.  
다음은 새로 서브 클래싱한 모델의 사용방법.  
main.cpp
```c++
QQmlApplicationEngine engine;

// QAbstractListModel 서브클래싱 모델 객체 생성.
MyDataModel *model = new MyDataModel();

// 모델 아이템 생성.
QObject * item0 = new QObject();
item0->setProperty("title", "There's Nothing Holdin' Me Back");
item0->setProperty("artistName", "Shawn Mendes");

QObject * item1 = new QObject();
item1->setProperty("title", "Shape of You");
item1->setProperty("artistName", "Ed Sheeran");
//~ 모델 아이템 생성.

// 모델에 아이템 삽입.
model->append(item0);
model->append(item1);
//~ 모델에 아이템 삽입.

engine.rootContext()->setContextProperty("myModel", model);
```
main.qml
```qml
import QtQuick 2.12
import QtQuick.Window 2.12

Window {
    id: window
    visible: true
    width: 640
    height: 480
    title: qsTr("Hello World")

    ListView{
        id: list_
        anchors.fill: parent
        model: myModel

        delegate: Component{
            Text{
                anchors.horizontalCenter: parent.horizontalCenter
                height: 30
                font.bold: true
                text: title + " - " + artistName
            }
        }
    }
}
```
### 결과
![Result](Images/ListView_c++_qml.png)
