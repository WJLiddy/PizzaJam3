using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musket : Gun {

    public float reloadtime;

    public override string getName()
    {
        return "Musket";
    }


    public override Gun spawn(float rarity)
    {
        Musket m = new Musket();
        m.reloadtime = 6 - (int)(rarity / 0.2f); //possible 1 sec reload over 6 for base
        return m;
    }


    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        b.accuracy_modifier_degree = UnityEngine.Random.Range(-7f, 7f);
        b.is_crit = false;
        b.projectile = Projectile.ProjectileType.Bullet;
        b.range = 9;
        b.speed = 5f;      
        fp.Add(b);
        return fp;
    }

    public override int getCapacity()
    {
        return 1;
    }

    public override float getReloadTime()
    {
        return reloadtime;
    }

    //means time between shots
    public override float getROF()
    {
        return 1f;
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
