using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileUnit : TileItem
{
    public abstract bool isFriendly();
    public enum Animation
    {
        MOVE,
        HARVEST,
        IDLE,
        COLLECT,
        STORE
    }

    //needed to shoot bullet, fuck.
    public GameRenderer gr;
    public Animation anim;
    public IntVec2 animDir;
    protected int HP;
    public abstract int getMaxHP();

    public TileUnit()
    {
        HP = getMaxHP();
    }

    public virtual void hurt(int dmg, out bool dead)
    {
        HP -= dmg;
        if(HP <= 0)
        {
            dead = true;
        } else
        {
            dead = false;
        }
    }


    static int MAX_RANGE = 2000;

    public static List<IntVec2> getPath(IntVec2 start, IntVec2 end, GameState gs)
    {
        Dictionary<IntVec2, int> dist = new Dictionary<IntVec2, int>();
        Dictionary<IntVec2, IntVec2> prev = new Dictionary<IntVec2, IntVec2>();

        dist[start] = 0;

        List<IntVec2> toSearch = new List<IntVec2>();
        toSearch.Add(start);

        int counter = 0;
        while (toSearch.Count > 0)
        {
            counter++;
            if(counter > MAX_RANGE)
            {
                return null;
            }
            IntVec2 minnode = null;
            foreach (IntVec2 iv2 in toSearch)
            {
                if (minnode == null || dist[iv2] < dist[minnode])
                {
                    minnode = iv2;
                }
            }

            if (minnode.Equals(end))
            {
                List<IntVec2> r = new List<IntVec2>();
                IntVec2 prevn = minnode;
                while (true)
                {
                    r.Add(prevn);
                    if (prev.ContainsKey(prevn))
                    {
                        prevn = prev[prevn];
                    }
                    else
                    {
                        r.Reverse();
                        return r;
                    }
                }
            }

            toSearch.Remove(minnode);

            //Something in the way.
            if (gs.getItem(minnode) != null && minnode != start)
            {
                continue;
            }

            for (int x = -1; x != 2; ++x)
            {
                for (int y = -1; y != 2; ++y)
                {
                    IntVec2 next_node = new IntVec2(minnode.x + x, minnode.y + y);
                    if (gs.isOOB(next_node))
                    {
                        continue;
                    }
                    int new_dist = dist[minnode] + 1;
                    if (!dist.ContainsKey(next_node))
                    {
                        dist[next_node] = new_dist;
                        prev[next_node] = minnode;
                        toSearch.Add(next_node);
                    }
                }
            }
        }
        return null;
    }



    public IntVec2 find(IntVec2 pos, GameState gs, Func<TileItem,bool> f)
    {
        Queue<IntVec2> searchNodes = new Queue<IntVec2>();
        HashSet<IntVec2> alreadyQueued = new HashSet<IntVec2>();
        searchNodes.Enqueue(pos);
        alreadyQueued.Add(pos);
        int searched = 0;
        while (searchNodes.Count > 0)
        {
            searched++;
            if (searched > MAX_RANGE)
            {
                return null;
            }

            IntVec2 toSearch = searchNodes.Dequeue();
            if (gs.getItem(toSearch) != null)
            {
                if (f(gs.getItem(toSearch)))
                {
                    return toSearch;
                }
            }

            //cant go this way
            if (gs.getItem(toSearch) != null && toSearch != pos)
            {
                continue;
            }

            for (int x = -1; x != 2; ++x)
            {
                for (int y = -1; y != 2; ++y)
                {
                    IntVec2 nloc = new IntVec2(toSearch.x + x, toSearch.y + y);
                    if (!alreadyQueued.Contains(nloc) && !gs.isOOB(nloc))
                    {
                        searchNodes.Enqueue(nloc);
                        alreadyQueued.Add(nloc);                 
                    }
                }
            }
        }
        return null;
    }

    public void shootProjectile(IntVec2 this_pos, Projectile.ProjectileType pt, IntVec2 targetTile, float velocity, float range, bool allied)
    {
        Gun.FiredProjectile fp;
        fp.accuracy_modifier_degree = 0;
        fp.is_crit = false;
        fp.projectile = Projectile.ProjectileType.Bullet;
        fp.range = range;
        fp.speed = velocity;

        gr.AddBullet(fp, Mathf.Rad2Deg *  Mathf.Atan2(targetTile.y - this_pos.y, targetTile.x - this_pos.x ), new Vector2(this_pos.x, this_pos.y),allied);
    }


}
