﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{

    float speedmod = 3.0f;
    float animTime = 0.5f;
    float animTimeMax = 0.5f;
    GameState gs;
    Dictionary<string, Sprite> sprs = new Dictionary<string, Sprite>();
	// Use this for initialization
	void Start ()
    {
        foreach(var v in Resources.LoadAll<Sprite>("Player"))
        {
            sprs[v.name] = v;
        }
	}

    void setGameState(GameState gs)
    {
        this.gs = gs;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 old = transform.localPosition;
        string animcode = "";

        if (Input.GetKey("w"))
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + speedmod * Time.deltaTime);
            if(Input.GetKey("a"))
            {
                animcode = "ul";
            } else if (Input.GetKey("d"))
            {
                animcode = "ur";
            } else
            {
                animcode = "u";
            }
        }

        if (Input.GetKey("s"))
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - speedmod * Time.deltaTime);
            if (Input.GetKey("a"))
            {
                animcode = "dl";
            }
            else if (Input.GetKey("d"))
            {
                animcode = "dr";
            }
            else
            {
                animcode = "d";
            }
        }

        if (Input.GetKey("d"))
        {
            if(animcode == "")
            {
                animcode = "r";
            }
            transform.localPosition = new Vector2(transform.localPosition.x + speedmod * Time.deltaTime, transform.localPosition.y);
        }

        if (Input.GetKey("a"))
        {
            if (animcode == "")
            {
                animcode = "l";
            }
            transform.localPosition = new Vector2(transform.localPosition.x - speedmod * Time.deltaTime, transform.localPosition.y);
        }

        if(animcode != "")
        {
            GetComponent<SpriteRenderer>().sprite = sprs[animcode + (animTime <= 0 ? "2" : "")];
            animTime -= Time.deltaTime;
            if(animTime < -animTimeMax)
            {
                animTime = animTimeMax;
            }
        } else
        {
            animTime = 0;
        }

        if(gs.)
        
        Camera.main.transform.position = new Vector3(transform.position.x,transform.position.y,Camera.main.transform.position.z);
    }
}