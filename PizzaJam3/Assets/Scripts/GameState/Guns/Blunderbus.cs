using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blunderbus : Gun {

    public float range = 6;
    public float speed = 6;
    public float spread;
    public int burst_size;


    public override string getName()
    {
        return "Blunderbus";
    }


    public override Gun spawn(float rarity)
    {
        Blunderbus s = new Blunderbus();
        s.spread = 40 - (7 * UnityEngine.Random.Range(0f, rarity));
        s.burst_size = 3 + (int)(rarity / 0.5f);
        return s;
    }


    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        for (int i = 0; i != burst_size; ++i)
        {

            b.accuracy_modifier_degree = UnityEngine.Random.Range(-spread, spread);
            b.is_crit = false;
            b.projectile = Projectile.ProjectileType.Fletchette;
            b.range = range; 
            b.speed = speed;
            fp.Add(b);
        }
        return fp;
    }

    public override int getCapacity()
    {
        return 1;
    }

    public override float getReloadTime()
    {
        return 5f;
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
