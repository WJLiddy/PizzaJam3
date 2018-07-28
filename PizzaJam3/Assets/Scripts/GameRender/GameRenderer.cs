using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRenderer : MonoBehaviour
{
    GameState gs;
    public TileRenderer tr;
    public PlayerRenderer pr;
    float tick_time_left;

    // Use this for initialization
    void Start ()
    {
        gs = new GameState(100);
        gs.addClearing(new IntVec2(0, 0), 10);
        gs.tiles_[0, 0] = new HarvesterRobot(Resource.Type.WOOD);
        tr.Setup(gs);
        pr.addPlayer();

    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {

        tick_time_left -= Time.deltaTime;
        if (tick_time_left < 0)
        {
            gs.process();
            gs.tick();
            tr.DrawState(gs);
            tick_time_left = GameState.TICK_TIME;
        }
    }
}
