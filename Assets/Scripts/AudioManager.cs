using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource sfx;

    public AudioClip bkg;
    public AudioClip punch;
    public AudioClip damage;
    public AudioClip pickup;
    public AudioClip next_lvl;
    public AudioClip jump;
    public AudioClip dash;

    private void Start()
    {
        music.clip = bkg;
        music.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }
}
