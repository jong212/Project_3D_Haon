//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using UnityEngine.UI;

//public class ButtonPointerEvent : MonoBehaviour
//{
//    public TextMeshProUGUI text;
//    public Transform transform;
//    public Image image;
//    public GameObject select;
//    Vector3 defaulttransform;
//    public void Start()
//    {
//        defaulttransform = transform.localScale;
//    }


//    public void OnPointerEnter()
//    {
//        transform.localScale = defaulttransform*1.2f;
//        text.color = Color.black;
//    }
//    public void OnPointerExit()
//    {

//        transform.localScale = defaulttransform;
//        text.color = Color.white;
//    }
//    public void OnPointerDown()
//    {
//        Debug.Log("누름");
//        transform.localScale = defaulttransform;
//        text.color = Color.white;
//    }
//    public void OnPointerUp()
//    {
//        Debug.Log("뗌");
//    }
//    public void OnPointerDownUpgrade()
//    {
//        image.color = Color.red;
//    }
//    public void OnPointerUpUpgrade()
//    {
//        image.color = Color.white;
//    }
//    public void OnpointerEnterStage()
//    {
//        transform.localScale = defaulttransform * 1.2f;
//        select.SetActive(true);

//    }
//    public void OnpointerExitStage()
//    {
//        transform.localScale = defaulttransform;
//        select.SetActive(false);

//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonPointerEvent : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image image;
    public GameObject select;
    private Vector3 defaultScale; // 초기 localScale 값을 저장하는 private 변수

    void Start()
    {
        defaultScale = transform.localScale; // 초기 localScale 값을 저장
    }

    public void OnPointerEnter()
    {
        ChangeScaleAndColor(1.2f, Color.black); // 크기와 색상을 변경하는 메서드 호출
    }

    public void OnPointerExit()
    {
        ChangeScaleAndColor(1.0f, Color.white); // 원래 크기와 색상으로 되돌리는 메서드 호출
    }

    public void OnPointerDown()
    {
        Debug.Log("누름"); // 클릭 시 로그 출력
        ResetScaleAndColor(); // 크기와 색상을 초기 상태로 되돌리는 메서드 호출
    }

    public void OnPointerUp()
    {
        Debug.Log("뗌"); // 클릭 해제 시 로그 출력
    }

    public void OnPointerDownUpgrade()
    {
        ChangeImageColor(Color.red); // 이미지 색상을 빨간색으로 변경하는 메서드 호출
    }

    public void OnPointerUpUpgrade()
    {
        ChangeImageColor(Color.white); // 이미지 색상을 흰색으로 변경하는 메서드 호출
    }

    public void OnPointerEnterStage()
    {
        ChangeScale(1.2f); // 크기를 변경하는 메서드 호출
        select.SetActive(true); // 선택 오브젝트 활성화
    }

    public void OnPointerExitStage()
    {
        ChangeScale(1.0f); // 원래 크기로 되돌리는 메서드 호출
        select.SetActive(false); // 선택 오브젝트 비활성화
    }

    // 크기와 텍스트 색상을 변경하는 메서드 추가
    private void ChangeScaleAndColor(float scaleMultiplier, Color textColor)
    {
        transform.localScale = defaultScale * scaleMultiplier; // 크기 변경
        text.color = textColor; // 텍스트 색상 변경
    }

    // 크기와 텍스트 색상을 초기 상태로 되돌리는 메서드 추가
    private void ResetScaleAndColor()
    {
        transform.localScale = defaultScale; // 크기 초기화
        text.color = Color.white; // 텍스트 색상 초기화
    }

    // 크기만 변경하는 메서드 추가
    private void ChangeScale(float scaleMultiplier)
    {
        transform.localScale = defaultScale * scaleMultiplier; // 크기 변경
    }

    // 이미지 색상만 변경하는 메서드 추가
    private void ChangeImageColor(Color color)
    {
        image.color = color; // 이미지 색상 변경
    }
}
