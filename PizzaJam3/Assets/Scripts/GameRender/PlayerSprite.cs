using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{

    float speedmod = 3.0f;
    float animTime = 0.5f;
    float animTimeMax = 0.5f;
    float cbox_radius = 0.2f;

    bool gun1_did_fire_last_frame = false;
    bool gun2_did_fire_last_frame = true;

    GameState gs;
    public GameRenderer gr;
    Dictionary<string, Sprite> sprs = new Dictionary<string, Sprite>();
	// Use this for initialization
	void Start ()
    {
        foreach(var v in Resources.LoadAll<Sprite>("Player"))
        {
            sprs[v.name] = v;
        }
	}

    public void setGameState(GameState gs)
    {
        this.gs = gs;
    }

    public void doAnim()
    {
        Vector2 old = transform.localPosition;
        string animcode = "";

        if (Input.GetKey("w"))
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + speedmod * Time.deltaTime);
            if (Input.GetKey("a"))
            {
                animcode = "ul";
            }
            else if (Input.GetKey("d"))
            {
                animcode = "ur";
            }
            else
            {
                animcode = "u";
            }
        }

        if (Input.GetKey("s"))
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - speedmod * Time.deltaTime);
            if (Input.GetKey("a"))
            {
                animcode = "dl";
            }
            else if (Input.GetKey("d"))
            {
                animcode = "dr";
            }
            else
            {
                animcode = "d";
            }
        }

        if (Input.GetKey("d"))
        {
            if (animcode == "")
            {
                animcode = "r";
            }
            transform.localPosition = new Vector2(transform.localPosition.x + speedmod * Time.deltaTime, transform.localPosition.y);
        }

        if (Input.GetKey("a"))
        {
            if (animcode == "")
            {
                animcode = "l";
            }
            transform.localPosition = new Vector2(transform.localPosition.x - speedmod * Time.deltaTime, transform.localPosition.y);
        }

        if (animcode != "")
        {
            GetComponent<SpriteRenderer>().sprite = sprs[animcode + (animTime <= 0 ? "2" : "")];
            animTime -= Time.deltaTime;
            if (animTime < -animTimeMax)
            {
                animTime = animTimeMax;
            }
        }
        else
        {
            animTime = 0;
        }

        if (wouldCollide())
        {
            transform.localPosition = old;
        }

        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }
	
    public bool wouldCollide()
    {
        Vector2 lp = transform.localPosition;
        if(lp.x - cbox_radius < 0 || lp.y - cbox_radius < 0)
        {
            return true;
        }
        return
            gs.tileWouldBeOccupied((int)(0.5f + lp.x - cbox_radius), (int)(0.5f + lp.y - cbox_radius)) ||
            gs.tileWouldBeOccupied((int)(0.5f + lp.x - cbox_radius), (int)(0.5f + lp.y + cbox_radius)) ||
            gs.tileWouldBeOccupied((int)(0.5f + lp.x + cbox_radius), (int)(0.5f + lp.y + cbox_radius)) ||
            gs.tileWouldBeOccupied((int)(0.5f + lp.x + cbox_radius), (int)(0.5f + lp.y - cbox_radius));
    }
	// Update is called once per frame
	void Update ()
    {
        doAnim();
        if(Input.GetMouseButton(0) && gs.player.gun1 != null)
        {
            if (!gun1_did_fire_last_frame || gs.player.gun1.isFullAuto())
            {
                gun1_did_fire_last_frame = true;
                shootGun(gr, gs.player.gun1);
            }
        } else
        {
            gun1_did_fire_last_frame = false;
        }

        if (Input.GetMouseButton(1) && gs.player.gun2 != null)
        {
            if (!gun2_did_fire_last_frame || gs.player.gun2.isFullAuto())
            {
                gun2_did_fire_last_frame = true;
                shootGun(gr, gs.player.gun2);
            }
        }
        else
        {
            gun2_did_fire_last_frame = false;
        }
    }

    public float getMouseAngle()
    {
        // Project the mouse point into world space at
        //   at the distance of the player.
        var v3Pos = Input.mousePosition;
        v3Pos.z = (transform.position.z - Camera.main.transform.position.z);
        v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);
        v3Pos = v3Pos - transform.position;
        var fAngle = Mathf.Atan2(v3Pos.y, v3Pos.x) * Mathf.Rad2Deg;
        if (fAngle < 0.0f) fAngle += 360.0f;
        return fAngle;
    }

    //renderer has to handle bullets.
    public void shootGun(GameRenderer gr, Gun g)
    {
        Player p = gs.player;

        if (g.getCapacity() > 0)
        {
            var v = g.fireGun();
            foreach(var b in v)
            {
                gun1_did_fire_last_frame = true;
                gr.AddBullet(b, getMouseAngle(),this.transform.localPosition);
            }

        }
    }

    
}
