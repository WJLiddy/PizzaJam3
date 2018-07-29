using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanel : MonoBehaviour
{
    public GameState gs;
    public GameRenderer gr;

    public void Awake()
    {
        GetComponentsInChildren<Text>()[0].text = 
            "Sawmill\nStores wood and creates lumberjack robots\n\n"+priceRender(BALANCE_CONSTANTS.SAWMILL_COST);
        GetComponentsInChildren<Text>()[1].text =
            "Foundry\nStores ore and creates mining robots\n\n" + priceRender(BALANCE_CONSTANTS.FOUNDRY_COST);
        GetComponentsInChildren<Text>()[2].text =
            "Refinery\nStores wood and creates drilling robots\n\n" + priceRender(BALANCE_CONSTANTS.REFINERY_COST);
        GetComponentsInChildren<Text>()[3].text =
            "Guard Tower\nActs as a light source, can shoot guns\n\n" + priceRender(BALANCE_CONSTANTS.TOWER_COST);
        GetComponentsInChildren<Text>()[4].text =
            "Gunsmith\nGiven resources, produces guns every 12 hours\n\n" + priceRender(BALANCE_CONSTANTS.GUNSMITH_COST);
    }

    public string priceRender(int[] res)
    {
        return "Wood: " + res[0] + "\nOre: " + res[1] + "\nOil " + res[2];
    }

    public void buyItem(string s)
    {
        TileUnit tu = null;
        switch(s)
        {
            case "sawmill": tu = new Storage(Resource.Type.WOOD); break;
            case "foundry": tu = new Storage(Resource.Type.ORE); break;
            case "refinery": tu = new Storage(Resource.Type.OIL); break;
            case "guardtower": tu = new GuardTower(); break;
            case "gunsmith": tu = new GunSmith(); break;
        }
        if(tu == null)
        {
            return;
        }
        tu.gr = gr;
        gs.placeItemNear(tu, new IntVec2((int)gs.player.location.x, (int)gs.player.location.y));

        if (tu is GuardTower)
        {
            gr.addGuardLight(new IntVec2((int)gs.player.location.x, (int)gs.player.location.y));
        }
    }

    public void activate()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
