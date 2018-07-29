using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile
{
    public enum ProjectileType
    {
                    //  DAMAGE   |   SIZE   |   AIR RES.
        MusketBall, //   Med         Med        Med
        CannonBall, //   High        Big        High
        Fletchette, //   Low         Small      High
        Bullet,     //   Low         Small      Low
        Rocket,     //   High        Big        High
    }
}
