# cinject 의 to  method 이해
## to 사용
```c++
c.bind<IWalker, IRunner, IJumper, ICrawler, ISwimmer, IWaterConsumer, Human>().to<Human>().inSingletonScope();
```
여기서 to 의 사용을 알아보자 이전 bind 를통해 ComponentBuilderBase객체를 통해 각 인터페이스 및 클래스들이 전달이 되었다.  

## to 내부 구현
```c++ 
template<typename TImplementation>
StorageConfiguration<InstanceStorage<TImplementation, ConstructorFactory<TImplementation>>>
    to()
{
    typedef InstanceStorage<TImplementation, ConstructorFactory<TImplementation>> InstanceStorageType;

    // Create instance holder
    auto instanceStorage = std::make_shared<InstanceStorageType>(ConstructorFactory<TImplementation>());
    // make_shared 기본 할당자를 사용하여 하나의 인수에서 작성된 할당된 객체를 가리키는 shared_ptr을 만들고 반환한다.
    // 지정된 형식의 개체와 
    registerType<TImplementation, InstanceStorageType, TComponents...>(instanceStorage);

    return StorageConfiguration<InstanceStorageType>(instanceStorage);
}
```
먼저 InstanceStorageType 을 typedef 를 이용해 타입을 만들어준다.  
다음 StorageConfiguration 타입의 ConstructorFactory를 이용 받은 클래스를 인스턴스 만든다.  


우선 StorageConfiguration 클래스를 알아보자...

이제 InstanceStore에서 제네릭으로 넘어온 StorageConfiguration 에대해 알아보자.
```c++
template<typename TInstanceStorage>
class StorageConfiguration
{
public:
    explicit StorageConfiguration(std::shared_ptr<TInstanceStorage> storage) :
        storage_(storage)
    {}

    StorageConfiguration& inSingletonScope()
    {
        storage_->setSingleton(true);

        return *this;
    }

    StorageConfiguration& alias(const std::string& name)
    {
        storage_->setName(name);

        return *this;
    }

    StorageConfiguration& alias(std::string&& name)
    {
        storage_->setName(name);

        return *this;
    }

private:
    std::shared_ptr<TInstanceStorage> storage_;
};
```
해당 클래스는 템플릿을 이용하여 공유 포인터 함수를 만들어 주는 역할을 한다. 또한 약속된 함수의 사용 특정 하는것만 받을 예정이다. 해당 함수들을 사용한다 특정 사용하는 클래스는 아래와 같다.  

```c++
template<typename TImplementation, typename TFactory>
class InstanceStorage
{
public:
    explicit InstanceStorage(TFactory factory) :
        factory_(factory)
    {}

    virtual std::shared_ptr<TImplementation> getInstance(InjectionContext* context)
    {
        if (!isSingleton_)
        {
            return createInstance(context);
        }

        if (instance_ == nullptr)
        {
            instance_ = createInstance(context);
        }

        return instance_;
    }

    void setSingleton(bool value) { isSingleton_ = value; }

    void setName(const std::string& name) { name_ = name; }
    void setName(std::string&& name) { name_ = name; }

private:
    std::shared_ptr<TImplementation> createInstance(InjectionContext* context)
    {
        ContextGuard guard(context, make_component_type<TImplementation>(!name_.empty() ? name_ : type_name<TImplementation>::value()));

        guard.ensureNoCycle();

        return factory_.createInstance(context);
    }

    TFactory factory_;
    bool isSingleton_ = false;
    std::shared_ptr<TImplementation> instance_;
    std::string name_;
};
``` 
클래스 명만 봐도 각 Instance의 Store 이다. TImplementation 과 TFactory 를 템플릿으로 받으며 TFactory 를 맴버 변수로 같는다. singleton 객체로 설저이 되었는지 확인하고 각 이름을 부여할수 있도록한다.  
만약 singleton이면 기존 처음에 넣엇던 std::shared_ptr<TImplementation> instance_; 를 반환해주고 아닐시 createInstance method를 통해 객체를 생성해준다.  

ConstructorFactory를 알아보자
```c++
template<typename T, class TEnable = void>
class ConstructorFactory
{
    static_assert(always_false<T>::value, "Could not deduce any ConstructorFactory");
};


// Factory for trivial constructors with no arguments
template<typename TInstance>
class ConstructorFactory<TInstance, typename std::enable_if<!has_constructor_injection<TInstance>::value&& std::is_constructible<TInstance>::value>::type>
{
public:
    std::shared_ptr<TInstance> createInstance(InjectionContext* context)
    {
        return std::make_shared<TInstance>();
    }
};

// Factory for automatic injection for one to ten arguments
template<typename TInstance>
class ConstructorFactory<TInstance, typename std::enable_if<!has_constructor_injection<TInstance>::value && !std::is_constructible<TInstance>::value>::type>
{
public:
    std::shared_ptr<TInstance> createInstance(InjectionContext* context)
    {
        return try_instantiate(
            ctor_arg_resolver(context),
            ctor_arg_resolver(context),
            ctor_arg_resolver(context),
            ctor_arg_resolver(context),
            ctor_arg_resolver(context),
            ctor_arg_resolver(context),
            ctor_arg_resolver(context),
            ctor_arg_resolver(context),
            ctor_arg_resolver(context),
            ctor_arg_resolver_1st<TInstance>(context));
    }

private:
    template<typename TArg, typename TNextArg, typename ... TRestArgs>
    typename std::enable_if<std::is_constructible<TInstance, TArg, TNextArg, TRestArgs ...>::value, std::shared_ptr<TInstance>>::type
        try_instantiate(TArg a1, TNextArg a2, TRestArgs ... args)
    {
        return std::make_shared<TInstance>(a1, a2, args...);
    }

    template<typename TArg, typename TNextArg, typename ... TRestArgs>
    typename std::enable_if<!std::is_constructible<TInstance, TArg, TNextArg, TRestArgs ...>::value, std::shared_ptr<TInstance>>::type
        try_instantiate(TArg a1, TNextArg a2, TRestArgs ... args)
    {
        return try_instantiate(a2, args...);
    }

    template<typename TArg>
    typename std::enable_if<std::is_constructible<TInstance, TArg>::value, std::shared_ptr<TInstance>>::type
        try_instantiate(TArg arg)
    {
        return std::make_shared<TInstance>(arg);
    }

    template<typename TArg>
    typename std::enable_if<!std::is_constructible<TInstance, TArg>::value, std::shared_ptr<TInstance>>::type
        try_instantiate(TArg arg)
    {
        static_assert(always_false<TInstance>::value, "Could not find any suitable constructor for injection. Try explicitly mark the constructor using CINJECT macro");
    }
};
```
```c++
auto instanceStorage = std::make_shared<InstanceStorageType>(ConstructorFactory<TImplementation>());
```
해당 구문에서 ConstructorFactory 안에 Hummun 클래스를 넣어서 InstanceStorageType 타입의 shared_ptr의 객체를 instanceStrorage르 만들어 준다.  
<br>
그후
```c++
registerType<TImplementation, InstanceStorageType, TComponents...>(instanceStorage);  
```
를 이용해 인스턴스를 넣어주는거같은데 .....
  

## to 실제 반환 값.
---
to 사용 반환



```c++

```

