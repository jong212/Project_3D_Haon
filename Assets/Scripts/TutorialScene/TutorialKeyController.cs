using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class TutorialKeyController : MonoBehaviour
{
    public Image AKey;       // A 키 이미지
    public Image SKey;       // S 키 이미지
    public Image DKey;       // D 키 이미지
    public Image WKey;       // W 키 이미지
    public Image ShiftKey;   // Shift 키 이미지
    public Image MouseLeft;  // 마우스 왼쪽 버튼 이미지
    public Image Key1;       // 1번 키 이미지
    public Image Key2;       // 2번 키 이미지

    private Dictionary<KeyCode, Image> keyImageMapping; // 키와 이미지 매핑

    void Start()
    {
        // 키와 이미지 매핑 초기화
        keyImageMapping = new Dictionary<KeyCode, Image>
        {
            { KeyCode.A, AKey },
            { KeyCode.S, SKey },
            { KeyCode.D, DKey },
            { KeyCode.W, WKey },
            { KeyCode.LeftShift, ShiftKey },
            { KeyCode.Alpha1, Key1 },
            { KeyCode.Alpha2, Key2 }
        };
    }

    void Update()
    {
        // 각 키에 대해 입력 상태 확인 및 색상 변경
        foreach (var keyImagePair in keyImageMapping)
        {
            UpdateKeyColor(keyImagePair.Key, keyImagePair.Value);
        }

        // 마우스 왼쪽 버튼 입력 상태 확인 및 색상 변경
        UpdateMouseColor(0, MouseLeft);
    }

    // 키 입력 상태에 따라 이미지 색상 변경
    private void UpdateKeyColor(KeyCode key, Image image)
    {
        if (Input.GetKeyDown(key)) image.color = Color.red;
        if (Input.GetKeyUp(key)) image.color = Color.white;
    }

    // 마우스 입력 상태에 따라 이미지 색상 변경
    private void UpdateMouseColor(int button, Image image)
    {
        if (Input.GetMouseButtonDown(button)) image.color = Color.red;
        if (Input.GetMouseButtonUp(button)) image.color = Color.white;
    }
}
