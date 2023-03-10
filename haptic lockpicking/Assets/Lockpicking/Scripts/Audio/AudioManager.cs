using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Sound[] sounds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            throw new Exception();
        }
        else
        {
            Instance = this;
        }

        foreach (Sound s in sounds)
        {
            s.SetSource(gameObject.AddComponent<AudioSource>());
            s.GetSource().clip = s.GetClip();
            s.GetSource().volume = s.GetVolume();
            s.GetSource().pitch = s.GetPitch();
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        if (s == null) throw new Exception();
        s.GetSource().Play();
    }
}
