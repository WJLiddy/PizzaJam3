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
    public int player_wood;
    public int player_ore;
    public int player_oil;
    public int day = 1;
    public static readonly float TREE_THRESH = 0.5f;
    public static readonly int minutes_per_tick = 4;
    public int dim_;
    public TileItem[,] tiles_;
    public Player player;

    public int time_hr;
    public int time_min;


    // BALANCE CONSTANTS WOOO


    // Players start at north west, baddies in south east.
    public GameState(int dim)
    {
       time_hr = 8;
       dim_ = dim;
       tiles_ = new TileItem[dim,dim];
       addTrees();
       addOres();
       addOils();
       addGuns();
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
        for(int i = 0; i != ((dim_*dim_) / 300); ++i)
        {
            Resource.AddOre(this,new IntVec2(Random.Range(0, dim_), Random.Range(0,dim_)));
        }
    }

    public void addOils()
    {    
        for (int i = 0; i != ((dim_ * dim_) / 700); ++i)
        {
            Resource.AddOil(this,new IntVec2(Random.Range(0, dim_), Random.Range(0, dim_)));
        }
    }

    public void addGuns()
    {
        for (int i = 0; i != ((dim_ * dim_) / 2000); ++i)
        {
            Gun g = null;
            switch(Random.Range(0,4))
            {
                case 0: g = new Musket().spawn(0); break;
                case 1: g = new Revolver().spawn(0); break;
                case 2: g = new M1911().spawn(0);  break;
                case 3: g = new ShortyShotgun().spawn(0); break;
            }

            tiles_[Random.Range(0, dim_), Random.Range(0, dim_)] = new GroundGun(g);
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
        if(isOOB(new IntVec2(x,y)))
        {
            stop = true;
            return false;
        }

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

    public bool processOne(ref int x_start, ref int y_start, out GameRenderer.animSet a)
    {
        a.anim = null;
        a.start = null;
        a.animType = TileUnit.Animation.IDLE;
        for (; x_start < dim_; ++x_start)
        {
            for (; y_start < dim_; ++y_start)
            {

                if (tiles_[x_start, y_start] != null && tiles_[x_start, y_start] is Robot 
                    && (tiles_[x_start, y_start] is GuardTower || tiles_[x_start, y_start] is GunSmith || (time_hr > 5 && time_hr < 21)))
                {
                    a.start = new IntVec2(x_start, y_start);
                    (tiles_[x_start, y_start] as Robot).doRobotAI(new IntVec2(x_start, y_start), this, ref a.anim, ref a.animType);
                    y_start++;
                    return true;
                }

                if (tiles_[x_start, y_start] != null && tiles_[x_start, y_start] is Baddie)
                {
                    a.start = new IntVec2(x_start, y_start);
                    (tiles_[x_start, y_start] as Baddie).doAI(new IntVec2(x_start, y_start), this, ref a.anim, ref a.animType);
                    y_start++;
                    return true;
                }
            }
            if(y_start >= dim_)
            {
                y_start = 0;
            }
        }
        return false;
    }

    public IntVec2 placeItemNear(TileItem ti, IntVec2 iv)
    {
        var l = player.inTiles();

        if(getItem(iv) == null && !l.Contains(iv))
        {
            tiles_[iv.x, iv.y] = ti;
            return new IntVec2(iv.x,iv.y);
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
                        tiles_[iv.x + x, iv.y + y] = ti;
                        return new IntVec2(iv.x + x, iv.y + y);
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
            if (!isOOB(new_pos) && getItem(new_pos) == null && !player.inTiles().Contains(new_pos))
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


    public string priceRender(int[] res)
    {
        return "Wood: " + res[0] + "\nOre: " + res[1] + "\nOil: " + res[2];
    }

    public string priceRenderNoNewline(int[] res)
    {
        return "Wood: " + res[0] + " Ore: " + res[1] + " Oil: " + res[2];
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
        oil = player_oil;
        wood = player_wood;
        ore = player_ore;
    }

    public bool tryBuy(int[] i)
    {
        if(player_wood >= i[0] && player_ore >= i[1] && player_oil >= i[2])
        {

            player_wood -= i[0];
            player_ore -= i[1];
            player_oil -= i[2];
            return true;
        }
        return false;
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
        day++;
    }

    public void baddieSpawn(GameRenderer gr)
    {
            int to_spawn = day;
            float ang = 0;
            int range = 10;
            int x = 0;
            int y = 0;

            while (range < 200)
            {
                x = (int)(player.location.x + (range * Mathf.Sin(ang * Mathf.Deg2Rad)));
                y = (int)(player.location.y + (range * Mathf.Cos(ang * Mathf.Deg2Rad)));

                if (!isOOB(new IntVec2(x,y)) && tiles_[x, y] == null)
                {
                    Baddie b = new Swarmer(BALANCE_CONSTANTS.BADDIE_HP + (day * BALANCE_CONSTANTS.BADDIE_HP_GROWTH_DAY));
                    b.anim = TileUnit.Animation.IDLE;
                    b.gr = gr;
                    var v = b.find(new IntVec2(x, y), this, flighthouse);
                    if(v == null)
                    {
                        tiles_[x, y] = b;
                        //OK!
                        to_spawn--;
                    }
                    else if (Vector2.Distance(new Vector2(x, y), new Vector2(v.x, v.y)) > 10)
                    {
                        tiles_[x, y] = b;
                        //OK!
                        to_spawn--;
                     }
                }

                if(to_spawn == 0)
                {
                    return;
                }

                ang += 31;
                if(ang > 360)
                {
                    ang -= 360;
                    range += 5;
                }
            }
    }
}
