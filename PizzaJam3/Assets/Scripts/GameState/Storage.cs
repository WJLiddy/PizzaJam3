using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : TileUnit
{
    public Resource.Type type;
    public int amount;
    public static int MAX_AMOUNT = 100;

    public Storage(Resource.Type t)
    {
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
}
