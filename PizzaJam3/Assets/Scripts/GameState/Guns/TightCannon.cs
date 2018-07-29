using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TightCannon : Gun
{
    public float crit_chance;
    public int capacity;
    public float range;
    public float speed;
    public float reloadtime;
    

    public override string getName()
    {
        return "Tight Cannon";
    }
    

    // 170 DPS. 4 * 50 = 200 damange, only takes 1 s to reload?
    public override Gun spawn(float rarity)
    {
        TightCannon c = new TightCannon();
        c.crit_chance = rarity;
        c.capacity = 4;
        c.range = 20;
        c.speed = 10;
        c.reloadtime = 1.5f;
        return c;
    }

    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        b.accuracy_modifier_degree = UnityEngine.Random.Range(-3f, 3f);
        b.is_crit = UnityEngine.Random.Range(0f, 1f) < crit_chance;
        b.projectile = Projectile.ProjectileType.CannonBall;
        b.range = range;
        b.speed = speed * (b.is_crit ? 1 : 2);
        fp.Add(b);
        return fp;
    }

    public override int getCapacity()
    {
        return capacity; 
    }

    public override float getReloadTime()
    {
        return reloadtime; 
    }

    //means time between shots
    public override float getROF()
    {
        return 0.9f; // TODO: figure out how this looks in game
    }

    public override bool isFullAuto()
    {
        return false;
    }

    public override bool consumeMultipleAmmoPerFire()
    {
        return false;
    }
}
