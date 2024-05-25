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

    public string _playerName;
    public string _userName;

    private void Start()
    {
        // Load player data from UserData on start
        LoadPlayerDataFromUserData();
    }

    private void OnEnable()
    {
        // Assign listener to the button click event
        if (nameButton != null)
        {
            nameButton.onClick.AddListener(ToggleText);
        }
        else
        {
            Debug.LogWarning("NameButton is not assigned.");
        }

        // Set initial name text
        if (nameText != null)
        {
            UpdateNameText();
        }
        else
        {
            Debug.LogWarning("NameText is not assigned.");
        }
    }

    private void OnDisable()
    {
        // Remove listener from the button click event
        if (nameButton != null)
        {
            nameButton.onClick.RemoveListener(ToggleText);
        }
    }

    private void ToggleText()
    {
        // Toggle between player name and player ID
        isName = !isName;
        UpdateNameText();
    }

    private void UpdateNameText()
    {
        // Update the text based on the current state
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
        // Load data from UserData instance
        if (UserData.Instance != null)
        {
            if (UserData.Instance.Character != null)
            {
                _playerName = UserData.Instance.Character.PlayerName;
                _userName = UserData.Instance.Character.PlayerId; // 수정: PlayerId는 Players 테이블의 Username
                UpdateNameText(); 
            }
            else
            {
                Debug.LogError("Character data is not loaded in UserData.");
            }
        }
        else
        {
            Debug.LogError("UserData is not loaded or user is not logged in.");
        }
    }
}
