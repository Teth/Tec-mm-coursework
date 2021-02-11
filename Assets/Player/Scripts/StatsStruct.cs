using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class StatsStruct
{
    public StatsStruct(int ins=5, int prow=5, int str=5)
    {
        Insight = ins;
        Prowess = prow;
        Strength = str;
    }

    public static StatsStruct getStatsFromClass(PlayerClass.PlayerClassEnum playerclass)
    {
        switch (playerclass)
        {
            case PlayerClass.PlayerClassEnum.Rogue:
                return new StatsStruct(3, 5, 8);
            case PlayerClass.PlayerClassEnum.Mage:
                return new StatsStruct(10, 3, 2);
            case PlayerClass.PlayerClassEnum.Warrior:
                return new StatsStruct(5, 8, 2);
            default:
                return new StatsStruct(5, 5, 5);
        }
    }

    public int Insight;
    public int Prowess;
    public int Strength;
}

