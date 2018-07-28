using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorRobot : TileUnit, Robot
{
    public Resource.Type type;
    public int amount;
    public static int MAX_AMOUNT = 200;

    public CollectorRobot(Resource.Type rt)
    {
        anim = Animation.IDLE;
    }

    public bool collectable(TileItem t)
    {
        return (t is ExtractedResource) && (t as ExtractedResource).type == type;
    }

    public bool dropoffable(TileItem t)
    {
        return (t is Storage) && (t as Storage).type == type;
    }

    // Returns current animation for this gamestate (and the next can be implied from there).
    public void doRobotAI(IntVec2 pos, GameState gs)
    {
        if (amount < MAX_AMOUNT)
        {
            IntVec2 r = find(pos, gs, collectable);
            if (r == null)
            {
                anim = Animation.IDLE;
            }

            if (r != null)
            {
                var list = getPath(pos, r, gs);
                if (list != null)
                {
                    // 1 means we didnt go anywhere.
                    Debug.Assert(list.Count > 1);
                    animDir = list[1] - pos;
                    if (list.Count == 2)
                    {
                        anim = Animation.COLLECT;
                    }
                    else
                    {
                        anim = Animation.MOVE;
                    }
                }
            }
        }
        //drop off.
        else
        {
            IntVec2 r = find(pos, gs, dropoffable);
            if (r == null)
            {
                anim = Animation.IDLE;
            }

            if (r != null)
            {
                var list = getPath(pos, r, gs);
                if (list != null)
                {
                    // 1 means we didnt go anywhere.
                    Debug.Assert(list.Count > 1);
                    animDir = list[1] - pos;
                    if (list.Count == 2)
                    {
                        anim = Animation.STORE;
                    }
                    else
                    {
                        anim = Animation.MOVE;
                    }
                }
            }
        }
    }


    public void deposit(GameState gs, IntVec2 pos)
    {
        Storage r = (gs.getItem(pos) != null && gs.getItem(pos) is Storage) ? (gs.getItem(pos) as Storage) : null;
        if (r == null || r.type != type)
        {
            return;
        }
        r.amount += amount;
        amount = 0;
    }


    public void collect(GameState gs, IntVec2 pos)
    {

        ExtractedResource r = (gs.getItem(pos) != null && gs.getItem(pos) is ExtractedResource) ? (gs.getItem(pos) as ExtractedResource) : null;
        if (r == null || r.type != type)
        {
            return;
        }

        int to_extract = Math.Min(r.amount, MAX_AMOUNT - r.amount);
        r.amount -= to_extract;
        amount += to_extract;
        if (r.amount == 0)
        {
            gs.tiles_[pos.x, pos.y] = null;
        }
    }
}
