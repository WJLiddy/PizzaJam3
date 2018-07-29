using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarmer : Baddie
{
    int max_hp;

    public Swarmer(int max_hp)
    {
        this.max_hp = max_hp;
        HP = max_hp;
    }
    public bool validTarget(TileItem ti)
    {
        return (ti is TileUnit) && (ti as TileUnit).isFriendly();
    }

    public override void doAI(IntVec2 pos, GameState gs, ref IntVec2 animD, ref TileUnit.Animation animT)
    {

        animD = new IntVec2(0, 0);
        animT = Animation.IDLE;
        IntVec2 iv = find(pos, gs, validTarget);
        if(iv == null)
        {
            return;
        }
        Debug.Assert(pos != null);
        if (Vector2.Distance(new Vector2(iv.x, iv.y), new Vector2(pos.x, pos.y)) < 7f)
        {
            int accmod = UnityEngine.Random.Range(-5, 5);
            shootProjectile(pos, Projectile.ProjectileType.Bullet, iv, 3, 4, accmod, false);
        }
        if(iv != null)
        {
            var r = getPath(pos, iv, gs);
            if (r != null)
            {
                // 1 means we didnt go anywhere.
                Debug.Assert(r.Count > 1);
                animD = r[1] - pos;
                animT = Animation.MOVE;
            }
        }
    }

    public override int getMaxHP()
    {
        return max_hp;
    }

    public override bool isFriendly()
    {
        return false;
    }
}
