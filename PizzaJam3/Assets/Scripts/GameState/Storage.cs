using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : TileUnit, Building
{
    public Resource.Type type;
    public int amount;
    public static int MAX_AMOUNT = 100;
    public string nameof;

    public Storage(Resource.Type t)
    {
        switch(t)
        {
            case Resource.Type.OIL: nameof =  "refinery"; break;
            case Resource.Type.ORE: nameof =  "foundry"; break;
            case Resource.Type.WOOD: nameof = "sawmill"; break;
        }
        amount = 0;
        type = t;
        anim = Animation.IDLE;
    }

    public override int getMaxHP()
    {
        return 100;
    }

    public override bool isFriendly()
    {
        return true;
    }

    public string renderName()
    {
        return nameof;
    }
}
