using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntVec2
{
    public readonly int x;
    public readonly int y;
    public IntVec2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static IntVec2 operator +(IntVec2 b, IntVec2 c)
    {
        return new IntVec2(b.x + c.x, b.y + c.y);
    }

    public static IntVec2 operator -(IntVec2 b, IntVec2 c)
    {
        return new IntVec2(b.x - c.x, b.y - c.y);
    }

    public override bool Equals(System.Object obj)
    {
        IntVec2 iObj = obj as IntVec2;
        if (iObj == null)
            return false;
        else
            return (iObj.x == x && iObj.y == y);
    }

    public override int GetHashCode()
    {
        int tmp = (y + ((x + 1) / 2));
        return x + (tmp * tmp);
    }

    public override string ToString()
    {
        return "IntVec2: " + x + " " + y;
    }
}

public class GameState
{
    public static readonly float TREE_THRESH = 0.5f;
    public static readonly int minutes_per_tick = 5;
    public int dim_;
    public TileItem[,] tiles_;
    public Player player;

    public int time_hr;
    public int time_min;
    // Players start at north west, baddies in south east.
    public GameState(int dim)
    {
       time_hr = 8;
       dim_ = dim;
       tiles_ = new TileItem[dim,dim];
       addTrees();
       addOres();
       addOils();
    }

    public bool isOOB (IntVec2 i)
    {
        return i.x < 0 || i.y < 0 || i.x >= dim_ || i.y >= dim_;
    }

    public TileItem getItem(IntVec2 v)
    {
        return tiles_[v.x, v.y];
    }

    public void addTrees()
    {
        float xbias = Random.Range(0, 1000f);
        float ybias = Random.Range(0, 1000f);
        for (int x = 0; x != dim_; ++x)
        {
            for (int y = 0; y != dim_; ++y)
            {
                if (Mathf.PerlinNoise(xbias + ((float)x) / 7f, ybias + ((float)y) / 7f) > TREE_THRESH)
                {
                    Resource.AddWood(this, new IntVec2(x, y));
                }
            }
        }
    }

    public void addOres()
    {
        // one ore per 20 by 20
        for(int i = 0; i != ((dim_*dim_) / 500); ++i)
        {
            Resource.AddOre(this,new IntVec2(Random.Range(0, dim_), Random.Range(0,dim_)));
        }
    }

    public void addOils()
    {    
        for (int i = 0; i != ((dim_ * dim_) / 1000); ++i)
        {
            Resource.AddOil(this,new IntVec2(Random.Range(0, dim_), Random.Range(0, dim_)));
        }
    }


    public void addClearing(IntVec2 loc, int radius)
    {
        for(int x = -radius; x != radius; ++x)
        {
            for(int y = -radius; y != radius; ++y)
            {
                IntVec2 to_clear = new IntVec2(loc.x + x, loc.y + y);
                if(!isOOB(to_clear))
                {
                    tiles_[to_clear.x, to_clear.y] = null;
                }
            }
        }
    }

    public bool hurt(int x, int y, int dmg, out bool stop)
    {
        stop = false;
        if (getItem(new IntVec2(x, y)) != null)
        {
            stop = true;
        }

        if (getItem(new IntVec2(x,y)) != null && getItem(new IntVec2(x, y)) is TileUnit)
        {
            bool ded = false;
            (getItem(new IntVec2(x, y)) as TileUnit).hurt(dmg, out ded);
            if(ded)
            {
                tiles_[x, y] = null;
            }
            return true;
        }
        return false;
    }

    public void process()
    {
        for (int x = 0; x != dim_; ++x)
        {
            for (int y = 0; y != dim_; ++y)
            {
                if (tiles_[x, y] != null && tiles_[x, y] is Robot && time_hr > 5 && time_hr < 21)
                {
                    (tiles_[x, y] as Robot).doRobotAI(new IntVec2(x, y), this);
                }

                if (tiles_[x, y] != null && tiles_[x, y] is Baddie)
                {
                    (tiles_[x, y] as Baddie).doAI(new IntVec2(x, y), this);
                }
            }
        }
    }

    public void placeItemNear(TileItem ti, IntVec2 iv)
    {
        var l = player.inTiles();

        if(getItem(iv) == null)
        {
            tiles_[iv.x, iv.y] = ti;
            return;
        }

        int range = 1;
        while (true)
        {
            for(int x = -range; x <= range; ++x)
            {
                for(int y = -range; y <= range; ++y)
                {
                    IntVec2 niv = new IntVec2(iv.x + x, iv.y + y);
                    if (!isOOB(niv) && getItem(niv) == null && !l.Contains(niv))
                    {
                        Debug.Log("PLACE: " +niv);
                        tiles_[iv.x + x, iv.y + y] = ti;
                        return;
                    }
                }
            }
            range++;
        }
    }

