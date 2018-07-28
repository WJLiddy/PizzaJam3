using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1911 : Gun
{
    public float crit_chance;
    public float range;
    public int capacity;

    public override string getName()
    {
        return "M1911";
    }


    public override Gun spawn(float rarity)
    {
        M1911 m = new M1911();
        m.crit_chance = rarity / 10f; // Worst gun never crits, best gun crits 10%
        m.range = UnityEngine.Random.Range(5f, 8f); // 5 to 8 tiles regardless of rarity
        m.capacity = (rarity > 0.5 ? 12 : 8); // above half rarity? extended mag
        return m;
    }


    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List <Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        b.accuracy_modifier_degree = UnityEngine.Random.Range(-10f, 10f);
        b.is_crit = UnityEngine.Random.Range(0f, 1f) < crit_chance;
        b.projectile = Projectile.ProjectileType.Bullet;
        b.range = range;
        b.speed = 5f;
        fp.Add(b);
        return fp;
    }

    public override int getCapacity()
    {
        return capacity;
    }

    public override float getReloadTime()
    {
        return 1f;
    }

    public override float getROF()
    {
        return 0;
    }

    public override bool isFullAuto()
    {
        return false;
    }
}
