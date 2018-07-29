using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTower : TileUnit, Robot, Building //This interface is FUCKED
{
    public Gun storedGun;
    public float reloadTime;

    public GuardTower()
    {
        anim = Animation.IDLE;
    }

    public bool findBaddie(TileItem t)
    {
        return (t is TileUnit) && !(t as TileUnit).isFriendly();
    }

    public void doRobotAI(IntVec2 pos, GameState gs)
    {
        if(storedGun == null)
        {
            return;
        }

        if(storedGun.bulletsLeft == 0)
        {
            storedGun.bulletsLeft = storedGun.getCapacity();
            return;
        }

        var v = find(pos, gs, findBaddie);

        if (v != null && storedGun.bulletsLeft > 0)
        {
            var bullets = storedGun.fireGun();
            foreach (var b in bullets)
            {
                if(storedGun.bulletsLeft == 0)
                {
                    return;
                }
                storedGun.bulletsLeft--;
                shootProjectile(pos, b.projectile, v, b.speed, b.range);              
            }
        }
    }

    public override int getMaxHP()
    {
        return 1000;
    }

    public override bool isFriendly()
    {
        return true;
    }

    public string renderName()
    {
        return "guardtower";
    }
}

