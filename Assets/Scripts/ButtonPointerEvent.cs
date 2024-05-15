using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonPointerEvent : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Transform transform;
    public Image image;
    public GameObject select;
    Vector3 defaulttransform;
    public void Start()
    {
        defaulttransform = transform.localScale;
    }
    

    public void OnPointerEnter()
    {
        transform.localScale = defaulttransform*1.2f;
        text.color = Color.black;
    }
    public void OnPointerExit()
    {
        
        transform.localScale = defaulttransform;
        text.color = Color.white;
    }
    public void OnPointerDown()
    {
        Debug.Log("´©¸§");
        transform.localScale = defaulttransform;
        text.color = Color.white;
    }
    public void OnPointerUp()
    {
        Debug.Log("¶À");
    }
    public void OnPointerDownUpgrade()
    {
        image.color = Color.red;
    }
    public void OnPointerUpUpgrade()
    {
        image.color = Color.white;
    }
    public void OnpointerEnterStage()
    {
        transform.localScale = defaulttransform * 1.2f;
        select.SetActive(true);

    }
    public void OnpointerExitStage()
    {
        transform.localScale = defaulttransform;
        select.SetActive(false);

    }
}
