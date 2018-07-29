using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Robot
{
    void doRobotAI(IntVec2 pos, GameState gs, ref IntVec2 animD, ref TileUnit.Animation anim);
}
