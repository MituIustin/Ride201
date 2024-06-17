using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Audio Manager 
// Used for sound effects and background music 

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource sfx;

    // Audio clips used 

    public AudioClip bkg;
    public AudioClip punch;
    public AudioClip damage;
    public AudioClip pickup;
    public AudioClip next_lvl;
    public AudioClip jump;
    public AudioClip dash;

    // BKG music player

    private void Start()
    {
        music.clip = bkg;
        music.loop = true;
        music.Play();
    }

    // Play a sound effect method

    public void PlaySFX(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }
}
