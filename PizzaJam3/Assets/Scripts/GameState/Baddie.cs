using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Baddie : TileUnit
{
    // Breaks coupling, but no other way to handle bullets?
    GameRenderer gr;

    // Call these in doAI()
    public void shootProjectile(Projectile.ProjectileType pt, double angle, double velocity)
    {
        //nop.
    }

    public void shootProjectile(IntVec2 this_pos, Projectile.ProjectileType pt, IntVec2 targetTile, float velocity, float range)
    {
        Gun.FiredProjectile fp;
        fp.accuracy_modifier_degree = 0;
        fp.is_crit = false;
        fp.projectile = Projectile.ProjectileType.Bullet;
        fp.range = range;
        fp.speed = velocity;

        gr.AddBullet(fp,Mathf.Atan2(this_pos.y - targetTile.y, this_pos.x - targetTile.x), new Vector2(this_pos.x,this_pos.y));
    }

    public void move(IntVec2 dir)
    {
        anim = Animation.MOVE;
        animDir = dir;
    }


    public abstract void doAI(IntVec2 pos, GameState gs);
    public abstract int getVisibility();
}
