using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All M4s are the same, rare ones jam less
public class M4: Gun
{
    public float jam_rate = 0;

    public override Gun spawn(float rarity)
    {
        M4 m = new M4();
        m.jam_rate = (1f - rarity) / 30; 
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
        b.speed = 3f;
        // jams, wont fire.
        if (Random.Range(0f, 1f) > jam_rate)
        {
            fp.Add(b);
        }
        return fp;
    }

    public override int getCapacity()
    {
        return 30;
    }

    public override float getReloadTime()
    {
        return 3f;
    }

    //means time between shots
    public override float getROF()
    {
        return 0.1f;
    }

    public override bool isFullAuto()
    {
        return true;
    }
}
