# Qt Property(속성) 시스템
## Property 란?
프로퍼티는 객체 지향 프로그래밍에서 객체를 모델링하고 추상화 할 때 정의한다. 예를 들어 `QWidget`이라면 그 폭이나 높이 , `QLabel`이라면 문자열등의 프로퍼티가 있다.  
<br>
일반적으로 C++에서 이러한 프로퍼티 값은 getter라고 하는 함수를 통해 얻고 setter라고 하는 함수로 설정한다. 예를들어 QLabel 의경우 text() 가 getter이고 setText()가 setter이 된다. 이와 같이 c++에서는 클래스 메서드를 통해 프로퍼티를 다루는 반면 QML로 엑세스하는 경우에는 getter/setter이 QML엔진측에 노출되고 자동적으로 사용되에 QML에서 속성은 변수처럼 사용된다.  
<br>

## Q_PROPERTY 매크로
C++클래스에서 속성을 만들 시 Q_PROPERTY 매크로를 사용하면 된다. 최소한 필요한 것은 프로퍼티의 type과 getter이다. 읽기와 쓰기를 모두 허용하려면 setter도 필요하다. QML로 바인딩 되는경우 값이 변경이 되었음을 알리는 SIGNAL도 선언되어야한다. 기본서식은 다음과 같다
> Q_PROPERTY(type name READ name WRITE setname NOTIFY nameChanged)

Qt 5에서 사용할수 있는 Q_PROPERTY의 전체옵션은 다음과 같다
```c++
Q_PROPERTY(type name
           (READ getFunction [WRITE setFunction] |
            MEMBER memberName [(READ getFunction | WRITE setFunction)])
           [RESET resetFunction]
           [NOTIFY notifySignal]
           [REVISION int]
           [DESIGNABLE bool]
           [SCRIPTABLE bool]
           [STORED bool]
           [USER bool]
           [CONSTANT]
           [FINAL]
           [REQUIRED])
```
다음은 Qt 6 에서 유효한 Q_PROPERTY 매크로다. BINDABLE 이 추가된 점을 빼면 Qt 5 와 큰 차이는 없다.
```c++
Q_PROPERTY(type name
           (READ getFunction [WRITE setFunction] |
            MEMBER memberName [(READ getFunction | WRITE setFunction)])
           [RESET resetFunction]
           [NOTIFY notifySignal]
           [REVISION int | REVISION(int[, int])]
           [DESIGNABLE bool]
           [SCRIPTABLE bool]
           [STORED bool]
           [USER bool]
           [BINDABLE bindableProperty]
           [CONSTANT]
           [FINAL]
           [REQUIRED])
```
예를 들어 int형의 count라고 하는 Property를 가지는 Counter 클래스를 생각해볼때 아래 예제와 같이 작성할 것이다.
```c++
class Counter : public QObject
{
    Q_OBJECT
    Q_PROPERTY(int count READ count WRITE setCount NOTIFY countChanged)
public:
    Counter(QObject *parent = 0);
 
    int count() const
    {
        return m_count;
    }
 
    void setCount(int value)
    {
        if (m_count != value) {
            m_count = value;
            emit countChanged();
        }
    }
 
singals:
    void countChanged();
 
private:
    int m_count;
};
```
`NOTIFY`로 지정하는 SIGNAL은 그 밖에 동시에 변화하는 프러퍼티가 있으면 `동시에 여러개를` 공유해줘도 괜찮다, 속성 type은 QVariant에서 지원하는 모든 유형이거나 사용자 정의 유형일 수 있다. 사용자 정의 유형은 해당 값을 QVariant 개체에도 저장할 수 있도록 `Q_DECLARE_METATYPE()` 매크로를 사용해 등록해야한다.  
<br>
이와 같이 `Q_PROPERTY` 매크로를 추가하고 관련된 메소드를 선언 및 정의 해 프로퍼티로 취급할수 있다. qml과 c++ 상호작용을 위해 필수적이며 편리한 기능이다. 다만 프로퍼티의 수가 많아지면 생성이 귀찮아져온다. 그래서 등장한 것이 MEMBER 지정자 이다.  
<br>
`MEMBER`의 `Q_PROPERTY`매크로의 기술시 예는 아래와같다.
``` c++
class Counter : public QObject
{
    Q_OBJECT
    Q_PROPERTY(int count MEMBER m_count NOTIFY countChanged)
public:
    Counter(QObject *parent = 0);
 
singals:
    void countChanged();
 
private:
    int m_count;
};
```
`MEMBER는` READ 또는 WRITE 지정자와 함께 사용할 수도 있다. `READ` 와 `MEMBER` 를 지정하는 경우에는 g*etter 를 작성할 필요가 있지만 setter 는 moc 가 작성한다.* 반대로 WRITE 와 `MEMBER` 를 지정하는 경우는 setter 를 작성할 필요가 있지만 getter 는 moc 가 작성한다. `MEMBER` 와 READ, WRITE 의 모두를 지정하는 것은 의미가 없기 때문에 지원의 대상 밖이다. setter 로 유효성 체크를 실시하고 싶은 경우등에 setter 만 작성하고 getter 는 생략하고 `MEMBER` 를 이용하는 것이 주된 사용법이다.  
<br>

## Meta-Object System 으로 읽고 쓰기
속성의 이름을 제외하고 소유 클래승에 대해 아는것이 없어도 일반함수 QObject::property() 및 QObject::setProperty()를 사용하여 속성을 읽고 쓸 수 있다.
```c++
QPushButton *button = new QPushButton;
button->setDown(true);

QObject *object = button;
object->setProperty("down", true);
// 이름으로 속성에 엑세스 하면 컴파일 타임에 알지 모하는 클래스에 엑세스 할수 있다. QObject, QMetaObject, QMetaProperties를 쿼리하여 런타임에 클래스의 속성을 알 수 있다.

QObject *object = ...
const QMetaObject *metaobject = object->metaObject();
int count = metaobject->propertyCount();
for (int i=0; i<count; ++i) {
    QMetaProperty metaproperty = metaobject->property(i);
    const char *name = metaproperty.name();
    QVariant value = object->property(name);
    ...
}
```
위의 스니펫에서 QMetaObject::property()는 알 수 없는 클래스에 정의된 각 속성에 대한 메타데이터를 가져오는 데 사용된다. 속성 이름은 메타데이터에서 가져와 QObject::property()에 전달되어 현재 객체의 속성 값을 가져온다. 다시 말하지만 property() 와 setProperty() 는 퍼포먼스적으로는 적절하게 생성된 getter / setter 보다 안 좋기 때문에 C++ 측으로부터 빈번하게 액세스 하는 프로퍼티에는 적합하지 않을 수도 있다.

## Dynamic Properties
QObject::setProperty()를 사용하면 런타임에 클래스의 인스턴스에 새 속성을 추가할 수도 있다. 주어진 이름의 속성이 QObject에 존재하지 않는 경우(즉, Q_PROPERTY로 선언되지 않은 경우), 주어진 이름과 값을 가진 새 속성을 자동으로 QObject에 추가한다.
```c++
MyClass myclass; // MyClass is Subclass of QObject.
myclass.setProperty("text", "Hello world");
qDebug() << myclass.property("text").toString();
```