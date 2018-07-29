using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSmith : TileUnit, Robot, Building
{
    int wood;
    int oil;
    int ore;

    public void doRobotAI(IntVec2 pos, GameState gs, ref IntVec2 animD, ref Animation anim)
    {
        if(( gs.time_hr ==0 || gs.time_hr == 12) && gs.time_min == 0)
        {
            //make gun
        }
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
        return "gunsmith";
    }
}
