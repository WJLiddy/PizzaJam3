﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUnit : TileItem
{
    public enum Animation
    {
        MOVE,
        HARVEST,
        IDLE
    }

    public Animation anim;
    public IntVec2 animDir;

    public static List<IntVec2> getPath(IntVec2 start, IntVec2 end, GameState gs)
    {
        Dictionary<IntVec2, int> dist = new Dictionary<IntVec2, int>();
        Dictionary<IntVec2, IntVec2> prev = new Dictionary<IntVec2, IntVec2>();

        dist[start] = 0;

        List<IntVec2> toSearch = new List<IntVec2>();
        toSearch.Add(start);

        while (toSearch.Count > 0)
        {
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
}