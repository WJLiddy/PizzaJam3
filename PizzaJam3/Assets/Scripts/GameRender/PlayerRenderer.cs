using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    public PlayerSprite p;
    public void addPlayer()
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("player_sprite"));
        p = go.GetComponent<PlayerSprite>();
        go.transform.SetParent(this.transform);
    }
}
