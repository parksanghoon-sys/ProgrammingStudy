using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingGame;

public class Game
{
    private int[] _rolls = new int[21];
    private int currentRoll = 0;
    private int _score = 0;
    public int GetScore()
    {
        _score = 0;
        int fistRollInFrame = 0;
        for (int frame = 0; frame < 10; frame++)
        {
            if (IsSpare(fistRollInFrame))
            {
                _score += 10 + NextBallForSpare(fistRollInFrame);
                fistRollInFrame += 2;
            }
            else if (IsStrike(fistRollInFrame))
            {
                _score += NextBallsForStrike(fistRollInFrame);
                fistRollInFrame += 1;
            }
            else
            {
                _score += NextBallsForFrame(fistRollInFrame);
                fistRollInFrame += 2;
            }

        }
        return _score;
    }

    private int NextBallsForFrame(int fistRollInFrame)
    {
        return _rolls[fistRollInFrame] + _rolls[fistRollInFrame + 1];
    }

    private int NextBallsForStrike(int fistRollInFrame)
    {
        return 10 + _rolls[fistRollInFrame + 1] + _rolls[fistRollInFrame + 2];
    }

    private int NextBallForSpare(int fistRollInFrame)
    {
        return _rolls[fistRollInFrame + 2];
    }

    private bool IsStrike(int fistRollInFrame)
    {
        return _rolls[fistRollInFrame] == 10;
    }

    private bool IsSpare(int fistRollInFrame)
    {
        return _rolls[fistRollInFrame] + _rolls[fistRollInFrame + 1] == 10;
    }

    public void Roll(int pins)
    {
        _rolls[currentRoll ++] = pins;
    }

}
