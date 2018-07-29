using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSprite : MonoBehaviour
{

    float speedmod = 3.0f;
    float animTime = 0.5f;
    float animTimeMax = 0.5f;
    float cbox_radius = 0.2f;

    bool gun1_did_fire_last_frame = false;
    bool gun2_did_fire_last_frame = true;

    float reload_timer_1 = 0;
    float reload_timer_2 = 0;

    float auto_recoil_timer_1 = 0;
    float auto_recoil_timer_2 = 0;

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

        if (.5 + lp.x - cbox_radius <= 0 || .5 + lp.y - cbox_radius <= 0)
        {
            return true;
        }
            return
            gs.tileWouldBeOccupied((int)(.5 + lp.x - cbox_radius), (int)(.5 + lp.y - cbox_radius)) ||
            gs.tileWouldBeOccupied((int)(.5 + lp.x - cbox_radius), (int)(.5 + lp.y + cbox_radius)) ||
            gs.tileWouldBeOccupied((int)(.5 + lp.x + cbox_radius), (int)(.5 + lp.y + cbox_radius)) ||
            gs.tileWouldBeOccupied((int)(.5 + lp.x + cbox_radius), (int)(.5 + lp.y - cbox_radius));

    }

    void updateItemSlot()
    {
        if (gs.player.gun1 != null)
        {
            gr.itemSlot1.GetComponentInChildren<Text>().text = gs.player.gun1.getName() + "\n" +
                ((reload_timer_1 > 0) ? "reloading" : gs.player.gun1.bulletsLeft + "/" + gs.player.gun1.getCapacity());
        } else
        {
            gr.itemSlot1.GetComponentInChildren<Text>().text = "None";
        }

        if (gs.player.gun2 != null)
        {
            gr.itemSlot2.GetComponentInChildren<Text>().text = gs.player.gun2.getName() + "\n" +
            ((reload_timer_2 > 0) ? "reloading" : gs.player.gun2.bulletsLeft + "/" + gs.player.gun2.getCapacity());
        } else
        {
            gr.itemSlot2.GetComponentInChildren<Text>().text = "None";
        }
    }

    void gunHandler()
    {
        if(Input.GetKeyDown("q") && reload_timer_1 <= 0 && gs.player.gun1 != null)
        {
            gs.player.gun1.bulletsLeft = gs.player.gun1.getCapacity();
            reload_timer_1 = gs.player.gun1.getReloadTime();
        }

        if (Input.GetKeyDown("e") && reload_timer_2 <= 0 && gs.player.gun2 != null)
        {
            gs.player.gun2.bulletsLeft = gs.player.gun2.getCapacity();
            reload_timer_2 = gs.player.gun2.getReloadTime();
        }

        if (Input.GetMouseButton(0) && gs.player.gun1 != null && reload_timer_1 <= 0 && auto_recoil_timer_1 <= 0)
        {
            if (!gun1_did_fire_last_frame || gs.player.gun1.isFullAuto())
            {
                if(gs.player.gun1.isFullAuto())
                {
                    auto_recoil_timer_1 = gs.player.gun1.getROF();
                }

                gun1_did_fire_last_frame = true;
                shootGun(gr, gs.player.gun1);
            }
        }
        else
        {
            gun1_did_fire_last_frame = false;
        }

        if (Input.GetMouseButton(1) && gs.player.gun2 != null && reload_timer_2 <= 0 && auto_recoil_timer_2 <= 0)
        {
            if (!gun2_did_fire_last_frame || gs.player.gun2.isFullAuto())
            {
                if (gs.player.gun2.isFullAuto())
                {
                    auto_recoil_timer_2 = gs.player.gun2.getROF();
                }
                gun2_did_fire_last_frame = true;
                shootGun(gr, gs.player.gun2);
            }
        }
        else
        {
            gun2_did_fire_last_frame = false;
        }

        if(reload_timer_1 > 0)
        {
            reload_timer_1 -= Time.deltaTime;
        }


        if (reload_timer_2 > 0)
        {
            reload_timer_2 -= Time.deltaTime;
        }

        if (auto_recoil_timer_1 > 0)
        {
            auto_recoil_timer_1 -= Time.deltaTime;
        }


        if (auto_recoil_timer_2 > 0)
        {
            auto_recoil_timer_2 -= Time.deltaTime;
        }
    }

    IntVec2 getFacing()
    {
        switch (GetComponent<SpriteRenderer>().sprite.name)
        {
            case "u": case "u2": return new IntVec2(0, 1);
            case "ur": case "ur2": return new IntVec2(1, 1);
            case "r": case "r2": return new IntVec2(1, 0);
            case "dr": case "dr2": return new IntVec2(1, -1);
            case "d": case "d2": return new IntVec2(0, -1);
            case "dl": case "dl2": return new IntVec2(-1, -1);
            case "l": case "l2": return new IntVec2(-1, 0);
            case "ul": case "ul2": return new IntVec2(-1, 1);
        }
        return new IntVec2(0, 0);
    }

    public IntVec2 getLookAt()
    {
        return new IntVec2((int)(gs.player.location.x + 0.5 + getFacing().x), (int)(gs.player.location.y + 0.5 + getFacing().y));
    }

    void optionsWatchdog()
    {
        IntVec2 lookat = getLookAt();
        if(!gs.isOOB(lookat) &&  gs.getItem(lookat) != null)
        {
            gr.showOptions(gs.getItem(lookat));
        } else
        {
            gr.hideOptions();
        }
    }
	// Update is called once per frame
	void Update ()
    {
        doAnim();

        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            gunHandler();

        optionsWatchdog();

        gs.player.location = transform.localPosition;

        updateItemSlot();
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

        if (g.bulletsLeft > 0)
        {
            var v = g.fireGun();
            foreach(var b in v)
            {
                if(g.bulletsLeft == 0)
                {
                    return;
                }
                if (g.consumeMultipleAmmoPerFire())
                {
                    g.bulletsLeft--;
                }
                gun1_did_fire_last_frame = true;
                gr.AddBullet(b, getMouseAngle(),this.transform.localPosition,true);
            }
            if (!g.consumeMultipleAmmoPerFire())
            {
                g.bulletsLeft--;
            }
        }
    }

    
}
