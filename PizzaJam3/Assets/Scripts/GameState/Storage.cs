using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : TileUnit, Building
{
    public Resource.Type type;
    public string nameof;

    public Storage(Resource.Type t)
    {
        switch(t)
        {
            case Resource.Type.OIL: nameof =  "refinery"; break;
            case Resource.Type.ORE: nameof =  "foundry"; break;
            case Resource.Type.WOOD: nameof = "sawmill"; break;
        }
        type = t;
        anim = Animation.IDLE;
    }

    public override int getMaxHP()
    {
        return BALANCE_CONSTANTS.WAREHOUSE_HP;
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
