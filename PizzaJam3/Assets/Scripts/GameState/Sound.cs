using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource NoiseSource;

    bool NoisePlay;
    bool ToggleChange;

    public static PlaySound(string noise)
	{
        // noise does nothing at the moment

        NoiseSource = GetComponent<AudioSource>();

        NoiseSource.Play();

        return null;
	}

    public static PlaySoundtwo(string noise)
    {
        // noise does nothing at the moment
        NoisePlay = true;
        ToggleChange = true;

        
        //Check to see if you just set the toggle to positive
        if (NoisePlay == true && m_ToggleChange == true)
        {
            //Play the audio you attach to the AudioSource component
            NoiseSource.Play();
            //Ensure audio doesn’t play more than once
            m_ToggleChange = false;
        }
        //Check if you just set the toggle to false
        if (NoisePlay == false && m_ToggleChange == true)
        {
            //Stop the audio
            NoiseSource.Stop();
            //Ensure audio doesn’t play more than once
            m_ToggleChange = false;
        }

        return null;
    }

}
