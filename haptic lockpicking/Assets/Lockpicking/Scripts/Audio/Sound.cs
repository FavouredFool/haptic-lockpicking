using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private AudioClip _clip;

    [Range(0.1f, 1f)]
    [SerializeField]
    private float _volume;

    [Range(0, 3f)]
    [SerializeField]
    private float _pitch;

    private AudioSource _source;


    public string GetName()
    {
        return _name;
    }

    public AudioClip GetClip()
    {
        return _clip;
    }

    public float GetVolume()
    {
        return _volume;
    }

    public float GetPitch()
    {
        return _pitch;
    }

    public AudioSource GetSource()
    {
        return _source;
    }

    public void SetSource(AudioSource source)
    {
        _source = source;
    }
}
