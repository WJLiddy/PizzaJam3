using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : Gun
{
    public int capacity;
    public float reloadtime;


    public override string getName()
    {
        return "RPG";
    }


    // 250 DPS. Rockets deal 100 + splash damage.
    public override Gun spawn(float rarity)
    {
        RPG r = new RPG();
        r.capacity = 1;
        r.reloadtime = 1;
        return r;
    }


    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        b.accuracy_modifier_degree = UnityEngine.Random.Range(-2f, 2f);
        b.is_crit = false;
        b.projectile = Projectile.ProjectileType.Rocket;
        b.range = 15;
        b.speed = 8f;
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
        return 2f;
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
