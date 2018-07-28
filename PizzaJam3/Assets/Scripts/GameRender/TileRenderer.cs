using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour
{
    GameObject[,] tiles;
    // Use this for initialization
    public void Setup (GameState gs)
    {
        tiles = new GameObject[gs.dim_, gs.dim_];
        prepareTiles(gs.dim_);
        DrawState(gs);
	}

    void prepareTiles(int dim)
    {
        for (int x = 0; x != dim; x++)
        {
            for (int y = 0; y != dim; y++)
            {
                GameObject grass = Instantiate(Resources.Load<GameObject>("generic_tile_item"));
                grass.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("grass");
                grass.transform.SetParent(this.transform);
                grass.transform.localPosition = new Vector3(x, y, 2);

                GameObject sprite = Instantiate(Resources.Load<GameObject>("generic_tile_item"));
                Sprite s = null;
                sprite.GetComponent<SpriteRenderer>().sprite = s;
                sprite.transform.SetParent(this.transform);
                sprite.transform.localPosition = new Vector2(x, y);
                tiles[x, y] = sprite;

            }
        }
    }

    public void DrawState(GameState gs)
    {
        for(int x = 0; x != gs.dim_; x++)
        {
            for(int y = 0; y != gs.dim_; y++)
            {
                if (gs.tiles_[x, y] == null)
                {
                    tiles[x, y].GetComponent<SpriteRenderer>().sprite = null;
                }
                else if (gs.tiles_[x, y] is Resource)
                {
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
            case Resource.Type.OIL: s = Resources.Load<Sprite>("Resource/oil_ext"); break;
            case Resource.Type.ORE: s = Resources.Load<Sprite>("Resource/ore_ext"); break;
            case Resource.Type.WOOD: s = Resources.Load<Sprite>("Resource/wood_ext"); break;
        }
        tiles[x, y].GetComponent<SpriteRenderer>().sprite = s;
    }

    void renderStorage(Storage r, int x, int y)
    {
        Sprite s = null;
        switch (r.type)
        {
            case Resource.Type.OIL: s = Resources.Load<Sprite>("Resource/refinery"); break;
            case Resource.Type.ORE: s = Resources.Load<Sprite>("Resource/forge"); break;
            case Resource.Type.WOOD: s = Resources.Load<Sprite>("Resource/sawmill"); break;
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

    void renderCollectorRobot(CollectorRobot r, int x, int y)
    {
        Sprite s = null;
        switch (r.type)
        {
            case Resource.Type.OIL: s = Resources.Load<Sprite>("Robot/oilcollectorbot"); break;
            case Resource.Type.ORE: s = Resources.Load<Sprite>("Robot/orecollectorbot"); break;
            case Resource.Type.WOOD: s = Resources.Load<Sprite>("Robot/woodcollectorbot"); break;
        }
        tiles[x, y].GetComponent<SpriteRenderer>().sprite = s;
    }
}
