using System;

public class Chef 
{
    private List<Food> _foodList;
    public Chef(List<Food> foodList)
    {
        _foodList = foodList;
    }
    public int CalculatePrice()
    {
        return _foodList.Sum(v => v.CalculatePrice());
    }
}
public abstract class Food
{
    protected readonly int _price;
    private Food _next;
    public Food(int price , Food next)
    {
        _price = price;
        _next = next;
    }
    public virtual int CalculatePrice()
    {
        return _next == null
            ? _price
            : _price + _next.CalculatePrice();        
    }
}
public class Steak : Food
{
    public Steak(int price, Food food)
    : base(price, food)
    {

    }
    public override int CalculatePrice()
    {
        return base.CalculatePrice() - 1000;
    }
}
public class Salad : Food
{
    public Salad(int price, Food food)
        :base(price,food)
    {

    }
}
public class Test<T> : Food where T : Food
{
    public Test(int price, Food next) : base(price, next)
    {
        Console.WriteLine(T.CalculatePrice());

    }
}
public class Program
{
    public static void Main()
    {
        List<Food> foodList = new();
        Food setMenu = new Steak(20000, new Salad(10000, null));

        Console.WriteLine($"가겱의 총합은 {setMenu.CalculatePrice()}");
        Console.ReadLine();
    }
}