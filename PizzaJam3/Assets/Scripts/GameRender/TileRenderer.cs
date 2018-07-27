using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour
{
    List<GameObject> children = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        GameState gs = new GameState(100);
        gs.addClearing(new IntVec2(0, 0), 10);
        gs.tiles_[0, 0] = new Robot(Resource.Type.WOOD);
        DrawState(gs);
	}

    void DrawState(GameState gs)
    {
        for(int x = 0; x != gs.dim_; x++)
        {
            for(int y = 0; y != gs.dim_; y++)
            {
                if(gs.tiles_[x,y] == null)
                {
                    renderGrass(x, y);
                } else if (gs.tiles_[x,y] is Resource)
                {
                    renderResouce(gs.tiles_[x,y] as Resource, x, y);
                } else if (gs.tiles_[x,y] is Robot)
                {
                    renderRobot(gs.tiles_[x, y] as Robot, x, y);
                }
                
            }
        }

    }

    void renderResouce(Resource r,int x, int y)
    {
        GameObject sprite = Instantiate(Resources.Load<GameObject>("generic_tile_item"));
        Sprite s = null;
        switch(r.type)
        {
            case Resource.Type.OIL: s = Resources.Load<Sprite>("Resource/oil_res"); break;
            case Resource.Type.ORE: s = Resources.Load<Sprite>("Resource/ore_res"); break;
            case Resource.Type.WOOD: s = Resources.Load<Sprite>("Resource/wood_res"); break;
        }
        sprite.GetComponent<SpriteRenderer>().sprite = s;
        sprite.transform.SetParent(this.transform);
        sprite.transform.localPosition = new Vector2(x, y);
    }

    void renderRobot(Robot r, int x, int y)
    {
        GameObject sprite = Instantiate(Resources.Load<GameObject>("generic_tile_item"));
        Sprite s = null;
        switch (r.type)
        {
            case Resource.Type.OIL: s = Resources.Load<Sprite>("Resource/drillbot"); break;
            case Resource.Type.ORE: s = Resources.Load<Sprite>("Resource/minebot"); break;
            case Resource.Type.WOOD: s = Resources.Load<Sprite>("Resource/axebot"); break;
        }
        sprite.GetComponent<SpriteRenderer>().sprite = s;
        sprite.transform.SetParent(this.transform);
        sprite.transform.localPosition = new Vector2(x, y);
    }


    void renderGrass(int x, int y)
    {
        GameObject sprite = Instantiate(Resources.Load<GameObject>("generic_tile_item"));
        Sprite s =  Resources.Load<Sprite>("grass");
        sprite.GetComponent<SpriteRenderer>().sprite = s;
        sprite.transform.SetParent(this.transform);
        sprite.transform.localPosition = new Vector2(x, y);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
