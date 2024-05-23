using UnityEngine;

public class BgmManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip firstBgm;
    [SerializeField] private AudioClip secondBgm;
    [SerializeField] private float gapVolume = 0.005f;
    public bool BgmSet = false;

    private bool oneShot = false;

    private void Start()
    {
        SetFirstBGM();
    }

    private void Update()
    {
        if (BgmSet && !oneShot) { SetSecondBGM(); }
    }

    void SetFirstBGM()
    {
        source.clip = firstBgm;
        source.Play();
    }

    void SetSecondBGM()
    {
        source.volume -= gapVolume;
        if (source.volume == 0)
        {
            source.clip = secondBgm;
            source.volume = 1;
            source.Play();
            oneShot = true;
        }
    }
}