    public bool tileWouldBeOccupied(int x, int y)
    {
        if(isOOB(new IntVec2(x, y)) || tiles_[x,y] != null)
        {
            return true;
        }

        for (int dx = -1; dx != 2; ++dx)
        {
            for (int dy = -1; dy != 2; ++dy)
            {
                if (isOOB(new IntVec2(dx+x, dy+y)) || tiles_[dx+x, dy+y] == null)
                {
                    continue;
                }

                TileItem t = (tiles_[x + dx, y + dy]);
                if(t is TileUnit)
                {
                    if ((t as TileUnit).anim == TileUnit.Animation.MOVE && (t as TileUnit).animDir.Equals(new IntVec2(-dx, -dy)))
                        return true;
                }
            }
        }
        return false;
    }

    public void handleUnitTick(int x, int y, TileUnit tu)
    {
        if (tu.anim == TileUnit.Animation.MOVE)
        {
            // Make sure nothing in the way.
            IntVec2 new_pos = new IntVec2(x + tu.animDir.x, y + tu.animDir.y);
            if (!isOOB(new_pos) && getItem(new_pos) == null)
            {
                tiles_[x, y] = null;
                tiles_[new_pos.x, new_pos.y] = tu;
            }
        }

        if(tu.anim == TileUnit.Animation.HARVEST && tu is HarvesterRobot)
        {
            (tu as HarvesterRobot).extract(this, new IntVec2(x + tu.animDir.x, y + tu.animDir.y));
        }

        if (tu.anim == TileUnit.Animation.COLLECT && tu is CollectorRobot)
        {
            (tu as CollectorRobot).collect(this, new IntVec2(x + tu.animDir.x, y + tu.animDir.y));
        }

        if (tu.anim == TileUnit.Animation.STORE && tu is CollectorRobot)
        {
            (tu as CollectorRobot).deposit(this, new IntVec2(x + tu.animDir.x, y + tu.animDir.y));
        }
        tu.anim = TileUnit.Animation.IDLE;
    }

    public void tick(GameRenderer gr)
    {
        for(int x = 0; x != dim_; ++x)
        {
            for(int y = 0; y != dim_; ++y)
            {
                if(getItem(new IntVec2(x,y)) != null)
                {
                    TileItem ti = getItem(new IntVec2(x, y));
                    if(ti is TileUnit)
                    {
                        handleUnitTick(x, y, ti as TileUnit);
                    }
                }
            }
        }

        time_min += minutes_per_tick;

        if(time_min > 59)
        {
            time_min -= 60;
            time_hr++;

            if(time_hr < 4 || time_hr > 22)
            {
                baddieSpawn(gr);
            }
            if(time_hr == 6)
            {
                cleanBaddies();
            }
        }
        if(time_hr > 23)
        {
            time_hr = 0;
        }
    }

    public void resourceCount(out int wood, out int ore, out int oil)
    {
        oil = 0;
        wood = 0;
        ore = 0;
        for (int i = 0; i != dim_; ++i)
        {
            for (int j = 0; j != dim_; ++j)
            {
                if(getItem(new IntVec2(i,j)) != null && getItem(new IntVec2(i, j)) is Storage)
                {
                    switch ((getItem(new IntVec2(i, j)) as Storage).type)
                    {
                        case Resource.Type.OIL: oil += (getItem(new IntVec2(i, j)) as Storage).amount; break;
                        case Resource.Type.WOOD: wood += (getItem(new IntVec2(i, j)) as Storage).amount; break;
                        case Resource.Type.ORE: ore += (getItem(new IntVec2(i, j)) as Storage).amount; break;
                    }

                }

            }
        }
    }

    public bool flighthouse(TileItem t)
    {
        return (t is GuardTower);
    }

    public void cleanBaddies()
    {
        for(int x = 0; x != dim_; ++x)
        {
            for(int y = 0; y != dim_; ++y)
            {
                if(tiles_[x,y] is Baddie)
                {
                    tiles_[x, y] = null;
                }
            }
        }
    }
    //Spawn 70 meters away from lighthouses.
    public void baddieSpawn(GameRenderer gr)
    {
        for(int i = 0; i != (dim_ * dim_) / 500; ++i)
        {
            int x = (int)Random.Range(0, dim_);
            int y = (int)Random.Range(0, dim_);

            if(tiles_[x,y] == null)
            {
                Baddie b = new Swarmer();
                b.anim = TileUnit.Animation.IDLE;
                b.gr = gr;
                var v = b.find(new IntVec2(x, y), this, flighthouse);
                if(v == null)
                {
                    tiles_[x, y] = b;
                    continue;
                }

                if(Vector2.Distance(new Vector2(x,y),new Vector2(v.x,v.y)) > 70)
                {
                    tiles_[x, y] = b;
                }
            }
        }
    }
}
