using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{

    public static AudioSource audioSource;

    private static Dictionary<string, AudioClip> sounds;

    public static void PreLoad()
    {
        sounds = new Dictionary<string, AudioClip>();
        foreach(var v in Resources.LoadAll<AudioClip>("Sounds/"))
        {
            sounds[v.name] = v;
        } 
    }

    public static void PlaySound(string noise)
    {
        audioSource.PlayOneShot(sounds[noise]);
    }

    

}
