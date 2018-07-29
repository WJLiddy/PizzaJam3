using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BALANCE_CONSTANTS
{
    public static int WORKER_HP = 30;
    public static int WAREHOUSE_HP = 200;
    public static int BADDIE_HP = 30;
    public static int TOWER_HP = 200;
    public static int BADDIE_HP_GROWTH_DAY = 20;

                                       // WOOD, ORE, OIL 
    public static int[] SAWMILL_COST = new int[3]{ 500, 0, 0 };
    public static int[] FOUNDRY_COST = new int[3] { 800, 0, 0 };
    public static int[] REFINERY_COST = new int[3] { 1000, 1000, 0 };
    public static int[] TOWER_COST = new int[3] { 300, 300, 0 };
    public static int[] GUNSMITH_COST = new int[3] { 500, 500, 500};

    public static int[] SAWMILL_WORKER_COST = new int[3] { 400, 0, 0 };
    public static int[] SAWMILL_COLLECTOR_COST = new int[3] { 300, 0, 0 };
    public static int[] FOUNDRY_WORKER_COST = new int[3] { 600, 0, 0 };
    public static int[] FOUNDRY_COLLECTOR_COST = new int[3] { 500, 0, 0 };
    public static int[] REFINERY_WORKER_COST = new int[3] { 800, 400, 0 };
    public static int[] REFINERY_COLLECTOR_COST = new int[3] { 700, 300, 0 };
}
