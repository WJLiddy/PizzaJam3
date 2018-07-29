using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSmith : TileUnit, Robot, Building
{
    int oil;

    public GunSmith()
    {
        anim = Animation.IDLE;
    }
    public List<Gun> allGuns()
    {
        List<Gun> l = new List<Gun>();
        l.Add(new Musket());
        l.Add(new Revolver());
        l.Add(new M1911());
        l.Add(new ShortyShotgun());
        l.Add(new MP5());
        l.Add(new M4());
        l.Add(new PumpShotgun());
        l.Add(new TightCannon());
        l.Add(new RPG());
        return l;
    }

    public void doRobotAI(IntVec2 pos, GameState gs, ref IntVec2 animD, ref Animation anim)
    {
        if((gs.time_hr == 0 || gs.time_hr == 12) && gs.time_min == 0)
        {
            //make gun
            float base_rarity = Math.Min(1, 0.3f + (oil / 3000f));
            var guns = allGuns();
            int gunid = UnityEngine.Random.Range(0, Math.Min(gs.day * 2, allGuns().Count));
            gs.placeItemNear(new GroundGun(guns[gunid].spawn(base_rarity)), pos);
            oil = 0;
        }

        if(gs.time_min == 0)
        {
            int[] b = new int[3] { 0,0,(int)(0.05 * gs.player_oil)};
            gs.tryBuy(b);
            oil += b[2];
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
