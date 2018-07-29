using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TightCannon : Gun
{
<<<<<<< HEAD
    public float crit_chance;
    public int capacity;
    public float range;
    public float speed;
    public float reloadtime;
=======
    public override string getName()
    {
        return "Tight Cannon";
    }

>>>>>>> 4437c264d87a89bd6bf7682755dba2acef60d4a8


    public override Gun spawn(float rarity)
    {
        TightCannon c = new TightCannon();
        c.crit_chance = rarity / 30f; // Worst gun never crits, best gun crits 30%
        c.capacity = 1 + (int)(rarity / 0.5f); //up to 2 extra bullets
        c.range = 2 + (int)UnityEngine.Random.Range(0f, 3 * rarity); // Chance of extended range if high crit chance.
        c.speed = 2 + (int)(rarity / 0.5f);
        c.reloadtime = 10 - (int)(rarity / 0.2f);
        return c;
    }

    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        b.accuracy_modifier_degree = UnityEngine.Random.Range(-7f, 7f);
        b.is_crit = UnityEngine.Random.Range(0f, 1f) < crit_chance;
        b.projectile = Projectile.ProjectileType.CannonBall;
        b.range = range;
        b.speed = speed;
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
        return 0.1f; // TODO: figure out how this looks in game
    }

    public override bool isFullAuto()
    {
        return false;
    }
}
