using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderBullet
{
    // See what the other object is. If it's a 
    private void OnTriggerStay(Collider other)
    {
        if (stay)
        {
            if (stayCount > 0.25f)
            {
                Debug.Log("staying");
                stayCount = stayCount - 0.25f;
            }
            else
            {
                stayCount = stayCount + Time.deltaTime;
            }
        }
    }
}
