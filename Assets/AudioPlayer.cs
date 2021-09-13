using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    public static AudioPlayer instance;
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public AudioClip morningAmb;
    public AudioClip nightAmb;
    public AudioClip morningSfx;
    public AudioClip nightSfx;

    public List<AudioClip> musicClips;
    public AudioSource morning;
    public AudioSource night;
    public AudioSource music;
    public AudioSource sfx;

    public void Set(AudioClip clip, AudioSource src)
    {
        src.clip = clip;
    }

    public void Play(AudioClip clip, AudioSource src)
    {
        src.PlayOneShot(clip);
    }

    public void Morning()
    {
        Play(morningAmb, morning);
        Play(morningSfx, sfx);
    }

    public void Night()
    {
        Play(nightAmb, night);
        Play(nightSfx, sfx);
    }

    public void RndClip()
    {
        Play(musicClips[Random.Range(0, musicClips.Count - 1)], music);
    }


}
