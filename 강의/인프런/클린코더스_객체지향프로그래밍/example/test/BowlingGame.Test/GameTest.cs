namespace BowlingGame.Test;

public class GameTest
{
    private Game _game;
    public GameTest()
    {
        _game = new Game();
    }
    private void RollMany(int pins, int frames)
    {
        for (int i = 0; i < frames; i++)
            _game.Roll(pins);
    }
    private void RollSpare()
    {
        _game.Roll(5);
        _game.Roll(5);
    }
    [Fact]
    public void CanRoll()
    {
        _game.Roll(0);
    }
    [Fact]
    public void GutterGame()
    {        
        RollMany(0, 20);
        Assert.Equal(0, _game.GetScore());
    }
    [Fact]
    public void AllOnesGame()
    {
        RollMany(1, 20);
        Assert.Equal(20, _game.GetScore());
    }
    [Fact]
    public void OneSpare()
    {
        RollSpare();
        _game.Roll(3);
        RollMany(17, 0);
        Assert.Equal(16, _game.GetScore());
    }
    [Fact]
    public void OneStrike()
    {
        RollStrike();
        _game.Roll(5);
        _game.Roll(3);
        RollMany(16, 0);
        Assert.Equal(26, _game.GetScore());
    }
    [Fact]
    public void PerfectGame()
    {        
        RollMany(10, 10);
        RollStrike();
        RollStrike();
        Assert.Equal(300, _game.GetScore());
    }
    [Fact(Skip = "스킵 방법예시")]
    public void SkippedTestExample()
    {
        // This test is skipped.
    }
    [Theory(Skip = "버그 수정 대기")]
    [InlineData(1, 2)]
    [InlineData(3, 4)]
    public void Add_test(int a, int b)
    {
        Assert.True(true);
    }
    private void RollStrike()
    {
        _game.Roll(10);
    }
}

