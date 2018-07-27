using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExtractedResource : TileItem
{

    public Resource.Type type;
    public int amount;

    public ExtractedResource(Resource.Type type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }
}
