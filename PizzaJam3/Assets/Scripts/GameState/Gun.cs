using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun
{
    public struct FiredProjectile
    {
        public Projectile.ProjectileType projectile;
        // in tiles/s
        public float speed;
        // in tiles.
        public float range;
        // where -90 is sideways, and 0 is perfect dead on, and 90 is sideways (the other way)
        public float accuracy_modifier_degree;
        // is a crit. 
       public  bool is_crit;
    }
    public abstract string getName();
    public int bulletsLeft = 0;
    public abstract Gun spawn(float rarity);
    public abstract int getCapacity();
    public abstract List<FiredProjectile> fireGun();
    public abstract float getReloadTime(); // in seconds.
    public abstract bool isFullAuto();
    public abstract float getROF();
    public abstract bool consumeMultipleAmmoPerFire();
}
