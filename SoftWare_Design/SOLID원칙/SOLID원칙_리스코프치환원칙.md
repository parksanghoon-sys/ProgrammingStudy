# **LSP : Liskov Substitution Principle**
## 의미
> ### **부모 클래스를 사용하는 인스턴스에 해당 클래스를 상속하는 자식 클래스를 할당 하더라도 모든 기능이 정상적으로 작동 해야한다 라는 의미** 
<br>
리스코프 치환 법칙을 설명할 떈 일반적으로 클래스의 상속을 통해 설명하고 증명하지만 아키텍쳐 관점에서는 좀 더 넓은 의미로 적용이 될수있다. "상속으로 이어진 관계에서 예상 못할 행동들을 하지말". 라는의미  
<br><br>

## 문제점
SOLID 원칙중'장애상황'. '버그'와 가장 가까운부분이 LSP원칙이다.  
인터페이스나 상위 정의된 부분과 실제 구현된 부분이 "예상"과 다르면 잘못 사용하게 될 가능성이 높아진다.  
<br><br>

## 예시
### 리스코프치환을 준수하지 않은 코드
```c#
/**
 * 직사각형 클래스
 *
 * @author RWB
 * @since 2021.08.14 Sat 11:12:44
 */
public abstract class Rectangle
{
    protected int width;
    protected int height;
    
    /**
     * 너비 반환 함수
     *
     * @return [int] 너비
     */
    public virtual int getWidth()
    {
        return width;
    }
    
    /**
     * 높이 반환 함수
     *
     * @return [int] 높이
     */
    public virtual int getHeight()
    {
        return height;
    }
    
    /**
     * 너비 할당 함수
     *
     * @param width: [int] 너비
     */
    public virtual void setWidth(int width)
    {
        this.width = width;
    }
    
    /**
     * 높이 할당 함수
     *
     * @param height: [int] 높이
     */
    public virtual void setHeight(int height)
    {
        this.height = height;
    }
    
    /**
     * 넓이 반환 함수
     *
     * @return [int] 넓이
     */
    public int getArea()
    {
        return width * height;
    }
}
```
```c#
/**
 * 정사각형 클래스
 *
 * @author RWB
 * @since 2021.08.14 Sat 11:19:07
 */
public class Square : Rectangle
{
    /**
     * 너비 할당 함수
     *
     * @param width: [int] 너비
     */    
    public override void setWidth(int width)
    {
        super.setWidth(width);
        super.setHeight(getWidth());
    }
    
    /**
     * 높이 할당 함수
     *
     * @param height: [int] 높이
     */    
    public override void setHeight(int height)
    {
        super.setHeight(height);
        super.setWidth(getHeight());
    }
}

```
```c#
/**
 * 메인 클래스
 *
 * @author RWB
 * @since 2021.06.14 Mon 00:06:32
 */
public class Main
{
    /**
     * 메인 함수
     *
     * @param args: [String[]] 매개변수
     */
    public static void main(String[] args)
    {
        Rectangle rectangle = new Rectangle();
        rectangle.setWidth(10);
        rectangle.setHeight(5);
        
        System.out.println(rectangle.getArea());
    }
    // =>25
}

```
### 리스코프치환을 준수한 코드
```c#
/**
 * 사각형 객체
 *
 * @author RWB
 * @since 2021.08.14 Sat 11:39:02
 */
public abstract class Shape
{
    protected int width;
    protected int height;
    
    /**
     * 너비 반환 함수
     *
     * @return [int] 너비
     */
    public int getWidth()
    {
        return width;
    }
    
    /**
     * 높이 반환 함수
     *
     * @return [int] 높이
     */
    public int getHeight()
    {
        return height;
    }
    
    /**
     * 너비 할당 함수
     *
     * @param width: [int] 너비
     */
    public virtual void setWidth(int width)
    {
        this.width = width;
    }
    
    /**
     * 높이 할당 함수
     *
     * @param height: [int] 높이
     */
    public virtual void setHeight(int height)
    {
        this.height = height;
    }
    
    /**
     * 넓이 반환 함수
     *
     * @return [int] 넓이
     */
    public int getArea()
    {
        return width * height;
    }
}
/**
 * 직사각형 클래스
 *
 * @author RWB
 * @since 2021.08.14 Sat 11:12:44
 */
class Rectangle extends Shape
{
    /**
     * Rectangle 생성자 함수
     *
     * @param width: [int] 너비
     * @param height: [int] 높이
     */
    public Rectangle(int width, int height)
    {
        setWidth(width);
        setHeight(height);
    }
}
/**
 * 정사각형 클래스
 *
 * @author RWB
 * @since 2021.08.14 Sat 11:19:07
 */
class Square extends Shape
{
    /**
     * Square 생성자 함수
     *
     * @param length: [int] 길이
     */
    public Square(int length)
    {
        setWidth(length);
        setHeight(length);
    }
}
/**
 * 메인 클래스
 *
 * @author RWB
 * @since 2021.06.14 Mon 00:06:32
 */
public class Main
{
    /**
     * 메인 함수
     *
     * @param args: [String[]] 매개변수
     */
    public static void main(String[] args)
    {
        Shape rectangle = new Rectangle(10, 5);
        Shape square = new Square(5);
        System.out.println(rectangle.getArea());
        System.out.println(square.getArea());
    }
}
 // => 50
 // => 25

```

