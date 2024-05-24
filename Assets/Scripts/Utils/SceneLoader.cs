using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] private TextMeshProUGUI loadingText;

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // 로딩 화면 페이드인
        loadingScreen.SetActive(true);
        loadingScreenCanvasGroup.alpha = 0;
        loadingScreenCanvasGroup.DOFade(1, 1f);

        // 텍스트 업데이트 코루틴 시작
        Coroutine loadingTextCoroutine = StartCoroutine(UpdateLoadingText());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        // 텍스트 업데이트 코루틴 종료
        StopCoroutine(loadingTextCoroutine);

        // 로딩 텍스트 페이드아웃
        loadingText.DOFade(0, 1f);

        // 로딩 화면 페이드아웃
        yield return loadingScreenCanvasGroup.DOFade(0, 1f).WaitForCompletion();
        loadingScreen.SetActive(false);
    }

    private IEnumerator UpdateLoadingText()
    {
        while (true)
        {
            loadingText.text = "Loading.";
            yield return new WaitForSeconds(0.3f);
            loadingText.text = "Loading..";
            yield return new WaitForSeconds(0.3f);
            loadingText.text = "Loading...";
            yield return new WaitForSeconds(0.3f);
        }
    }

}
