using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    public AudioClip[] sfxClip;
    public float sfxVolume;
    AudioSource sfxPlayer;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    void Awake()
    {
        if (null == instance)
        {

            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {

            Destroy(this.gameObject);
        }
        Init();
    }
    
    void Init()
    {
        GameObject bgmObject = new GameObject("BGMplayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.loop = true;
        bgmPlayer.clip = bgmClip;

        GameObject sfxObject = new GameObject("SFXplayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false ;
            sfxPlayers[index].volume = sfxVolume;
            

        }



    }
}
