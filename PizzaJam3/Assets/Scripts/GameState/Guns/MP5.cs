using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3 round burst
public class MP5 : Gun
{
    public float crit_chance;
    public int capacity;
    public int burst_size;
    public float spread;
    public float range;
    

    public override string getName()
    {
        return "MP5";
    }

    //an even 50 fps. 250 damage / 5s ~ 50dps
    public override Gun spawn(float rarity)
    {
        MP5 m = new MP5();
        m.range = 7 + (int)UnityEngine.Random.Range(0f, 7*rarity); // Chance of extended range.
        m.capacity = 25 + (int)(rarity * 15); //up to 20 extra bullets
        m.crit_chance = 1f / capacity;
        m.burst_size = 3 + (int)UnityEngine.Random.Range(0f, 3f * rarity); //may shoot 4 or 5 bullets if rare
        // all mp5s have bad spread, rare ones mabye less.
        m.spread = 15 - (7 * UnityEngine.Random.Range(0f, rarity));
        return m;
    }

    // extra bullets will not be fired if not enough in mag
    public override List<FiredProjectile> fireGun()
    {
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        bool is_crits = UnityEngine.Random.Range(0f, 1f) < crit_chance;
        for (int i = 0; i != burst_size; ++i)
        {
            Gun.FiredProjectile b;
            b.accuracy_modifier_degree = UnityEngine.Random.Range(-spread, spread);
            b.is_crit = is_crits;
            b.projectile = Projectile.ProjectileType.Bullet;
            b.range = range;
            b.speed = 5f + Random.Range(0f,2f);
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
        return 2f;
    }

    public override float getROF()
    {
        return 0;
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
