using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Gun
{

    public float crit_chance;
    public int capacity;
    public float range;
    public float reloadtime;


    public override string getName()
    {
        return "Revolver";
    }

    // 5 shots * 10 dmg / 3.5 s reload ~ 15 DPS
    public override Gun spawn(float rarity)
    {
        Revolver r = new Revolver();
        r.capacity = 5 + (int)UnityEngine.Random.Range(0f, (rarity * 5)); //up to 4 extra bullets
        r.range = 10 + (int)UnityEngine.Random.Range(0f, rarity * 5); // Chance of extended range if high crit chance.
        r.reloadtime = (int)(r.capacity * 0.7f);
        r.crit_chance = rarity / 20f; // Worst gun never crits, best gun crits 5%
        return r;
    }

    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        b.accuracy_modifier_degree = UnityEngine.Random.Range(-9f, 9f);
        b.is_crit = UnityEngine.Random.Range(0f, 1f) < crit_chance;
        b.projectile = Projectile.ProjectileType.Bullet;
        b.range = range; 
        b.speed = 7f;
   
        
        fp.Add(b);
        return fp;
    }

    public override int getCapacity()
    {
        return capacity; 
    }

    public override float getReloadTime()
    {
        return reloadtime; // TODO: should be  average
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
