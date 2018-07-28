using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPanel : MonoBehaviour
{
    public GameState gs;
    public void buyItem(string s)
    {
        switch(s)
        {
            case "sawmill": gs.placeItemNear(new Storage(Resource.Type.WOOD), new IntVec2((int)gs.player.location.x, (int)gs.player.location.y)); break;
            case "foundry": gs.placeItemNear(new Storage(Resource.Type.ORE), new IntVec2((int)gs.player.location.x, (int)gs.player.location.y)); break;
            case "refinery": gs.placeItemNear(new Storage(Resource.Type.OIL), new IntVec2((int)gs.player.location.x, (int)gs.player.location.y)); break;
        }
    }

    public void activate()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
