using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : TileItem
{
    public enum Type
    {
        WOOD, OIL, ORE
    }

    public Type type;
    public int amount;

    private Resource(Type type, int amount )
    {
        this.type = type;
        this.amount = amount;
    }

    private static Resource makeTree()
    {
        return new Resource(Type.WOOD, 200);
    }

    private static Resource makeOre()
    {
        return new Resource(Type.ORE, 40 * (int)Random.Range(1, 5));
    }

    private static Resource makeOil()
    { 
        return new Resource(Type.OIL, 100 * (int)Random.Range(5, 15));
    }

    private static void createWood(GameState g, IntVec2 iv)
    {
        g.tiles_[iv.x, iv.y] = makeTree();
    }

    private static void createOre(GameState g, IntVec2 iv)
    {
        g.tiles_[iv.x, iv.y] = makeOre();
    }

    private static void createOil(GameState g, IntVec2 iv)
    {
        g.tiles_[iv.x, iv.y] = makeOil();
    }

    public static void AddWood(GameState g, IntVec2 iv)
    {
        createWood(g, iv);
    }

    public static void AddOre(GameState g, IntVec2 iv)
    {
        int count = Random.Range(5, 10);
        for (int i = 0; i != count; ++i)
        {
            IntVec2 delt = new IntVec2(Random.Range(-5, 5), Random.Range(-5, 5));
            if (!g.isOOB(iv + delt))
            {
                createOre(g, iv + delt);
            }
        }
    }

    public static void AddOil(GameState g, IntVec2 iv)
    {
        createOil(g, iv);
    }

}
