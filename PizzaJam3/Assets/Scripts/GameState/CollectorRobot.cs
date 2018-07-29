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
        type = rt;
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
    public void doRobotAI(IntVec2 pos, GameState gs, ref IntVec2 animD, ref TileUnit.Animation animT)
    {
        animT = Animation.IDLE;
        animD = new IntVec2(0, 0);
        if (amount < MAX_AMOUNT)
        {
            IntVec2 r = find(pos, gs, collectable);
            if (r == null)
            {
                animT = Animation.IDLE;
            }

            if (r != null)
            {
                var list = getPath(pos, r, gs);
                if (list != null)
                {
                    // 1 means we didnt go anywhere.
                    Debug.Assert(list.Count > 1);
                    animD = list[1] - pos;
                    if (list.Count == 2)
                    {
                        animT = Animation.COLLECT;
                    }
                    else
                    {
                        animT = Animation.MOVE;
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
                animT = Animation.IDLE;
            }

            if (r != null)
            {
                var list = getPath(pos, r, gs);
                if (list != null)
                {
                    // 1 means we didnt go anywhere.
                    Debug.Assert(list.Count > 1);
                    animD = list[1] - pos;
                    if (list.Count == 2)
                    {
                        animT = Animation.STORE;
                    }
                    else
                    {
                        animT = Animation.MOVE;
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
        switch ((gs.getItem(pos) as Storage).type)
        {
            case Resource.Type.WOOD: gs.player_wood += amount; break;
            case Resource.Type.ORE: gs.player_ore += amount; break;
            case Resource.Type.OIL: gs.player_oil += amount; break;
        }
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

    public override int getMaxHP()
    {
        return BALANCE_CONSTANTS.WORKER_HP;
    }

    public override bool isFriendly()
    {
        return true;
    }
}
