using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortyShotgun : Gun
{
    public int capacity;
    public float range = 6;
    public float reloadtime;
    public float speed = 6;
    public float spread;
    
    public int burst_size;
    public override string getName()
    {
        return "Shorty Shotty";
    }
    
    public float shell_miss_rate = 0.2f; //Shell doesn't fire.

    // Looking for 33 DPS. this gun holds 4 rounds 30 damage a piece = 120 / 3s =  40. Also, it jams.
    public override Gun spawn(float rarity)
    {
        ShortyShotgun s = new ShortyShotgun();
        s.capacity = 4 + (int)(rarity * 5); //up to 10 extra bullets
        s.reloadtime = s.capacity;
        s.spread = 25 - ( UnityEngine.Random.Range(0f, 30 * rarity));
        s.burst_size = 4 + (int)(rarity * 2);
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
            b.projectile = Projectile.ProjectileType.Bullet;
            b.range = Random.Range(5,15);
            b.speed = speed;
            // missing shell
            if (Random.Range(0f, 1f) > shell_miss_rate)
            {
                fp.Add(b);
            }
            
        }
        return fp;
    }

    public override int getCapacity()
    {
        return capacity;
    }

    public override float getReloadTime()
    {
        return reloadtime; // TODO: could this be fast but is repeated based on (Capacity-RoundsRemaining)
    }

    //means time between shots
    public override float getROF()
    {
        return 2f; // TODO: figure out how this looks in game but slower than this
    }

    public override bool isFullAuto()
    {
        return false; // unlike the proper shotgun this one should not be full auto
    }

    public override bool consumeMultipleAmmoPerFire()
    {
        return false;
    }
}
