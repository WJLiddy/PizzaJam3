using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPanel : MonoBehaviour
{
    public GameState gs;
    public GameRenderer gr;
    public void buyItem(string s)
    {
        TileUnit tu = null;
        switch(s)
        {
            case "sawmill": tu = new Storage(Resource.Type.WOOD); break;
            case "foundry": tu = new Storage(Resource.Type.ORE); break;
            case "refinery": tu = new Storage(Resource.Type.OIL); break;
            case "guardtower": tu = new GuardTower(); break;
        }
        if(tu == null)
        {
            return;
        }
        tu.gr = gr;
        gs.placeItemNear(tu, new IntVec2((int)gs.player.location.x, (int)gs.player.location.y));
    }

    public void activate()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
