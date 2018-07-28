using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterRobot : TileUnit, Robot
{
    public Resource.Type type;
    public int amountExtracted;
    public int capacity = 50;

    public HarvesterRobot(Resource.Type rt)
    {
        anim = Animation.IDLE;
    }

    public IntVec2 findResource(IntVec2 pos, GameState gs)
    {
        Queue<IntVec2> searchNodes = new Queue<IntVec2>();
        HashSet<IntVec2> alreadyQueued = new HashSet<IntVec2>();
        searchNodes.Enqueue(pos);
        alreadyQueued.Add(pos);
        while (searchNodes.Count > 0)
        {
            IntVec2 toSearch = searchNodes.Dequeue();
            if (gs.getItem(toSearch) != null && gs.getItem(toSearch) is Resource)
            {
                if ((gs.getItem(toSearch) as Resource).type == type)
                {
                    return toSearch;
                }
            }

            //cant go this way
            if(gs.getItem(toSearch) != null && toSearch != pos)
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

    public void extract(GameState gs, IntVec2 pos)
    {

        Resource r = (gs.getItem(pos) != null && gs.getItem(pos) is Resource) ? (gs.getItem(pos) as Resource) : null;
        if(r == null || r.type != type)
        {
            return;
        }

        int to_extract = Math.Min(r.amount, 10);
        amountExtracted += to_extract;
        r.amount -= to_extract;
        if(r.amount == 0)
        {
            gs.tiles_[pos.x, pos.y] = null;
        }

        if(amountExtracted >= capacity)
        {
            gs.placeItemNear(new ExtractedResource(type, amountExtracted), pos);
            amountExtracted = 0;
        }
    
    }

    // Returns current animation for this gamestate (and the next can be implied from there).
    public void doRobotAI(IntVec2 pos, GameState gs)
    {
        IntVec2 r = findResource(pos, gs);
        if (r == null)
        {
            anim = Animation.IDLE;
        }

        if(r != null)
        {
            var list = getPath(pos, r, gs);
            if(list != null)
            {
                // 1 means we didnt go anywhere.
                Debug.Assert(list.Count > 1);
                animDir = list[1] - pos;
                if(list.Count == 2)
                {
                    anim = Animation.HARVEST;
                } else
                {
                    anim = Animation.MOVE;
                }
            }
        }
    }
    public override int getMaxHP()
    {
        return 10;
    }
    public override bool isFriendly()
    {
        return true;
    }
}
