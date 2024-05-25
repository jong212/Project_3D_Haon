using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Button buttonA;
    public Button buttonB;
    public GameObject panelA;
    public GameObject panelB;

    private Color32 inactiveColor = new Color32(142, 191, 224, 177); // #8EBFE0, alpha 177
    private Color32 activeColor = new Color32(170, 181, 221, 255); // #AAB5DD, alpha 255

    void Start()
    {
        //// 초기 상태 설정
        //ShowPanelA();

        // 버튼 클릭 이벤트 추가
        buttonA.onClick.AddListener(ShowPanelA);
        buttonB.onClick.AddListener(ShowPanelB);
    }

    void ShowPanelA()
    {
        SetPanelState(panelA, buttonA, true);
        SetPanelState(panelB, buttonB, false);
    }

    void ShowPanelB()
    {
        SetPanelState(panelA, buttonA, false);
        SetPanelState(panelB, buttonB, true);
    }

    void SetPanelState(GameObject panel, Button button, bool isActive)
    {
        panel.SetActive(isActive);
        button.GetComponent<Image>().color = isActive ? activeColor : inactiveColor;
    }
}
