# Qml과 C++로 구현하는 GUI어플리케이션
![connect](Images/qmlConnectC++.png)

예제를 통해 QML 과 C++가 연결 하는 방법을 알아보자
```c++
#include <QGuiApplication>
#include <QQmlApplicationEngine>
 
int main(int argc, char *argv[])
{
    QCoreApplication::setAttribute(Qt::AA_EnableHighDpiScaling);
 
    QGuiApplication app(argc, argv);
 
    QQmlApplicationEngine engine;
    engine.load(QUrl(QStringLiteral("qrc:/main.qml")));
    if (engine.rootObjects().isEmpty())
        return -1;
 
    return app.exec();
}
```
QQmlApplicationEngine 객체를 생성하고 main.qml을 로드 시키고 있다.  
<br>
일반적인 방법은 QQmlApplicationEngine의 객체 즉, QQmlEngine의 context에 QObject를 서브 클래싱한 C++클래스 객체를 등록하여 QML에서 등록된 객체에 접근을 할 수 있게한다.  
<br>
QQmlApplicationEngine클래스는 QQmlEngine을 상속 받고 있는데 **QQmlEngine 클래스는 QML 엔진의 rootcontext를 얻을 수 있도록 rootContext() 메서드를 제공한다.**  
<br>
**rootContext()는 QQmlontext포인터를 반환하고 QQmlContext의 setContextProperty()메서드를 통해 C++ 인스턴스를 등록 할 수 있다.**  
<br>
QML에서 C++ 객체의 메서드를 호출하거나 비동기로 시그널을 처리하는 방법이다. Phonebook 클래스는 QObject를 상속 받고있고 setter, getter 메서드를 선언하였다. 여기서 `Q_INVOKEABLE` 를 주목하자
* Q_INVOKEABLE : 컴파일 시점에 moc에 의해 표준 C++로 재성성되고 Q_INVOKEABLE으로 선언된 클래스의 함수는 QML에서 호출이 가능하도록 해준다

아래 예를보자
```c++
#include <QObject>
 
class Phonebook: public QObject{
    Q_OBJECT
public:
    explicit Phonebook(QObject *parent = nullptr);
    virtual ~Phonebook();
 
    Q_INVOKABLE void setName(const QString &name);
    Q_INVOKABLE QString name() const;
 
signals:
    void nameChanged();
 
private:
    QString m_name;
};
```
```c++
#include "phonebook.h"
 
Phonebook::Phonebook(QObject *parent)
    :QObject(parent),
      m_name("")
{
 
}
 
Phonebook::~Phonebook()
{
 
}
 
void Phonebook::setName(const QString &name)
{
    if(name != m_name){
        m_name = name;
        emit this->nameChanged();
    }
}
 
QString Phonebook::name() const
{
    return m_name;
}
```
main 함수에는 phonebook 객체를 생성하고 QML엔진에 "phonebook" 이라는 이름으로 등록한다  
main.cpp
```c++
#include <QGuiApplication>
#include <QQmlApplicationEngine>
#include <QQmlContext>
#include "phonebook.h"
 
int main(int argc, char *argv[])
{
    QCoreApplication::setAttribute(Qt::AA_EnableHighDpiScaling);
 
    QGuiApplication app(argc, argv);
 
    QQmlApplicationEngine engine;
    Phonebook pb;
    engine.rootContext()->setContextProperty("phonebook", &pb);
    engine.load(QUrl(QStringLiteral("qrc:/main.qml")));
    if (engine.rootObjects().isEmpty())
        return -1;
 
    return app.exec();
}
```
```qml
import QtQuick 2.12
import QtQuick.Window 2.12
import QtQuick.Dialogs 1.3
import QtQuick.Controls 2.4
 
Window {
    visible: true
    width: 640
    height: 480
    title: qsTr("Hello World")
 
    Dialog{
        id: inputDialog
        title: qsTr("이름을 입력하세요.")
        anchors.centerIn: parent
        height: 200
        width: 300
        modal: Qt.ApplicationModal
        contentItem: TextField{
            width: 100
            height: 50
            font.pixelSize: 25
            horizontalAlignment: TextInput.AlignHCenter
        }
        standardButtons: Dialog.Ok | Dialog.Cancel
 
        onAccepted: {
            if(contentItem.text.length){
                phonebook.setName(contentItem.text)
            }
        }
    }
 
    Connections{
        target: phonebook
        onNameChanged:{
            name.text = phonebook.name()
        }
    }
 
    Row{
        height: 50
        anchors.centerIn: parent
 
        Text {
            id: name
            width: 150
            font.pixelSize: 20
            font.bold: true
            anchors.verticalCenter: parent.verticalCenter
            elide: Text.ElideRight
        }
        Button{
            width: 70
            height: 50
            text: qsTr("입력")
            onClicked: {
                inputDialog.open()
            }
        }
    }
}
```
> Q_PROPERTY(type name READ name WRITE setname NOTIFY nameChanged) 메서드를 이용해서도 qml 과 C++을 바인딩 연결이 가능하다. 추후 Property 글에서 자세하게 설명하겠다.