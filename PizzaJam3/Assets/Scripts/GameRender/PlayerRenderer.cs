using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    public PlayerSprite p;
    public void addPlayer(GameState gs)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("player_sprite"));
        p = go.GetComponent<PlayerSprite>();
        p.transform.localPosition = new Vector2(5f, 5f);
        p.setGameState(gs);
        go.transform.SetParent(this.transform);
    }
}
