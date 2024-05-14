using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingMenu : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject button1280;
    public GameObject button1920;
    public GameObject buttonfull;


    public Slider masterVolume;
    public Slider bgm;
    public Slider sfx;

    
  

    public void OnClickResolutionButton1()
    {
        button1280.GetComponent<Image>().sprite = sprites[0];
        button1920.GetComponent<Image>().sprite = sprites[1];
        buttonfull.GetComponent<Image>().sprite = sprites[1];
        Screen.SetResolution(1920, 1080, false);
    }
    public void OnClickResolutionButton2()
    {
        button1280.GetComponent<Image>().sprite = sprites[1];
        button1920.GetComponent<Image>().sprite = sprites[0];
        buttonfull.GetComponent<Image>().sprite = sprites[1];
        Screen.SetResolution(1920, 1080, true);
    }

    public void OnClickResolutionButton3()
    {
        button1280.GetComponent<Image>().sprite = sprites[1];
        button1920.GetComponent<Image>().sprite = sprites[1];
        buttonfull.GetComponent<Image>().sprite = sprites[0];
        Screen.SetResolution(1280, 720, false);
    }





}
