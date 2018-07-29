using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Baddie : TileUnit
{
    // Breaks coupling, but no other way to handle bullets?
    public abstract void doAI(IntVec2 pos, GameState gs);
 }
