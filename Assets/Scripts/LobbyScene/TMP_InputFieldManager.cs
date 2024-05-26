using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMP_InputFieldManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField titleInputField;
    [SerializeField] private TextMeshProUGUI hiddenTitleText;

    private const string DefaultTitle = "파티사냥 가실분";

    private void OnEnable()
    {
        // Add listener to TMP_InputField to detect changes
        titleInputField.onValueChanged.AddListener(OnTitleChanged);
        // Initialize the hidden title text with the default or current input field value
        UpdateHiddenTitleText(titleInputField.text);
    }

    private void OnDisable()
    {
        // Remove listener when disabled to avoid memory leaks
        titleInputField.onValueChanged.RemoveListener(OnTitleChanged);
    }

    private void OnTitleChanged(string newTitle)
    {
        UpdateHiddenTitleText(newTitle);
    }

    private void UpdateHiddenTitleText(string newTitle)
    {
        if (hiddenTitleText != null)
        {
            hiddenTitleText.text = string.IsNullOrEmpty(newTitle) ? DefaultTitle : newTitle;
        }
    }

    public string GetHiddenTitleText()
    {
        return hiddenTitleText != null ? hiddenTitleText.text : DefaultTitle;
    }
}
