using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musket : Gun {

    public float reloadtime;

    public override string getName()
    {
        return "Musket";
    }


    // Damage = 60, reload time = 7, ~~ 10 DPS
    public override Gun spawn(float rarity)
    {
        Musket m = new Musket();
        m.reloadtime = 7 - (rarity * 3);
        return m;
    }


    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        b.accuracy_modifier_degree = UnityEngine.Random.Range(-10f, 10f);
        b.is_crit = Random.Range(0, 5) > 0.5;
        b.projectile = Projectile.ProjectileType.Musket;
        b.range = 9;
        b.speed = 6f; 
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
