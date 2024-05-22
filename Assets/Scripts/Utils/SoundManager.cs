using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip bgmClip;
    public AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource bgmPlayer;
    //public int bgmchannels;
    //AudioSource[] bgmPlayers;


    public AudioClip[] sfxClips;
    public float sfxVolume;
    AudioSource sfxPlayer;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;
    public enum Bgm
    {
        bgm1,
        bgm2, 
        bgm3, 
        bgm4
    }
    public enum Sfx
    {
        sfx1,
        sfx2,
        sfx3,
        sfx4,
        sfx5,
        sfx6,
        sfx7,
        sfx8,
        sfx9,
        sfx10

    }
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
        //bgmPlayers = new AudioSource[bgmchannels];

        //for (int index = 0; index < bgmPlayers.Length; index++)
        //{
        //    bgmPlayers[index] = bgmObject.AddComponent<AudioSource>();
        //    bgmPlayers[index].playOnAwake = false;
        //    bgmPlayers[index].loop = true;
        //    bgmPlayers[index].volume =bgmVolume;
        //}


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

    //public void PlayBgm(bool isPlay)
    //{
    //    if (isPlay)
    //    {
    //        bgmPlayer.Play();
    //    }
    //    else
    //    {
    //        bgmPlayer.Stop();
    //    }
    //}
    public void PlayBgm(Bgm bgm,bool isplaying)
    {
        bgmPlayer.clip = bgmClips[(int)bgm]; 
        if(isplaying) bgmPlayer.Play();
        else bgmPlayer.Stop();
        
    }
    //SoundManager.instance.PlayBgm(SoundManager.Bgm.num,true);
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0;index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index +channelIndex) %sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying) continue;


            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }       
    }
    //SoundManager.instance.PlaySfx(SoundManager.Sfx.num);
}
