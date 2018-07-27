using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Baddie : TileUnit
{


    // Call these in doAI()
    public void shootProjectile(Projectile.ProjectileType pt, double angle, double velocity)
    {

    }
    public void shootProjectile(Projectile.ProjectileType pt, IntVec2 targetTile, double velocity)
    {

    }

    public void move(IntVec2 dir)
    {

    }

    public int distanceTo(IntVec2 v)
    {
        return 1;
    }
    
    
    public abstract int getMaxHp();
    public abstract void doAi();
    public abstract int getVisibility();
}
