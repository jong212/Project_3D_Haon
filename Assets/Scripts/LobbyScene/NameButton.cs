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

    private void OnEnable()
    {
        if (nameButton != null)
        {
            nameButton.onClick.AddListener(ToggleText);
        }
        else
        {
            Debug.LogWarning("NameButton is not assigned.");
        }

        if (nameText != null)
        {
            UpdateNameText();
        }
        else
        {
            Debug.LogWarning("NameText is not assigned.");
        }

        LoadPlayerDataFromUserData();
    }

    private void OnDisable()
    {
        if (nameButton != null)
        {
            nameButton.onClick.RemoveListener(ToggleText);
        }
    }

    private void ToggleText()
    {
        isName = !isName;
        UpdateNameText();
    }

    private void UpdateNameText()
    {
        if (isName)
        {
            nameText.text = UserData.Instance.Character?.PlayerName ?? "Unknown Player";
        }
        else
        {
            nameText.text = UserData.Instance.Character?.PlayerId ?? "Unknown ID";
        }
    }

    private void LoadPlayerDataFromUserData()
    {
        if (UserData.Instance != null && UserData.Instance.Character != null)
        {
            UpdateNameText(); // Ensure the name text is set correctly on load
        }
        else
        {
            Debug.LogError("Character data is not loaded in UserData.");
        }
    }
}
