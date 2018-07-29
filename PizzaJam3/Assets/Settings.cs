using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public void retry()
    {
        Application.LoadLevel("Main");
    }

    public void quit()
    {
        Application.Quit();
    }
}
