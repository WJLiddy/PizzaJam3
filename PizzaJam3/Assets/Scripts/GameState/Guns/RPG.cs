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


    public override Gun spawn(float rarity)
    {
        RPG r = new RPG();
        r.capacity = 1 + (int)(rarity / 0.2f); //up to 5 extra
        r.reloadtime = 6 - ((rarity / 0.2f) * capacity);
        return r;
    }


    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        b.accuracy_modifier_degree = UnityEngine.Random.Range(-7f, 7f);
        b.is_crit = false;
        b.projectile = Projectile.ProjectileType.Bullet; // needs replaced by rocket
        b.range = 10;
        b.speed = 10f;
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
        return 2f; // TODO: figure out why this is faster than the shotty, yet I copied it from there
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
