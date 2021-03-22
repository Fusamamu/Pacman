using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager sharedInstance { get; set; }

    public AudioSource MUSIC_SOUNDSRC;
    public AudioSource EFFECT_SOUNDSRC;

    public AudioClip BGM_clip;
    public AudioClip eating_sound_clip;
    public AudioClip dead_sound_clip;

    public bool updatingSound = false;

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        MUSIC_SOUNDSRC.Play();
    }

    public void OnEating()
    {
        EFFECT_SOUNDSRC.PlayOneShot(eating_sound_clip);
    }

    public void OnDied()
    {
        MUSIC_SOUNDSRC.PlayOneShot(dead_sound_clip);
    }

    public void OnFinishUpdate()
    {
        updatingSound = false;
    }
}
