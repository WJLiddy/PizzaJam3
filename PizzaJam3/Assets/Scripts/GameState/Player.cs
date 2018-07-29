using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Gun gun1;
    public Gun gun2;
    public Vector2 location;
    public float cbox_radius = 0.2f;
    public static float size;

    public HashSet<IntVec2> inTiles()
    {
        var lp = location;
        HashSet<IntVec2> iv = new HashSet<IntVec2>();
        iv.Add(new IntVec2((int)(lp.x + 0.5 + - cbox_radius), (int)(lp.y + 0.5 + - cbox_radius)));
        iv.Add(new IntVec2((int)(lp.x + 0.5 + - cbox_radius), (int)(lp.y + 0.5 + cbox_radius)));
        iv.Add(new IntVec2((int)(lp.x + 0.5 + cbox_radius), (int)(lp.y + 0.5 + - cbox_radius)));
        iv.Add(new IntVec2((int)(lp.x + 0.5 + cbox_radius), (int)(lp.y + 0.5 + cbox_radius)));
        return iv;
    }


}
