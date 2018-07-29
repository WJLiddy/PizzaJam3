using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarmer : Baddie
{
    public bool validTarget(TileItem ti)
    {
        return (ti is TileUnit) && (ti as TileUnit).isFriendly();
    }

    public override void doAI(IntVec2 pos, GameState gs)
    {

        anim = Animation.IDLE;
        IntVec2 iv = find(pos, gs, validTarget);
        if(iv == null)
        {
            return;
        }
        Debug.Assert(pos != null);
        shootProjectile(pos, Projectile.ProjectileType.Bullet, iv, 1, 3);
        if(iv != null)
        {
            var r = getPath(pos, iv, gs);
            if (r != null)
            {
                // 1 means we didnt go anywhere.
                Debug.Assert(r.Count > 1);
                animDir = r[1] - pos;
                anim = Animation.MOVE;
            }
        }
    }

    public override int getMaxHP()
    {
        return 100;
    }


    public override bool isFriendly()
    {
        return false;
    }
}
