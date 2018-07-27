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

}
