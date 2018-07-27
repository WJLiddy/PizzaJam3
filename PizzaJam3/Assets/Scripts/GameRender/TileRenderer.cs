using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour
{
    GameObject[,] tiles;
    GameState gs;
    // Use this for initialization
    void Start ()
    {
        gs = new GameState(30);
        tiles = new GameObject[gs.dim_, gs.dim_];
        gs.addClearing(new IntVec2(0, 0), 10);
        gs.tiles_[0, 0] = new HarvesterRobot(Resource.Type.WOOD);
        prepareTiles();
        DrawState(gs);
	}

    void prepareTiles()
    {
        for (int x = 0; x != gs.dim_; x++)
        {
            for (int y = 0; y != gs.dim_; y++)
            {
                GameObject sprite = Instantiate(Resources.Load<GameObject>("generic_tile_item"));
                Sprite s = null;
                sprite.GetComponent<SpriteRenderer>().sprite = s;
                sprite.transform.SetParent(this.transform);
                sprite.transform.localPosition = new Vector2(x, y);
                tiles[x, y] = sprite;
            }
        }
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
                    renderResource(gs.tiles_[x,y] as Resource, x, y);
                } else if (gs.tiles_[x,y] is HarvesterRobot)
                {
                    renderRobot(gs.tiles_[x, y] as HarvesterRobot, x, y);
                } else if (gs.tiles_[x,y] is ExtractedResource)
                {
                    renderExtractedResource(gs.tiles_[x, y] as ExtractedResource, x, y);
                }
                
            }
        }

    }

    void renderExtractedResource(ExtractedResource r, int x, int y)
    {
        Sprite s = null;
        switch (r.type)
        {
            case Resource.Type.OIL: s = Resources.Load<Sprite>("Resource/oil_ext"); break;
            case Resource.Type.ORE: s = Resources.Load<Sprite>("Resource/ore_ext"); break;
            case Resource.Type.WOOD: s = Resources.Load<Sprite>("Resource/wood_ext"); break;
        }
        tiles[x, y].GetComponent<SpriteRenderer>().sprite = s;
    }



    void renderResource(Resource r,int x, int y)
    {
  
        Sprite s = null;
        switch(r.type)
        {
            case Resource.Type.OIL: s = Resources.Load<Sprite>("Resource/oil_res"); break;
            case Resource.Type.ORE: s = Resources.Load<Sprite>("Resource/ore_res"); break;
            case Resource.Type.WOOD: s = Resources.Load<Sprite>("Resource/wood_res"); break;
        }
        tiles[x,y].GetComponent<SpriteRenderer>().sprite = s;
    }

    void renderRobot(HarvesterRobot r, int x, int y)
    {
        Sprite s = null;
        switch (r.type)
        {
            case Resource.Type.OIL: s = Resources.Load<Sprite>("Robot/drillbot"); break;
            case Resource.Type.ORE: s = Resources.Load<Sprite>("Robot/minebot"); break;
            case Resource.Type.WOOD: s = Resources.Load<Sprite>("Robot/axebot"); break;
        }
        tiles[x, y].GetComponent<SpriteRenderer>().sprite = s;
    }


    void renderGrass(int x, int y)
    {
        Sprite s =  Resources.Load<Sprite>("grass");
        tiles[x, y].GetComponent<SpriteRenderer>().sprite = s;
    }

    // Update is called once per frame
    void Update ()
    {
        gs.process();
        gs.tick();
        DrawState(gs);
    }
}
