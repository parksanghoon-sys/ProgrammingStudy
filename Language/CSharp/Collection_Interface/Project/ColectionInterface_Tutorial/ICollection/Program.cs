using System.Collections;

class Program
{
    static void Main()
    {
        int[] srcarr = new int[3] { 1, 2, 4 };
        int[] dstarr = new int[5] { 11, 12, 13, 14, 15 };
        Queue<int> q = new Queue<int>();
        srcarr.CopyTo(dstarr, 2);
        // dstarr 개체의 인덱스 2인덱스부터 그뒤에 3개전부 srcarr 개체에 보관된 요소들을 복사
        View(dstarr);
    }
    private static void View(ICollection colection)
    {
        Console.WriteLine("Count :{0}",colection.Count);
        foreach(var obj in colection)
        {
            Console.WriteLine("{0}", obj);
        }
        Console.WriteLine();
    }
}
