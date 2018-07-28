using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour
{
    GameObject[,] tiles;
    Dictionary<string, Sprite> sd = new Dictionary<string, Sprite>();
    // Use this for initialization
    public void Setup (GameState gs)
    {
        foreach(var t in Resources.LoadAll<Sprite>("Tiles/"))
        {
            sd[t.name] = t;
        }

        tiles = new GameObject[gs.dim_, gs.dim_];
        prepareTiles(gs.dim_);
        DrawState(gs,true);
	}

    void prepareTiles(int dim)
    {
        for (int x = 0; x != dim; x++)
        {
            for (int y = 0; y != dim; y++)
            {
                GameObject grass = Instantiate(Resources.Load<GameObject>("generic_tile_item"));
                grass.name = "grass";
                grass.GetComponent<SpriteRenderer>().sprite = sd["grass"];
                grass.transform.SetParent(this.transform);
                grass.transform.localPosition = new Vector3(x, y, 0.01f);

                GameObject sprite = Instantiate(Resources.Load<GameObject>("generic_tile_item"));
                sprite.name = x + "," + y;
                Sprite s = null;
                sprite.GetComponent<SpriteRenderer>().sprite = s;
                sprite.transform.SetParent(this.transform);
                sprite.transform.localPosition = new Vector2(x, y);
                tiles[x, y] = sprite;

            }
        }
    }

    public void DrawState(GameState gs, bool first)
    {
        for(int x = 0; x != gs.dim_; x++)
        {
            for(int y = 0; y != gs.dim_; y++)
            {
                if (gs.tiles_[x, y] == null)
                {
                    tiles[x, y].GetComponent<SpriteRenderer>().sprite = null;
                }
                // Resources only disappear, so can be rendered once and forgotten about. (until replacement.)
                else if (gs.tiles_[x, y] is Resource)
                {
                    if(!first)
                    {
                        continue;
                    }
                    renderResource(gs.tiles_[x, y] as Resource, x, y);
                }
                else if (gs.tiles_[x, y] is HarvesterRobot)
                {
                    renderRobot(gs.tiles_[x, y] as HarvesterRobot, x, y);
                }
                else if (gs.tiles_[x, y] is CollectorRobot)
                {
                    renderCollectorRobot(gs.tiles_[x, y] as CollectorRobot, x, y);
                }
                else if (gs.tiles_[x, y] is ExtractedResource)
                {
                    renderExtractedResource(gs.tiles_[x, y] as ExtractedResource, x, y);
                }
                else if (gs.tiles_[x, y] is Storage)
                {
                    renderStorage(gs.tiles_[x, y] as Storage, x, y);
                }

            }
        }

    }

    void renderExtractedResource(ExtractedResource r, int x, int y)
    {
        Sprite s = null;
        switch (r.type)
        {
            case Resource.Type.OIL: s = sd["oil_ext"]; break;
            case Resource.Type.ORE: s = sd["ore_ext"]; break;
            case Resource.Type.WOOD: s = sd["wood_ext"]; break;
        }
        tiles[x, y].GetComponent<SpriteRenderer>().sprite = s;
    }

    void renderStorage(Storage r, int x, int y)
    {
        Sprite s = null;
        switch (r.type)
        {
            case Resource.Type.OIL: s = sd["refinery"]; break;
            case Resource.Type.ORE: s = sd["forge"]; break;
            case Resource.Type.WOOD: s =sd["sawmill"]; break;
        }
        tiles[x, y].GetComponent<SpriteRenderer>().sprite = s;
    }



    void renderResource(Resource r,int x, int y)
    {
        Sprite s = null;
        switch(r.type)
        {
            case Resource.Type.OIL: s = sd["oil_res"]; break;
            case Resource.Type.ORE: s = sd["ore_res"]; break;
            case Resource.Type.WOOD: s = sd["wood_res"]; break;
        }
        tiles[x,y].GetComponent<SpriteRenderer>().sprite = s;
    }

    void renderRobot(HarvesterRobot r, int x, int y)
    {
        Sprite s = null;
        switch (r.type)
        {
            case Resource.Type.OIL: s = sd["drillbot"]; break;
            case Resource.Type.ORE: s = sd["pickbot"]; break;
            case Resource.Type.WOOD: s = sd["axebot"]; break;
        }
        tiles[x, y].GetComponent<SpriteRenderer>().sprite = s;
    }

    void renderCollectorRobot(CollectorRobot r, int x, int y)
    {
        Sprite s = null;
        switch (r.type)
        {
            case Resource.Type.OIL: s = sd["oilcollectorbot"]; break;
            case Resource.Type.ORE: s = sd["orecollectorbot"]; break;
            case Resource.Type.WOOD: s = sd["woodcollectorbot"]; break;
        }
        tiles[x, y].GetComponent<SpriteRenderer>().sprite = s;
    }
}
