using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRenderer : MonoBehaviour
{
    public GameObject dfloats;
    public BuildPanel bp;
    // FUCK SHIT AAAAAAAAAAAAAHHHHHHHHHHHHHHHH
    public Dictionary<IntVec2, GameObject> guardlights = new Dictionary<IntVec2, GameObject>();
    GameState gs;
    bool in_ai_phase = false;
    public static readonly float AI_TIME = 0.9f;
    public static readonly float DRAW_TIME = 0.1f;
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
    public GameObject itemSlot1ReloadRem;
    public GameObject itemSlot2ReloadRem;
    public struct animSet
    {
        public IntVec2 start;
        public IntVec2 anim;
        public TileUnit.Animation animType;
    }

    List<animSet> ANIMSETS;
    int GLOBAL_ANIM_X;
    int GLOBAL_ANIM_Y;
    // Use this for initialization
    void Start ()
    {
        Application.targetFrameRate = 20;
        QualitySettings.vSyncCount = 0;
        gs = new GameState(100);
        gs.addClearing(new IntVec2(0, 0), 10);
        gs.tiles_[7, 6] = new HarvesterRobot(Resource.Type.WOOD);
        gs.tiles_[6, 7] = new CollectorRobot(Resource.Type.WOOD);
        gs.tiles_[7, 7] = new Storage(Resource.Type.WOOD);

        GuardTower g = new GuardTower();
        g.gr = this;
        addGuardLight(new IntVec2(8, 8));
        gs.tiles_[8, 8] = g;
        
        gs.player_wood = 2000;
        gs.time_hr = 9;


   

        gs.tiles_[4, 4] = new GroundGun(new MP5().spawn(0));
        gs.tiles_[4, 5] = new GroundGun(new M4().spawn(0));
        gs.tiles_[4, 6] = new GroundGun(new PumpShotgun().spawn(0));
        gs.tiles_[4, 7] = new GroundGun(new TightCannon().spawn(0));
        gs.tiles_[4, 8] = new GroundGun(new RPG().spawn(0));
        /** GUNS
         * 
         * 
         * 
         * 
         * */
        tr.Setup(gs);
        gs.player = new Player();
        pr.addPlayer(gs);
        pr.p.gr = this; //disgusting
        gs.player.gun1 = (new Revolver()).spawn(0f);
        gs.player.gun2 = (new Revolver()).spawn(0f);

        bp.gs = gs;

        Sound.audioSource = pr.gameObject.AddComponent<AudioSource>();
        Sound.PreLoad();
        List<animSet> sets = new List<animSet>();
        int x = 0; int y = 0;
        while (true)
        {
            animSet a;
            if(!gs.processOne(ref x, ref y, out a))//sets up animations and moves.
            {
                break;
            }
            sets.Add(a);
        }
        applyAnimSets(sets);
        gs.tick(this);

        bp.gr = this;

        ANIMSETS = new List<animSet>();
    }


    public void applyAnimSets(List<animSet> l)
    {
        foreach(var v in l)
        {
            TileUnit tu = gs.tiles_[v.start.x, v.start.y] as TileUnit;
            if(tu is Robot || tu is Baddie)
            {
                tu.anim = v.animType;
                tu.animDir = v.anim;
            }
        }
    }

    public string resourceName(Resource.Type t)
    {
        switch(t)
        {
            case Resource.Type.WOOD: return "Wood";
            case Resource.Type.ORE: return "Ore";
            case Resource.Type.OIL: return "Oil";
        }
        return "";
    }

    public void showOptions(TileItem tu)
    {
        if(tu is Storage)
        {
            bItem1.SetActive(true);
            bItem2.SetActive(true);

            int[] costof1 = null;
            switch ((tu as Storage).type)
            {
                case Resource.Type.OIL: costof1 = BALANCE_CONSTANTS.REFINERY_WORKER_COST; break;
                case Resource.Type.WOOD: costof1 = BALANCE_CONSTANTS.SAWMILL_WORKER_COST; break;
                case Resource.Type.ORE: costof1 = BALANCE_CONSTANTS.FOUNDRY_WORKER_COST; break;
            }

            int[] costof2 = null;
            switch ((tu as Storage).type)
            {
                case Resource.Type.OIL: costof2 = BALANCE_CONSTANTS.REFINERY_COLLECTOR_COST; break;
                case Resource.Type.WOOD: costof2 = BALANCE_CONSTANTS.SAWMILL_COLLECTOR_COST; break;
                case Resource.Type.ORE: costof2 = BALANCE_CONSTANTS.FOUNDRY_COLLECTOR_COST; break;
            }

            bItem1.GetComponentInChildren<Text>().text = "Build " + resourceName((tu as Storage).type) + " Harvester\n" + gs.priceRenderNoNewline(costof1);
            bItem2.GetComponentInChildren<Text>().text = "Build " + resourceName((tu as Storage).type) + " Collector\n" + gs.priceRenderNoNewline(costof2);
        } else if(tu is GuardTower)
        {
            bItem1.SetActive(gs.player.gun1 != null);
            if (gs.player.gun1 != null)
            {
                bItem1.GetComponentInChildren<Text>().text = "Attach " + gs.player.gun1.getName();
            }

            bItem2.SetActive(gs.player.gun2 != null);
            if (gs.player.gun2 != null)
            {
                bItem2.GetComponentInChildren<Text>().text = "Attach " + gs.player.gun2.getName();
            }

            if((tu as GuardTower).storedGun != null)
            {
                bItem1.SetActive(false);
                bItem2.SetActive(false);
            }
        }

        else if (tu is GroundGun)
        {

            bItem1.SetActive(true);
            bItem2.SetActive(true);
            GroundGun gg = (tu as GroundGun);
            if (gs.player.gun1 == null)
            {
                bItem1.GetComponentInChildren<Text>().text = "Take " + gg.gun.getName();
            } else
            {
                bItem1.GetComponentInChildren<Text>().text = "Swap for " + gg.gun.getName();
            }

            if (gs.player.gun2 == null)
            {
                bItem2.GetComponentInChildren<Text>().text = "Take " + gg.gun.getName();
            }
            else
            {
                bItem2.GetComponentInChildren<Text>().text = "Swap for " + gg.gun.getName();
            }
        }


        else
        {
            hideOptions();
        }
    }

    public void addGuardLight( IntVec2 iv)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("tower_light"));
        go.transform.SetParent(this.transform);
        go.transform.localPosition = new Vector3(iv.x, iv.y, -4);

        /**
        for(int i = -1; i != 2; ++i)
        {
            for(int j = -1; j != 2; ++j)
            {
                if(i == 0 && j == 0)
                {
                    continue;
                }

                if(!gs.isOOB(new IntVec2(iv.x+i,iv.y+j)) && gs.getItem(new IntVec2(iv.x + i, iv.y + j)) != null &&
                     gs.getItem(new IntVec2(iv.x + i, iv.y + j)) is GuardTower)
                {
                    if(guardlights[new IntVec2(iv.x + i, iv.y + j)].GetComponent<Light>().intensity > 0)
                    dim = true;
                }
            }
        }

       // go.GetComponent<Light>().intensity = (dim ? 0 : 0.8f);
    */
        guardlights[iv] = go;
    }

    public void towersUpdate(GameState gs)
    {
        foreach(var v in guardlights.Values)
        {
            v.SetActive(gs.time_hr < 6 || gs.time_hr > 20);
        }
    }
    public void hideOptions()
    {
        bItem1.SetActive(false);
        bItem2.SetActive(false);
    }

    public void AddBullet(Gun.FiredProjectile fp, float base_angle_deg, Vector2 start, bool allied)
    {

        GameObject go = null;
        switch (fp.projectile)
        {
            case Projectile.ProjectileType.Bullet: go = Instantiate(Resources.Load<GameObject>("bullet"));
                break;
            case Projectile.ProjectileType.CannonBall:
                go = Instantiate(Resources.Load<GameObject>("cannonball"));
                break;
            case Projectile.ProjectileType.Musket:
                go = Instantiate(Resources.Load<GameObject>("musket"));
                break;
            case Projectile.ProjectileType.Rocket:
                go = Instantiate(Resources.Load<GameObject>("rocket"));
                break;
        }

        float ang = base_angle_deg + fp.accuracy_modifier_degree;
        go.transform.localEulerAngles = new Vector3(0,0,ang);
        go.GetComponent<Rigidbody2D>().velocity = new Vector3(fp.speed * Mathf.Cos(ang * Mathf.Deg2Rad), fp.speed * Mathf.Sin(ang * Mathf.Deg2Rad));
        go.GetComponent<RenderBullet>().is_crit = fp.is_crit;
        go.GetComponent<RenderBullet>().range =  fp.range;
        go.GetComponent<RenderBullet>().gs = gs;
        go.GetComponent<RenderBullet>().friendlyBullet = allied;
        go.GetComponent<RenderBullet>().gr = this;
        go.transform.localPosition = start;
    }

    internal void addDFloat(Vector3 localPosition, string dmg)
    {

        GameObject go = Instantiate(Resources.Load<GameObject>("d_float"));
        go.GetComponent<TextFadeOut>().text =  dmg;
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
        if (in_ai_phase)
        {
            for (int i = 0; i != 3; ++i)
            {
                animSet a;
                if (gs.processOne(ref GLOBAL_ANIM_X, ref GLOBAL_ANIM_Y, out a))
                {
                    ANIMSETS.Add(a);
                }
            }
        }

        tick_time_left -= Time.deltaTime;

        if (tick_time_left < 0)
        {
            if (in_ai_phase)
            {
                applyAnimSets(ANIMSETS);
                ANIMSETS.Clear();
                if (GLOBAL_ANIM_X != gs.dim_ || GLOBAL_ANIM_Y != 0)
                {
                    Debug.Log("failed to render in time!");
                }
                else
                {
                    GLOBAL_ANIM_X = 0;
                    GLOBAL_ANIM_Y = 0;
                }
                in_ai_phase = false;
                tr.DrawState(gs,false);
                tick_time_left = DRAW_TIME;
            } else
            {
                //draw state ended, put people down.
                gs.tick(this); // actually moves the units. 
                tr.DrawState(gs, false); // all animation information is lost, but Ai is now thinking.
                tick_time_left = AI_TIME;
                l.color = getLighting(gs.time_hr, gs.time_min);
                time.text = getTime(gs.time_hr, gs.time_min);
                updateResourceCount();
                towersUpdate(gs);
                in_ai_phase = true;
            }
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
        Player p = gs.player;
        IntVec2 v = pr.p.getLookAt();
        TileItem t = gs.getItem(v);

        if (t is Storage)
        {
            int[] costof = null;
            if (opt == 1)
            {
                switch ((t as Storage).type)
                {
                    case Resource.Type.OIL: costof = BALANCE_CONSTANTS.REFINERY_WORKER_COST; break;
                    case Resource.Type.WOOD: costof = BALANCE_CONSTANTS.SAWMILL_WORKER_COST; break;
                    case Resource.Type.ORE: costof = BALANCE_CONSTANTS.FOUNDRY_WORKER_COST; break;
                }
                if (gs.tryBuy(costof))
                {
                    gs.placeItemNear(new HarvesterRobot((t as Storage).type), v);
                }
            }

            if (opt == 2)
            {
                switch ((t as Storage).type)
                {
                    case Resource.Type.OIL: costof = BALANCE_CONSTANTS.REFINERY_COLLECTOR_COST; break;
                    case Resource.Type.WOOD: costof = BALANCE_CONSTANTS.SAWMILL_COLLECTOR_COST; break;
                    case Resource.Type.ORE: costof = BALANCE_CONSTANTS.FOUNDRY_COLLECTOR_COST; break;
                }
                if (gs.tryBuy(costof))
                {
                    gs.placeItemNear(new CollectorRobot((t as Storage).type), v);
                }
            }
        }

        if (t is GuardTower)
        {
            GuardTower gt = t as GuardTower;
            if (opt == 1)
            {
                if (gt.storedGun == null && p.gun1 != null)
                {
                    gt.storedGun = p.gun1;
                    p.gun1 = null;
                }
            }

            if (opt == 2)
            {
                if (gt.storedGun == null && p.gun2 != null)
                {
                    gt.storedGun = p.gun2;
                    p.gun2 = null;
                }
            }
        }

        if (t is GroundGun)
        {
            GroundGun gg = (t as GroundGun);
            if (opt == 1 && gs.player.gun1 == null)
            {
                gs.player.gun1 = gg.gun;
                gs.tiles_[v.x, v.y] = null;
            }
            if (opt == 1 && gs.player.gun1 != null)
            {
                Gun g = gg.gun;
                gg.gun = gs.player.gun1;
                gs.player.gun1 = g;
            }

            if (opt == 2 && gs.player.gun2 == null)
            {
                gs.player.gun2 = gg.gun;
                gs.tiles_[v.x, v.y] = null;
            }
            if (opt == 2 && gs.player.gun2 != null)
            {
                Gun g = gg.gun;
                gg.gun = gs.player.gun2;
                gs.player.gun2 = g;
            }
        }
    }
}