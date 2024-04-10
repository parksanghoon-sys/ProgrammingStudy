using System.Collections;
/// <summary>
/// 수강생 모델
/// </summary>
class Tutee
{
	public string Name { get; private set; }
	public Tutee(string name)
	{
		Name = name;
	}
    public override string ToString()
    {
		return string.Format($"수강생 이름 : {Name}");
    }
}
/// <summary>
/// 강사 모델
/// </summary>
class Tutor
{
    public string Name { get; private set; }
    public Tutor(string name)
    {
        Name = name;
    }
    public override string ToString()
    {
        return string.Format($"강사 이름 : {Name}");
    }
}
/// <summary>
/// 강의실 클래스
/// </summary>
class LectureRoom : IEnumerable, IEnumerator
{
    private Tutor _tutor = null;
    ArrayList _tutees = new ArrayList();
    private int _now;
    public LectureRoom()
    {
        Reset();
    }
    public void AddTutee(Tutee tutee)
    {
        _tutees.Add(tutee);
    }
    public bool IsInTutor(Tutor tutor)
    {
        if(_tutor == null)
        {
            _tutor = tutor;
            return true;
        }
        return false;
    }
    public object Current
    {
        get 
        {
            if (_tutor == null) return _tutees[_now]; //강사가 없을때
            if (_now == 0) return _tutor;
            return _tutees[_now - 1];
        }
    }
    /// <summary>
    /// IEnumerator 를 상속 받았기 떄문에 자기자신을 호출
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator()
    {
        return this;
    }
    /// <summary>
    /// 현재 위치를 기억하기위해 _now 를 증가시킨다
    /// 강사가 없으면 현재위치인 now가 수강생보다 적을시 ture를 반환, 그렇지 않을시 Reset을 호출하여 now를 초기화위치로 변환하고 false를 반환한다.
    /// 강사가 있다면 now가 수강생보다 작거나 같을때 true를 반환, 반대일시 초기화
    /// </summary>
    /// <returns></returns>
    public bool MoveNext()
    {
        _now ++;
        if(_tutor == null ) //강사가 없을때
        {
            if (_now < _tutees.Count) return true;
            Reset();
            return false;
        }
        // 강사가 있을 떄
        if (_now <= _tutees.Count) return true;
        Reset();
        return false;
    }
    /// <summary>
    /// 초기화 메서드
    /// </summary>
    public void Reset()
    {
        _now = -1;
    }
}
class Program
{
    static void Main(string[] args)
    {
        LectureRoom lr = new LectureRoom();
        lr.AddTutee(new Tutee("홍길동"));
        lr.AddTutee(new Tutee("강감찬"));

        lr.IsInTutor(new Tutor("언휴"));
        foreach (object obj in lr)
        {
            Console.WriteLine(obj);
        }
    }
}