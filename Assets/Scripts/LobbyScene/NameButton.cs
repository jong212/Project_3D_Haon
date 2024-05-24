using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NameButton : MonoBehaviour
{
    public Button nameButton;
    public TextMeshProUGUI nameText;
    private bool isName = true;


    private void Awake()
    {
        nameText.text = PlayInfo.userName;
    }
    private void OnEnable()
    {
        if(nameButton != null && nameText != null)
        {
            nameButton.onClick.AddListener(ToggleText);            
        }
    }
    private void OnDisable()
    {
        if (nameButton != null && nameText != null)
        {
            nameButton.onClick.RemoveListener(ToggleText);
        }
    }

    void ToggleText()
    {
        if(isName)
        {
            nameText.text = PlayInfo.userID;
        }
        else
        {
            nameText.text = PlayInfo.userName;
        }
        isName = !isName;
    }
}
