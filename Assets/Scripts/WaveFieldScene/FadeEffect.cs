using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float delayTime = 0.007f;
    //[SerializeField] private bool changeScene = false;

    private float fadeStartTime = 0.0f;
    
    private void Update()
    {
        Fade();
    }

    void Fade()
    {
        Color color = image.color;
        color.a = 1.0f;

        fadeStartTime += delayTime;
        color.a = Mathf.Clamp01(0 + fadeStartTime);
        Debug.Log($"{color.a}");
        image.color = color;

        if (color.a == 1)
        {
            SceneChange();
        }
    }

    void SceneChange()
    {
        SceneManager.LoadScene("Boss Field_Junhu");
    }
}
