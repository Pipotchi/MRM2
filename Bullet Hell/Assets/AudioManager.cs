using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Play(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.pitch = pitch;
        s.source.Play();
        
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();

    }

    public bool FindSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.isPlaying;
    }

    public void FadeOut(string name, float volume)
    {
            for(int i = 0; i < 15; i++)
            {
                StartCoroutine(DropVolume(i, name, volume));
                //Debug.Log("lowersound");
            }

    }
    public void FadeIn(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = 0;
        s.source.Play();
        for (int i = 0; i < 15; i++)
        {
            StartCoroutine(RaiseVolume(i, name, volume));
            //Debug.Log("insound");
        }

    }
    public void FadeUp(string name, float volume)
    {
        for (int i = 0; i < 15; i++)
        {
            StartCoroutine(RaiseVolume(i, name, volume));
            //Debug.Log("upsound");
        }

    }

    IEnumerator DropVolume(float time, string name, float volume)
    {
        yield return new WaitForSeconds(0.3f *time);

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source.isPlaying)
        {
            s.source.volume -= 0.03f;
            if(s.source.volume < volume)
            {
                s.source.volume = volume;
            }
        }
    }
    IEnumerator RaiseVolume(float time, string name, float volume)
    {
        yield return new WaitForSeconds(0.3f * time);

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source.isPlaying)
        {
            s.source.volume += 0.03f;
            if (s.source.volume > volume)
            {
                s.source.volume = volume;
            }
        }
    }

}
