using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance { get; private set; }

    [SerializeField]
    RenderTexture _renderTexture;

    [SerializeField]
    VideoClip [] _videoClips;

    Dictionary<int, VideoPlayer> _videoPlayerDict = new();


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            throw new System.Exception();
        }
        else
        {
            Instance = this;
        }

        for (int i = 0; i < _videoClips.Length; i++)
        {
            if (_videoClips[i] == null)
            {
                continue;
            }

            VideoPlayer player = gameObject.AddComponent<VideoPlayer>();
            player.playOnAwake = false;
            player.isLooping = true;
            player.targetTexture = _renderTexture;
            player.clip = _videoClips[i];

            _videoPlayerDict.Add(i, player);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayVideo(0);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayVideo(1);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayVideo(2);
        }
    }

    public void PlayVideo(int sectionNr)
    {
        StopAllVideos();
        _videoPlayerDict[sectionNr].Play();
    }

    public void StopAllVideos()
    {
        foreach(VideoPlayer player in _videoPlayerDict.Values)
        {
            player.Stop();
        }
    }

    public bool SectionHasVideo(int sectionNr)
    {
        return _videoClips[sectionNr] != null;
    }
}


