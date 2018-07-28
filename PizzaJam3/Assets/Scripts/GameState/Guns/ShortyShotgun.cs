using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortyShotgun : Gun
{

    public float jam_rate = 0.3f; // this should be high for this gun I assume this is 30% chance

    public override Gun spawn(float rarity)
    {
        TightCannon c = new TightCannon();
        c.jam_rate = (1f - rarity) / 30; // TODO: copied from m4. need to adjust not sure how this effects things
        return c;
    }

    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        b.accuracy_modifier_degree = UnityEngine.Random.Range(-7f, 7f);
        b.is_crit = false;
        b.projectile = Projectile.ProjectileType.Bullet;
        b.range = 9; // TODO: also not sure what this effects, and speed should also be slower
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
        return 5;
    }

    public override float getReloadTime()
    {
        return 3f; // TODO: could this be fast but is repeated based on (Capacity-RoundsRemaining)
    }

    //means time between shots
    public override float getROF()
    {
        return 0.1f; // TODO: figure out how this looks in game but slower than this
    }

    public override bool isFullAuto()
    {
        return false; // unlike the proper shotgun this one should not be full auto
    }
}
