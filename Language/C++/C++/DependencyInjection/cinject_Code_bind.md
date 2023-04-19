# cinject 의 Bind method 이해
## singleton 서비스 등록 각 인터페이스 사용시
```c++
c.bind<IWalker, IRunner, IJumper, ICrawler, ISwimmer, IWaterConsumer, Human>().to<Human>().inSingletonScope();
```
## 기본
```c++
	class Container
	{
		template<typename ... TComponents>
		friend class ComponentBuilderBase;
        // private 및 protected 맴버에 대한 엑세스 권한을 부여한다.
        // 클래스간 우정은 한 클래스가 다른 클래스의 비공개 맴버에 엑세스 할수있도록 하는 C++ 의 메커니즘이다, 
        // firend class 선언은 클래스가 해당 클래스의 맴버 또는 파생클래스가 아닌 경우에도 클래스가 다른 클래스의 전용 및 보호 맴버에 엑세스 할 수있음을 선언하는방법
        
	public:
		Container() = default;
		explicit Container(const Container* parentContainer) : parentContainer_(parentContainer) {}

        template<typename... TArgs>
		ComponentBuilder<TArgs...> bind();

        template<typename TVectorWithInterface>
		typename std::enable_if<is_vector<TVectorWithInterface>::value &&
			!is_shared_ptr<typename trim_vector<TVectorWithInterface>::type>::value &&
			!std::is_reference<TVectorWithInterface>::value,
			std::vector<std::shared_ptr<typename trim_vector<TVectorWithInterface>::type>>>::type
			get(InjectionContext* context = nullptr);


        template<typename TVectorWithInterface>
		typename std::enable_if<is_vector<TVectorWithInterface>::value&&
			is_shared_ptr<typename trim_vector<TVectorWithInterface>::type>::value &&
			!std::is_reference<TVectorWithInterface>::value,
			std::vector<typename trim_vector<TVectorWithInterface>::type>>::type
			get(InjectionContext* context = nullptr);

            ...

    private:
    void findInstanceRetrievers(std::vector<std::shared_ptr<IInstanceRetriever>>& instanceRetrievers, const component_type& type) const;
		const Container* parentContainer_ = nullptr;
		std::unordered_map<component_type, std::vector<std::shared_ptr<IInstanceRetriever>>, component_type_hash> registrations_;
    }
```
friend class ComponentBuilderBase; 를 선언함으로써 Container 에서 ComponentBuilderBase 의 bind,to,get을 사용이 가능하게 된다.  
기본 Container 클래스. 해당 클래스를 이용해서 의존성에 넣고 사용을 하곤 한다.

## bind 기능...
```c++
	template<typename... TArgs>
	ComponentBuilder<TArgs...> Container::bind()
	{
		return ComponentBuilder<TArgs...>(this);
	}

    template<typename ... TComponents>
	class ComponentBuilder : public ComponentBuilderBase<TComponents...>
	{
	public:
		explicit ComponentBuilder(Container* container) :
			ComponentBuilderBase<TComponents...>(container)
		{}
	};

    template<typename ... TComponents>
	class ComponentBuilderBase
	{
	public:
		explicit ComponentBuilderBase(Container* container) :
			container_(container)
		{}
    }
    
```
 bind메소드가 인수와 함께 호출 되면 IWalker, IRunner, IJumper, ICrawler, ISwimmer, IWaterConsumer, Human해당 유형이 **클래스의 템플릿 인수**로 사용됩니다 ComponentBuilder.

클래스는 클래스 ComponentBuilder에서 파생되는 템플릿 기반 클래스입니다 ComponentBuilderBase. 클래스 는 멤버 변수를 설정하는 데 사용되는 생성자에서 포인터를 ComponentBuilderBase사용합니다 . 클래스 는 기본 클래스의 생성자에 전달되는 생성자에서도 포인터를 사용합니다 .Containercontainer_ComponentBuilderContainer

따라서 c.bind<IWalker, IRunner, IJumper, ICrawler, ISwimmer, IWaterConsumer, Human>()가 호출되면 새 객체를 생성하고 해당 객체를 생성자에 대한 인수로 ComponentBuilder<IWalker, IRunner, IJumper, ICrawler, ISwimmer, IWaterConsumer, Human>전달합니다 .Container로 명시 적으로 생성 된것은 아니다. 

## 결론
---
**IWalker, IRunner, IJumper, ICrawler, ISwimmer, IWaterConsumer, Human 클래스에 대해 Container 개체가 명시적으로 생성되지 않지만 이러한 유형의 구성 요소는 ComponentBuilder 및 ComponentBuilderBase를 사용하여 Container개체에 추가 되었다. Container클래스에서 선언한 friend ComponentBuilderBase를 통해 접근한다.** 

## Next
[to 를 알아보자 ](/Language/C%2B%2B/C%2B%2B/DependencyInjection/cinject_Code_to.md)