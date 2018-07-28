using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFadeOut : MonoBehaviour
{
    public float dir = 2.0f;
    public string text;

	// Use this for initialization
	void Start ()
    {
        GetComponent<TextMesh>().text = text;
	}
	
	// Update is called once per frame
	void Update ()
    {
        dir -= Time.deltaTime;
        transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + Time.deltaTime);

        if(dir < 0)
        {
            Destroy(this.gameObject);
        }
		
	}
}
