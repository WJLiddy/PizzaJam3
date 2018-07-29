using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortyShotgun : Gun
{
    public float crit_chance;
    public int capacity;
    public float range = 4;
    public float reloadtime;
    public float speed = 4;
    public float spread;
    public bool consumeMultipleAmmoPerFire = false;

    public override string getName()
    {
        return "Shorty Shotty";
    }
    
    public float jam_rate = 0.3f; // this should be high for this gun I assume this is 30% chance

    public override Gun spawn(float rarity)
    {
        ShortyShotgun s = new ShortyShotgun();
        s.crit_chance = rarity / 30f; // Worst gun never crits, best gun crits 30%
        s.capacity = 4 + (int)(rarity / 0.1f); //up to 10 extra bullets
        s.jam_rate = (1f - rarity) / 30; // TODO: copied from m4. need to adjust should be high
        s.reloadtime = 6 - ((rarity / 0.2f) * capacity); // should be based on how many shells have been fired
        s.spread = 40 - (7 * UnityEngine.Random.Range(0f, rarity));
        return s;
    }

    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        b.accuracy_modifier_degree = UnityEngine.Random.Range(-spread, spread);
        b.is_crit = false;
        b.projectile = Projectile.ProjectileType.Fletchette;
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
        return capacity;
    }

    public override float getReloadTime()
    {
        return reloadtime; // TODO: could this be fast but is repeated based on (Capacity-RoundsRemaining)
    }

    //means time between shots
    public override float getROF()
    {
        return 0.9f; // TODO: figure out how this looks in game but slower than this
    }

    public override bool isFullAuto()
    {
        return false; // unlike the proper shotgun this one should not be full auto
    }
}
