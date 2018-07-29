﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRenderer : MonoBehaviour
{
    public GameObject dfloats;
    public BuildPanel bp;
    GameState gs;
    public static readonly float TICK_TIME = 0.5f;
    public TileRenderer tr;
    public PlayerRenderer pr;
    public Light l;
    public Text time;
    public Text resourceText;
    float tick_time_left;

    public GameObject bItem1;
    public GameObject bItem2;

    public GameObject itemSlot1;
    public GameObject itemSlot2;

    // Use this for initialization
    void Start ()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30; // Sorry, my AI needs this extra time. Threading is out of the option since i can't clone();
        gs = new GameState(100);
        gs.addClearing(new IntVec2(0, 0), 10);
        gs.tiles_[0, 0] = new HarvesterRobot(Resource.Type.WOOD);
        gs.tiles_[1, 0] = new CollectorRobot(Resource.Type.WOOD);
        gs.tiles_[2, 0] = new Storage(Resource.Type.WOOD);
        tr.Setup(gs);
        gs.player = new Player();
        pr.addPlayer(gs);
        pr.p.gr = this; //disgusting
        gs.player.gun1 = (new MP5()).spawn(0.5f);
        gs.player.gun2 = (new ShortyShotgun()).spawn(0.5f);

        bp.gs = gs;

        Sound.audioSource = pr.gameObject.AddComponent<AudioSource>();
        Sound.PreLoad();

        gs.process(); //sets up animations and moves.

    }

    public void showOptions(TileItem tu)
    {
        if(tu is Storage)
        {
            bItem1.SetActive(true);
            bItem2.SetActive(true);
            bItem1.GetComponentInChildren<Text>().text = "Make [H]arvester";
            bItem2.GetComponentInChildren<Text>().text = "Make [C]ollector";
        } else
        {
            hideOptions();
        }
    }

    public void hideOptions()
    {
        bItem1.SetActive(false);
        bItem2.SetActive(false);
    }

    public void AddBullet(Gun.FiredProjectile fp, float base_angle_deg, Vector2 start)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("bullet"));
        float ang = base_angle_deg + fp.accuracy_modifier_degree;
        go.transform.localEulerAngles = new Vector3(0,0,ang);
        go.GetComponent<Rigidbody2D>().velocity = new Vector3(fp.speed * Mathf.Cos(ang * Mathf.Deg2Rad), fp.speed * Mathf.Sin(ang * Mathf.Deg2Rad));
        go.GetComponent<RenderBullet>().is_crit = fp.is_crit;
        go.GetComponent<RenderBullet>().range =  fp.range;
        go.GetComponent<RenderBullet>().gs = gs;
        go.GetComponent<RenderBullet>().gr = this;
        go.transform.localPosition = start;
    }

    internal void addDFloat(Vector3 localPosition, int dmg)
    {

        GameObject go = Instantiate(Resources.Load<GameObject>("d_float"));
        go.GetComponent<TextFadeOut>().text = "" + dmg;
        go.transform.SetParent(dfloats.transform);
        go.transform.localPosition = localPosition;

    }

    string getTime(int hr, int min)
    {
        if(hr < 1)
        {
            return "12:" + min.ToString("00") + " AM";
        }
        if(hr < 12)
        {
            return hr + ":" + min.ToString("00") + " AM";
        }
        if(hr == 12)
        {
            return hr + ":" + min.ToString("00") + " PM";
        }
        return hr - 12 + ":" + min.ToString("00") + " PM";
    }


    public void updateResourceCount()
    {
        int w, o, l;
        gs.resourceCount(out w, out o, out l);
        resourceText.text = "Wood " + w + "\nOre " + o + "\nOil " + l;
    }


    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        tick_time_left -= Time.deltaTime;
        if (tick_time_left < 0)
        { 
            gs.tick(); // actually moves the units. 
            gs.process(); //sets up animations and moves.
            tr.DrawState(gs,false); // all animation information is lost!
            tick_time_left = TICK_TIME;
            l.color = getLighting(gs.time_hr, gs.time_min);
            time.text = getTime(gs.time_hr, gs.time_min);
            updateResourceCount();
        }
    }

    public Color getLighting(int hr, int min)
    {
        //5 to 7 am
        //8 to 10 pm
        if(hr < 5)
        {
            return Color.black;
        }
        if(hr < 7)
        {
            float pct = 1 - ((7f - ((float)hr + (float)min / 60)) / 2);
            return new Color(pct, Mathf.Pow(pct, 2), Mathf.Pow(pct, 3));
        } 
        if(hr < 20)
        {
            return Color.white;
        }
        if(hr < 22)
        {
            float pct = ((22f - ((float)hr + (float)min / 60)) / 2);
            return new Color(Mathf.Pow(pct, 3), Mathf.Pow(pct, 2), pct);
        }
        if(hr < 24)
        {
            return Color.black;
        }
        return Color.black;
    }

    public void runOption(int opt)
    {
        IntVec2 v = pr.p.getLookAt();
        TileItem t = gs.getItem(v);
        if (t is Storage)
        {
            if(opt == 1)
            {
                gs.placeItemNear(new HarvesterRobot((t as Storage).type), v);
            }

            if(opt == 2)
            {
                gs.placeItemNear(new CollectorRobot((t as Storage).type), v);
            }
        }
    }
}
