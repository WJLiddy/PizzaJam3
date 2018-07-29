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
            "Sawmill\nStores wood and creates lumberjack robots\n\n"+gs.priceRender(BALANCE_CONSTANTS.SAWMILL_COST);
        GetComponentsInChildren<Text>()[1].text =
            "Foundry\nStores ore and creates mining robots\n\n" + gs.priceRender(BALANCE_CONSTANTS.FOUNDRY_COST);
        GetComponentsInChildren<Text>()[2].text =
            "Refinery\nStores wood and creates drilling robots\n\n" + gs.priceRender(BALANCE_CONSTANTS.REFINERY_COST);
        GetComponentsInChildren<Text>()[3].text =
            "Guard Tower\nActs as a light source, can shoot guns\n\n" + gs.priceRender(BALANCE_CONSTANTS.TOWER_COST);
        GetComponentsInChildren<Text>()[4].text =
            "Gunsmith\nGiven resources, produces guns every 12 hours\n\n" + gs.priceRender(BALANCE_CONSTANTS.GUNSMITH_COST);
    }

    public void buyItem(string s)
    {
        TileUnit tu = null;
        switch(s)
        {
            case "sawmill":
                if(gs.tryBuy(BALANCE_CONSTANTS.SAWMILL_COST))
                tu = new Storage(Resource.Type.WOOD); break;
            case "foundry":
                if (gs.tryBuy(BALANCE_CONSTANTS.FOUNDRY_COST))
                    tu = new Storage(Resource.Type.ORE); break;
            case "refinery":
                if (gs.tryBuy(BALANCE_CONSTANTS.REFINERY_COST))
                    tu = new Storage(Resource.Type.OIL); break;
            case "guardtower":
                if (gs.tryBuy(BALANCE_CONSTANTS.TOWER_COST))
                    tu = new GuardTower(); break;
            case "gunsmith":
                if (gs.tryBuy(BALANCE_CONSTANTS.GUNSMITH_COST))
                    tu = new GunSmith(); break;
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

        if(tu != null)
        {
            activate();
        }
    }

    public void activate()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
