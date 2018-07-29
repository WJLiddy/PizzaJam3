using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpShotgun : Gun
{
    public float crit_chance;
    public int capacity;
    public float range = 6;
    public float reloadtime;
    public float speed = 5;
    public float spread;

    public override string getName()
    {
        return "Pump Shotty";
    }
    
    public int burst_size;

    //112 DPS
    public override Gun spawn(float rarity)
    {
        PumpShotgun s = new PumpShotgun();
        s.crit_chance = rarity * 0.3f; // Worst gun never crits, best gun crits 30%
        s.capacity = 7 + (int)(rarity * 4); //up to 10 extra bullets
        s.reloadtime = (s.capacity/2);
        s.spread = 15 - (7 * UnityEngine.Random.Range(0f, rarity));
        s.burst_size = 10 + (int)(rarity * 5);
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
            b.range = Random.Range(7,12); // TODO: also not sure what this effects, and speed should also be slower
            b.speed = speed;
            fp.Add(b);
        }
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
        return 1f;
    }

    public override bool isFullAuto()
    {
        return true;
    }

    public override bool consumeMultipleAmmoPerFire()
    {
        return false;
    }

}
