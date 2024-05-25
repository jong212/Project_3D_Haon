using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingUI;
    [SerializeField] private Button settingActive;
    [SerializeField] private Button settingInActive;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Button resolutionHD;
    [SerializeField] private Button resolutionFHD;
    [SerializeField] private Button resolutionFullScreen;

    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider bgm;
    [SerializeField] private Slider sfx;

    [SerializeField] private AudioMixer audioMixer;

    private void OnEnable()
    {
        settingActive.onClick.AddListener(ShowSettingUI);
        settingInActive.onClick.AddListener(HideSettingUI);

        // 버튼 클릭 이벤트에 리스너 추가
        resolutionHD.onClick.AddListener(OnClickResolutionHD);
        resolutionFHD.onClick.AddListener(OnClickResolutionFHD);
        resolutionFullScreen.onClick.AddListener(OnClickResolutionFullScreen);

        // 슬라이더 값 변경 이벤트에 리스너 추가
        masterVolume.onValueChanged.AddListener(SetMasterVolume);
        bgm.onValueChanged.AddListener(SetBGMVolume);
        sfx.onValueChanged.AddListener(SetSFXVolume);

    }

    private void OnDisable()
    {
        settingActive.onClick.RemoveListener(ShowSettingUI);
        settingInActive.onClick.RemoveListener(HideSettingUI);

        // 버튼 클릭 이벤트에서 리스너 제거
        resolutionHD.onClick.RemoveListener(OnClickResolutionHD);
        resolutionFHD.onClick.RemoveListener(OnClickResolutionFHD);
        resolutionFullScreen.onClick.RemoveListener(OnClickResolutionFullScreen);

        // 슬라이더 값 변경 이벤트에서 리스너 제거
        masterVolume.onValueChanged.RemoveListener(SetMasterVolume);
        bgm.onValueChanged.RemoveListener(SetBGMVolume);
        sfx.onValueChanged.RemoveListener(SetSFXVolume);

    }

    public void OnClickResolutionHD()
    {
        resolutionHD.GetComponent<Image>().sprite = sprites[0];
        resolutionFHD.GetComponent<Image>().sprite = sprites[1];
        resolutionFullScreen.GetComponent<Image>().sprite = sprites[1];
        Screen.SetResolution(1280, 720, false);

    }
    public void OnClickResolutionFHD()
    {
        resolutionHD.GetComponent<Image>().sprite = sprites[1];
        resolutionFHD.GetComponent<Image>().sprite = sprites[0];
        resolutionFullScreen.GetComponent<Image>().sprite = sprites[1];
        Screen.SetResolution(1920, 1080, false);
    }

    public void OnClickResolutionFullScreen()
    {
        resolutionHD.GetComponent<Image>().sprite = sprites[1];
        resolutionFHD.GetComponent<Image>().sprite = sprites[1];
        resolutionFullScreen.GetComponent<Image>().sprite = sprites[0];
        Screen.SetResolution(1920, 1080, true);
    }

    private void ShowSettingUI()
    {
        settingUI.SetActive(true); // Setting UI 활성화
    }

    private void HideSettingUI()
    {
        settingUI.SetActive(false); // Setting UI 비활성화
    }

    private void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20); // 슬라이더 값을 로그 변환하여 설정
    }

    private void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20); // 슬라이더 값을 로그 변환하여 설정
    }

    private void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20); // 슬라이더 값을 로그 변환하여 설정
    }

}
