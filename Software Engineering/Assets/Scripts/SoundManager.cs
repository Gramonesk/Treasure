using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audio;

    public void PlayAudio()
    {
        audio.Play();
    }
}
