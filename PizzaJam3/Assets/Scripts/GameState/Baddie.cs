using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Baddie : TileUnit
{
    // Breaks coupling, but no other way to handle bullets?

    public void move(IntVec2 dir)
    {
        anim = Animation.MOVE;
        animDir = dir;
    }


    public abstract void doAI(IntVec2 pos, GameState gs);
    public abstract int getVisibility();
}
