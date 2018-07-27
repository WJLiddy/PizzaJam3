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
        return x ^ y;
    }

    public override string ToString()
    {
        return "IntVec2: " + x + " " + y;
    }
}

public class GameState
{
    public static readonly float TREE_THRESH = 0.7f;

    public int dim_;
    public TileItem[,] tiles_;
    // Players start at north west, baddies in south east.
    public GameState(int dim)
    {
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
        for (int x = 0; x != dim_; ++x)
        {
            for (int y = 0; y != dim_; ++y)
            {
                if (Mathf.PerlinNoise(((float)x) / 10f, ((float)y) / 10f) > TREE_THRESH)
                {
                    Resource.AddWood(this, new IntVec2(x, y));
                }
            }
        }
    }

    public void addOres()
    {
        // one ore per 20 by 20
        for(int i = 0; i != ((dim_*dim_) / 1500); ++i)
        {
            Resource.AddOre(this,new IntVec2(Random.Range(0, dim_), Random.Range(0,dim_)));
        }
    }

    public void addOils()
    {    
        for (int i = 0; i != ((dim_ * dim_) / 3000); ++i)
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
                    tiles_[loc.x, loc.y] = null;
                }
            }
        }
    }

    public void process()
    {
        for (int x = 0; x != dim_; ++x)
        {
            for (int y = 0; y != dim_; ++y)
            {
                if (tiles_[x, y] != null && tiles_[x, y] is HarvesterRobot)
                {
                    (tiles_[x, y] as HarvesterRobot).doRobotAI(new IntVec2(x, y), this);
                }
            }
        }
    }

    public void placeItemNear(TileItem ti, IntVec2 iv)
    {
        if(getItem(iv) == null)
        {
            tiles_[iv.x, iv.y] = ti;
            return;
        }
        while(true)
        {
            int range = 1;
            for(int x = -range; x != range; ++x)
            {
                for(int y = -range; y != range; ++y)
                {
                    IntVec2 niv = new IntVec2(iv.x + x, iv.y + y);
                    if (getItem(niv) == null && !isOOB(niv))
                    {
                        tiles_[iv.x, iv.y] = ti;
                    }
                }
            }
        }
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
                tu.anim = TileUnit.Animation.IDLE;
            }
        }

        if(tu.anim == TileUnit.Animation.HARVEST && tu is HarvesterRobot)
        {
            tiles_[x + tu.animDir.x, y + tu.animDir.y] = null;
            placeItemNear(new ExtractedResource((tu as HarvesterRobot).type, 50), new IntVec2(x + tu.animDir.x, y + tu.animDir.y));
        }
    }

    public void tick()
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
    }

}
