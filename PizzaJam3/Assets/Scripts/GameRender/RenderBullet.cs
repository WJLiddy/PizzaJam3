using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderBullet : MonoBehaviour
{
    public Projectile.ProjectileType type;
    public bool is_enemy_bullet;
    public GameState gs;
    public GameRenderer gr;
    public bool is_crit;
    public float range;
    public Vector2 start;
    bool startSet = false;
    public bool friendlyBullet;

    public void Update()
    {
        if(!startSet)
        {
            startSet = true;
            start = transform.localPosition;
        }
        if (Vector2.Distance(start, transform.localPosition) > range)
        {
            Destroy(this.gameObject);
        }
    }
    // See what the other object is. If it's a 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains(","))
        {
            int x = Convert.ToInt32(other.name.Split(',')[0]);
            int y = Convert.ToInt32(other.name.Split(',')[1]);
            bool stop;
            if (gs.getItem(new IntVec2(x, y)) != null && (gs.getItem(new IntVec2(x, y)) is TileUnit))
            {
                if((gs.getItem(new IntVec2(x, y)) as TileUnit).isFriendly() == friendlyBullet)
                {
                    return;
                }
            }
            int dmg = 0;
            if (type == Projectile.ProjectileType.Bullet)
            {
                dmg = UnityEngine.Random.Range(7, 11);
            }

            if(type == Projectile.ProjectileType.Musket)
            {
                dmg = UnityEngine.Random.Range(20, 30);
            }

            if (type == Projectile.ProjectileType.CannonBall)
            {
                dmg = UnityEngine.Random.Range(40, 75);
            }

            if (type == Projectile.ProjectileType.Rocket)
            {
                dmg = UnityEngine.Random.Range(100, 200);

            }

            if(is_crit)
            {
                dmg *= 3;
            }
            bool damaged = gs.hurt(x,y,dmg, out stop);
            if(damaged)
            {
                gr.addDFloat(transform.localPosition, dmg + (is_crit ? "!" : ""));
            }
            if(stop)
            {
                Destroy(this.gameObject);
                for(int i = -1; i != 2; i++)
                {
                    for(int j = -1; j != 2; ++j)
                    {
                        gs.hurt(x + i, y + i , dmg / 2, out stop);
                    }
                }
            }
        }
            // get name. it will be a coordinate (x,y) or the player which we can look up and handle accordingly
    }
}
