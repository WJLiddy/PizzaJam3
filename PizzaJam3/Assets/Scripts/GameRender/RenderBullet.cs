using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderBullet : MonoBehaviour
{
    public bool is_enemy_bullet;
    public bool is_crit;
    public float range;
    public Vector2 start;
    bool startSet = false;

    public void Update()
    {
        if(!startSet)
        {
            startSet = true;
            start = transform.localPosition;
        }
        if (Vector2.Distance(start, transform.localPosition) > range)
        {
            Destroy(this.gameObject);
        }
    }
    // See what the other object is. If it's a 
    private void OnTriggerCollide(Collider other)
    {
            // get name. it will be a coordinate (x,y) or the player which we can look up and handle accordingly
    }
}
