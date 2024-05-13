using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingMenu : MonoBehaviour
{
    FullScreenMode screenMode;
    public void OnClickLowResolution()
    {
        Screen.SetResolution(1280, 720, false);
    }
    public void OnClickHighResolution()
    {
        Screen.SetResolution(1920,1080,false);
    }
    public void OnClickFullScreenResolution()
    {
        Screen.SetResolution(1920, 1080, true);
    }







}
