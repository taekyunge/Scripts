using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldData 
{
    public ContentType Type;
    public int Gold;
    public int Start_Level;
    public int End_Level;
    public int More_Gold;

    public GoldData(ContentType type, int gold, int startLevel, int endLevel, int moreGold)
    {
        Type = type;
        Gold = gold;
        Start_Level = startLevel;
        End_Level = endLevel;
        More_Gold = moreGold;
    }
}
