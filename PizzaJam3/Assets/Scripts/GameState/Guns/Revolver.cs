using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Gun
{

    public float crit_chance;
    public int capacity;
    public float range;
    public float reloadtime;
    public float speed = 6;
    public bool consumeMultipleAmmoPerFire = false;

    public override string getName()
    {
        return "Revolver";
    }


    public override Gun spawn(float rarity)
    {
        Revolver r = new Revolver();
        r.capacity = 5 + (int)(rarity / 0.2f); //up to 5 extra bullets
        r.range = 7 + (int)UnityEngine.Random.Range(0f, 4 * rarity); // Chance of extended range if high crit chance.
        r.reloadtime = 6 - (int)(rarity / 0.2f); //possible 1 sec reload over 6 for base
        r.crit_chance = rarity / 20f; // Worst gun never crits, best gun crits 20%
        return r;
    }

    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        b.accuracy_modifier_degree = UnityEngine.Random.Range(-7f, 7f);
        b.is_crit = UnityEngine.Random.Range(0f, 1f) < crit_chance;
        b.projectile = Projectile.ProjectileType.Bullet;
        b.range = range; 
        b.speed = 4f;
   
        
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
        return 0.1f; // TODO: figure out how this looks in game
    }

    public override bool isFullAuto()
    {
        return false;
    }
}
