using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInFadeOutSceneManager : Singleton<FadeInFadeOutSceneManager>
{
    public CanvasGroup fadeImg;
    private float fadeDuration = 2f;

    public GameObject Loading;
    public TextMeshProUGUI loadingText;

    private bool isLoading = false;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ChangeScene(string sceneName)
    {
        if (!isLoading)
        {
            isLoading = true;
            fadeImg.DOFade(1, fadeDuration)
                .OnStart(() =>
                {
                    fadeImg.blocksRaycasts = true;
                })
                .OnComplete(() =>
                {
                    StartCoroutine(LoadSceneAsync(sceneName));
                });
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        Loading.SetActive(true);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f) * 100f;
            loadingText.text = progress.ToString("0") + "%";

            if (async.progress >= 0.9f)
            {
                async.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadeImg.DOFade(0, fadeDuration)
            .OnStart(() => { Loading.SetActive(false); })
            .OnComplete(() =>
            {
                fadeImg.blocksRaycasts = false;
                isLoading = false;
            });
    }



}
