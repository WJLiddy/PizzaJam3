using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpShotgun : Gun
{
<<<<<<< HEAD
    public float crit_chance;
    public int capacity;
    public float range = 4;
    public float reloadtime;
    public float speed = 4;
    public float spread;
=======
    public override string getName()
    {
        return "Pump Shotty";
    }

>>>>>>> 4437c264d87a89bd6bf7682755dba2acef60d4a8
    public float jam_rate = 0.1f; // this should be high for this gun I assume this is 10% chance
    public bool fullauto;

    public override Gun spawn(float rarity)
    {
        PumpShotgun s = new PumpShotgun();
        s.crit_chance = rarity / 30f; // Worst gun never crits, best gun crits 30%
        s.capacity = 7 + (int)(rarity / 0.1f); //up to 10 extra bullets
        s.jam_rate = (1f - rarity) / 30; // TODO: copied from m4. need to adjust
        s.fullauto = true; //TODO: fix this needs rarity 
        s.reloadtime = ((5 - (rarity / 0.2f)) * capacity); // should be based on how many shells have been fired
        s.spread = 30 - (7 * UnityEngine.Random.Range(0f, rarity));
        return s;
    }

    public override List<FiredProjectile> fireGun()
    {
        //shoots one bullet
        List<Gun.FiredProjectile> fp = new List<FiredProjectile>();
        Gun.FiredProjectile b;
        b.accuracy_modifier_degree = UnityEngine.Random.Range(-7f, 7f);
        b.is_crit = false;
        b.projectile = Projectile.ProjectileType.Fletchette;
        b.range = range; // TODO: also not sure what this effects, and speed should also be slower
        b.speed = speed;
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
        return 0.3f; // TODO: figure out how this looks in game but slower than this
    }

    public override bool isFullAuto()
    {
        return fullauto;
    }
}
