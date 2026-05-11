# Software Architectures2

[From Chaos to Clarity: Mastering Software Architectures That Scale in the Modern World-Part II | by Bhargava Koya - Fullstack .NET Developer | Jul, 2025 | Medium](https://medium.com/@bhargavkoya56/from-chaos-to-clarity-mastering-software-architectures-that-scale-in-the-modern-world-part-ii-435f3320fdd9)

소프트웨어 설계의 최첨단을 나타내며, 각 아키텍처는 애플리케이션을 기존 경계를 넘어 확장해야할 때 발생하는 특정 문제를 해결한다, 이는 단순한 개념이 아니라 대규모의 복잡한 분산 시스템 및 최신 디지털 경험의 까다로운 요구사항을 처리하는데 사용하는 실전 테스트를 거친 패턴이다

### 마이크로서비스 아키텍처 : 분산형 강국

대규모 애플리케이션을 구축하는 방식에서 가장 중요한 변화를 나타낸다. 마이크로서비스는 단일 모놀로식 애플리케이션을 배포하는 대신 기능을 잘 정의된 API를 통해 통신하는 작고 독립적인 서비스로 나눈다.

#### 마이크로서비스의 핵심 원칙

* 단일 책임
  * 각 서비스는 변경해야하는 한가지 이유가 있어야하며 특정 비지니스 기능에 중점을 두어야한다.
* 분산형 데이터 관리
  * 각 서비스는 데이터와 스키마를 소유하므로 서비스간의 결합이 줄어든다
* 오류격리
  * 한 서비스의 실패가 다른 서비스의 옲로부터 격리되어야 한다
* 독립적인 배포
  * 서비스를 독립적으로 배포할 수 있으므로 릴리즈 주기가 빨라지고 배포 위험이 줄어든다.

#### Netflix의 마이크로 에코 시스템

Netflix의 아키텍처는 다음과 같은 주요 패턴을 보여준다

* API Gateway
  * 외부 요청을 적절한 마이크로서비스 라우팅하는 동시에 인증 및 속도 제한과 같은 교차 편집 문제를 처리
* 서비스검색
  * 서비스가 동적으로 서로를 찾고 통신가능하게 하낟
* Circuit Breaker
  * 서비스를 사용할 수 었을 때 대체 응답을 제공하여 연속 장애를 방지한다
* 부하 분산
  * 증가된 부하를 처리하기 위해 여러 서비스 인스턴스에 요청을 분산한다

#### 장점과 과제

장점

* 서비스 확장에 유리
* 대규모팀의 개발 주기 단축 및 오류 격리

도전과제

* 운영 복잡성 증가 - 네트워크 대기 시간 및 안정성 문제
* 분산 트랜잭션 관리
* 모니터링 및 디버깅 복잡성

### 마이크로 프론트엔드 아키텍쳐 : 프론트엔드로 독립성 확장

마이크로 서비스가 백엔드 아키텍처에 혁명을 일으켯지만 프론트엔드는 모놀리식 단일 페이지 애플리케이션으로 남아 있다, 마이크로 프론트앤드는 마이크로 서비스철학을 프론트앤드로 확장하여 팀이 사용자 인터페이스의 여러 부분에서 독립적으로 작업 할 수있도록한다

#### 마이크로 프론트앤드

애플리케이션을 서로 다른 팀에서 독립적으로 개발, 테스트 및 배포할 수있는 더 작고 관리가능한 부분으로 나눈다, 각 프론트 마이크로 서비스는 특정 비지니스 도메인 또는 기능영역을 담당

**구현 접근 방식**

- **빌드 시간 통합:** 마이크로 프론트엔드는 패키지와 유사하게 빌드 프로세스 중에 구성됩니다.
- **런타임 통합:** 마이크로 프론트엔드는 브라우저에 통합되어 독립적인 배포가 가능합니다.
- **서버 측 통합:** 마이크로 프론트엔드는 클라이언트로 전송되기 전에 서버에서 구성됩니다

```csharp
// Shell Application (Host)
// webpack.config.js
const ModuleFederationPlugin = require('@module-federation/webpack');

module.exports = {
  plugins: [
    new ModuleFederationPlugin({
      name: 'shell',
      remotes: {
        'product-catalog': 'productCatalog@http://localhost:3001/remoteEntry.js',
        'shopping-cart': 'shoppingCart@http://localhost:3002/remoteEntry.js',
        'user-profile': 'userProfile@http://localhost:3003/remoteEntry.js'
      }
    })
  ]
};

// App.js
import React, { Suspense } from 'react';

const ProductCatalog = React.lazy(() => import('product-catalog/ProductCatalog'));
const ShoppingCart = React.lazy(() => import('shopping-cart/ShoppingCart'));
const UserProfile = React.lazy(() => import('user-profile/UserProfile'));

function App() {
  return (
    <div className="app">
      <Header />
      <Suspense fallback={<div>Loading...</div>}>
        <ProductCatalog />
        <ShoppingCart />
        <UserProfile />
      </Suspense>
    </div>
  );
}
```

**마이크로 프론트엔드에 대한 모범 사례**

- **명확한 경계 정의:** 중복을 방지하기 위해 각 마이크로 프론트엔드에 대해 잘 정의된 책임을 설정합니다.
- **디자인 일관성 유지:** 디자인 시스템과 공유 구성 요소 라이브러리를 사용하여 시각적 응집력을 보장합니다.
- **적절한 통신 구현:** 마이크로 간 프론트엔드 통신을 위해 이벤트 버스 또는 공유 상태 관리를 사용합니다.
- **독립적인 배포:** 각 마이크로 프론트엔드는 다른 마이크로 프론트엔드에 영향을 주지 않고 독립적으로 배포할 수 있어야 합니다.
- **보안 고려 사항:** 마이크로 프론트엔드 전반에 걸쳐 적절한 인증 및 권한 부여 전략을 구현합니다.

### 어니언 아키텍쳐 : 유연성을 위한 종속성 반전

**어니언 아키텍쳐의 핵심원칙**

* 종속성 반전
  * 종속성은 코어를 향해 안쪽으로 흐르며, 외부레어어는 내부 레이어에 의존하지만, 그 반대는 그렇지 않다
* 도메인 중심설계
  * 도메인 모델은 핵심 비지니스 논리와 규칙을 나타내는 중앙에 위치
* 인프라 독립성
  * 핵심 비지니스 로직은 데이터베이스, 프레임워크 및 외부 서비스와 독립적으로 유지된다

#### 계층구조

* 도메인 계층
  * 엔터티, 값 개체 및 비지니스 규칙을 포함, 이계층은 외부시스템에 대한 종속성이 없다
* 에플리케이션 계층
  * 비지니스 워크 플로를 조정하고 사용사례를 포함, 도메인 계층에 종속
* 인프라 계층
  * 데이터의 지속성, 외부 서비스 및 프레임워크 별 구현을 처리
* 프레젠테이션 계층
  * 사용자 인터페이와 사용자 상호작용을 관리한다, 웸, 모바일 API 컨트롤러일 수있다.

**사용 예제**

```csharp
// Domain Layer - Core Business Entity
namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; private set; }
        public int CustomerId { get; private set; }
        public List<OrderItem> Items { get; private set; } = new();
        public decimal TotalAmount { get; private set; }
        public OrderStatus Status { get; private set; }
  
        public Order(int customerId)
        {
            CustomerId = customerId;
            Status = OrderStatus.Pending;
        }
  
        public void AddItem(Product product, int quantity)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive");
      
            var item = new OrderItem(product.Id, product.Price, quantity);
            Items.Add(item);
            TotalAmount += item.TotalPrice;
        }
  
        public void ConfirmOrder()
        {
            if (!Items.Any()) throw new InvalidOperationException("Cannot confirm empty order");
            Status = OrderStatus.Confirmed;
        }
    }
}

// Application Layer - Use Cases
namespace ECommerce.Application.UseCases
{
    public class CreateOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
  
        public CreateOrderUseCase(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }
  
        public async Task<Order> ExecuteAsync(CreateOrderRequest request)
        {
            var order = new Order(request.CustomerId);
      
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) throw new ProductNotFoundException(item.ProductId);
          
                order.AddItem(product, item.Quantity);
            }
      
            await _orderRepository.SaveAsync(order);
            return order;
        }
    }
}

// Infrastructure Layer - Data Persistence
namespace ECommerce.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
  
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
  
        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
  
        public async Task SaveAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}
```

**어니언 아키텍처의 이점**

- **테스트 가능성:** 비즈니스 로직은 외부 시스템에 대한 종속성 없이 격리하여 테스트할 수 있습니다.
- **유지 보수성:** 관심사를 명확하게 분리하면 코드베이스를 더 쉽게 이해하고 수정할 수 있습니다.
- **유연성:** 비즈니스 로직에 영향을 주지 않고 인프라를 변경할 수 있습니다.
- **프레임워크 독립성:** 핵심 비즈니스 로직은 특정 프레임워크나 기술에 연결되지 않습니다.

### 이벤트 기반 아키텍쳐 : 반응형 시스템 구축

이벤트 기반아키텍쳐(EDA)는 기존의 요청-응답패턴에서 이벤트 발생 시 대응하는 반응형 시스템으로의 패러다임 전환을 나타낸다

**이벤트 기반 아키텍쳐의 핵심 개념**

* 이벤트 : 시스템에서 중요한 발생 또는 상태변경
* 이벤트 생산자 : 이벤트를 생산하고 개시하는 구성요소
* 이벤트 소비자 : 이벤트를 구독하고 처리하는 구성요소
* 이벤트 브로커 : 이벤트 배포 및 라우팅을 용이하게 하는 미들웨어

**구현 패턴**

* 게시-구독 패턴 : 개시자는 주제에 이벤트를 보내고 구독자는 관심 있는 이벤트를 받는다
* 이벤트 스트리밍 : 이벤트는 변경할 수 없는 로그에 저자오디며 재생할 수 있다.
* 이벤트 소싱 : 시스템 상태는 일련의 이벤트에서 파생


**Event-Driven Architecture의 이점**

- **느슨한 결합:** 구성 요소는 서로에 대해 직접 알 필요가 없습니다.
- **확장성:** 이벤트는 비동기식으로 처리되고 여러 소비자에게 분산될 수 있습니다.
- **복원력:** 일부 구성 요소에 장애가 발생하더라도 시스템이 계속 작동할 수 있습니다.
- **실시간 처리:** 이벤트가 발생할 때 처리할 수 있어 실시간 응답이 가능합니다.

**이벤트 기반 시스템을 위한 모범 사례**

- **이벤트 디자인:** 이벤트 페이로드에 JSON을 사용하고 메타데이터 봉투를 포함합니다.
- **멱등성:** 이벤트 핸들러가 동일한 이벤트를 여러 번 안전하게 처리할 수 있는지 확인합니다.
- **오류 처리:** 배달 못한 편지 대기열 및 재시도 메커니즘을 구현합니다.
- **모니터링:** 이벤트 흐름 및 처리 메트릭을 추적합니다.
- **스키마 진화:** 이벤트 스키마 변경을 계획하고 이전 버전과의 호환성을 유지합니다.

## 일반적인 성능 병목 현상 및 솔루션

1. **데이터베이스 병목 현상:**

* **문제:** 느린 쿼리, 잠금 경합, 부적절한 인덱싱
* **솔루션:** 쿼리 최적화, 적절한 인덱싱, 읽기 전용 복제본, 데이터베이스 샤딩.

**2. 메모리 병목 현상:**

* **문제:** 메모리 누수, 비효율적인 데이터 구조, 과도한 객체 생성.
* **솔루션:** 메모리 프로파일링, 가비지 수집 튜닝, 개체 풀링.

**3. 네트워크 병목 현상:**

* **문제:** 높은 대기 시간, 대역폭 제한, 수다스러운 통신.
* **솔루션:** CDN, 연결 풀링, 데이터 압축, 일괄 처리.

**4. CPU 병목 현상:**

* **문제:** 비효율적인 알고리즘, 동기 처리, 열악한 병렬화.
* **솔루션:** 알고리즘 최적화, 비동기 처리, 병렬 컴퓨팅.

## 확장성 모범 사례

**캐싱 전략:

- **다단계 캐싱 구현(브라우저, CDN, 애플리케이션, 데이터베이스)
- 캐시 무효화 전략을 사용하여 데이터 일관성
  유지 - 캐시 적중률 모니터링 및 캐시 크기 최적화

**로드 밸런싱:

- **여러 서버에 트래픽 분산
- 상태 확인을 사용하여 실패한
  인스턴스로 트래픽을 전송하지 않도록 - 필요한 경우 고정 세션 구현

**데이터베이스 최적화:

- **데이터베이스 샤딩을 사용하여 여러 데이터베이스에 데이터 배포
- 읽기 집약적인 워크로드를 처리하기 위해 읽기 전용 복제본 구현
- 쿼리 최적화 및 적절한 인덱스 사용

**모니터링 및 경고:

- **모든 시스템 구성 요소
  에 대한 포괄적인 모니터링 구현 - 성능 저하에 대한 사전 경고 설정
- 분산 추적을 사용하여 요청 흐름 이해
