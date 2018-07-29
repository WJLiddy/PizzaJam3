using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderBullet : MonoBehaviour
{
    public bool is_enemy_bullet;
    public GameState gs;
    public GameRenderer gr;
    public bool is_crit;
    public float range;
    public Vector2 start;
    int dmg = 10;
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

            bool damaged = gs.hurt(x,y,dmg, out stop);
            if(damaged)
            {
                gr.addDFloat(transform.localPosition, dmg);
            }
            if(stop)
            {
                Destroy(this.gameObject);
            }
        }
            // get name. it will be a coordinate (x,y) or the player which we can look up and handle accordingly
    }
}
