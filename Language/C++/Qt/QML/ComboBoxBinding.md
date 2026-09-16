# ComboBox Binding
qml 과 c++ 사이에 Combobox model 과 선택시 인덱스 모두 바인딩을 해주어야한다.  
뷰모델이 qml과 연결이 되어 있으며 따로 SerialModel이라는 모델을 만들어 ViewModel에서 참조를 해 Model로 Serial을 연결하도록 구현하였다.  
아래 예를보자  
<br>

SerialPortViewModel.cpp
```c++
#include "ViewModel/SerialPortViewModel.h"
#include "Model/SerialPortModel.h"

struct SerialPortViewModel::privateStruct
{
    SerialPortModel mSerialModel;
    int m_sleectSerialPorNametIndex = 0;
    int m_sleectSerialbaudRatesIndex = 0;
};

SerialPortViewModel::SerialPortViewModel(QObject *parent)
    :QObject(parent),d(new privateStruct)
{

}

SerialPortViewModel::~SerialPortViewModel()
{

}

QStringList SerialPortViewModel::getSerialPorts() const
{
    return d->mSerialModel.getPortName();
}

QStringList SerialPortViewModel::getBaudRates() const
{
    return d->mSerialModel.getBaudRate();
}

int SerialPortViewModel::selectedPortNameIndex() const
{
    return d->m_sleectSerialPorNametIndex;
}

void SerialPortViewModel::setSelectedPortNameIndex(int index)
{
    d->m_sleectSerialPorNametIndex = index;
    emit selectedPortNameIndex();
}

int SerialPortViewModel::selectedBaudRateIndex() const
{
    return d->m_sleectSerialbaudRatesIndex;
}

void SerialPortViewModel::setSelectedBaudRateIndex(const int &index)
{
    d->m_sleectSerialbaudRatesIndex = index;
    emit selectedBaudRateIndex();
}
```
SerialPortModel.cpp
```c++
#include "SerialPortModel.h"
#include <QSerialPort>
#include <QSerialPortInfo>

struct SerialPortModel::privateStruct
{
    QSerialPort m_serialPort;
    QStringList m_comPorts;
    QStringList m_baudRates;

    bool m_isOpen = false;
};

SerialPortModel::SerialPortModel(QObject *parent)
    :QObject(parent),d(new privateStruct)
{
    d->m_comPorts = this->getConnectedSerialPortNames();
    d->m_baudRates = QStringList({ "9600", "19200", "38400", "57600", "115200"});
}

SerialPortModel::~SerialPortModel()
{

}

QStringList SerialPortModel::getPortName()
{
    return d->m_comPorts;
}

QStringList SerialPortModel::getBaudRate()
{
    return d->m_baudRates;
}

bool SerialPortModel::Open()
{
    auto portName = d->m_comPorts.at(2);
    QString baudRate = d->m_baudRates.at(2);
    d->m_serialPort.setPortName(portName);
    d->m_serialPort.setBaudRate(baudRate.toInt());
    d->m_serialPort.setDataBits(QSerialPort::Data8);
    d->m_serialPort.setParity(QSerialPort::NoParity);
    d->m_serialPort.setStopBits(QSerialPort::OneStop);
    d->m_serialPort.setFlowControl(QSerialPort::NoFlowControl);

    d->m_isOpen = d->m_serialPort.open(this->getOpenMode());
    return d->m_isOpen;
}

QStringList SerialPortModel::getConnectedSerialPortNames()
{
    QSerialPortInfo info;

    auto infomation = info.availablePorts();

    if(infomation.count() == 0)
        return QStringList();

    QStringList portList;
    for(auto itr : infomation)
    {
        portList.append(itr.portName());
    }
    return portList;
}

QIODevice::OpenMode SerialPortModel::getOpenMode(int openMode)
{
    switch(openMode)
    {
    case 0 :
        return QIODevice::ReadOnly;

    case 1 :
        return QIODevice::WriteOnly;

    case 2 :
        return QIODevice::ReadWrite;
    }

    return QIODevice::NotOpen;
}

```
단 
>  Q_INVOKABLE QStringList getSerialPorts() const;  
> `Combobox Model을 연결시`  
> Q_PROPERTY(int selectedPortNameIndex READ selectedPortNameIndex WRITE setSelectedPortNameIndex NOTIFY selectedPortNameIndexChanged)  
> `Combobox Index를 따로 바인딩 해주어야 한다`  
> 여기서 원인은 모르겟지만 setSelectedPortNameIndex 해당 메서드는 public slots 로 선언해주어야한다.


