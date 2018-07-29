using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject a1;

    public GameObject a2;

    public GameObject a3;

    public void Go(int i)
    {
        switch (i)
        {
            case 0:
                a1.SetActive(false);
                a2.SetActive(true);
                break;
            case 1:
                a2.SetActive(false);
                a3.SetActive(true);
                break;
            case 2:
                Application.LoadLevel("Main");
                break;
        }
    }

}
