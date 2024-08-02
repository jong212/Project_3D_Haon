using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;

public class RegisterLoginManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField loginUsernameField;
    [SerializeField] private TMP_InputField loginPasswordField;
    [SerializeField] private TMP_InputField registerUsernameField;
    [SerializeField] private TMP_InputField registerPasswordField;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private GameObject authPanel;
    [SerializeField] private TextMeshProUGUI loginText;
    private string _loginip;
    private string _dbName;
    private string _uid;
    private string _pwd;
    private string _port;

    [Header("DB Connection Info")]
    private MySqlConnection _dbConnection;

    public static bool isLogin = false;
    public static event Action OnLoginSuccess;

    IEnumerator Start()
    {
        while (string.IsNullOrEmpty(RemoteConfigManager._ip))
        {
            Debug.Log("test");
            yield return null;
        }
        _loginip = RemoteConfigManager._ip;
        _dbName = RemoteConfigManager._dbName;
        _uid = RemoteConfigManager._Uid;
        _pwd = RemoteConfigManager._Pwd;
        _port = RemoteConfigManager._Port;
        Debug.Log(_loginip);

        _dbConnection = new MySqlConnection($"Server={_loginip};Database={_dbName};Uid={_uid};Pwd={_pwd};Port={_port};");
        try
        {
            _dbConnection.Open();
            Debug.Log("Database connection successful");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Database connection failed: {e}");
        }
    }

    public void OnRegisterButtonClicked()
    {
        StartCoroutine(RegisterUser());
    }

    public void OnLoginButtonClicked()
    {
        StartCoroutine(LoginUser());
    }

    public void OnCancelButtonClicked()
    {
        authPanel.SetActive(false);
        loginText.gameObject.SetActive(true);
    }

    IEnumerator RegisterUser()
    {
        if (string.IsNullOrEmpty(registerUsernameField.text))
        {
            ShowFeedback("���̵� �Է����ּ���");
            yield break;
        }

        if (string.IsNullOrEmpty(registerPasswordField.text))
        {
            ShowFeedback("�н����带 �Է����ּ���");
            yield break;
        }

        string query = $"INSERT INTO u_info (Nickname, Password) VALUES ('{registerUsernameField.text}', '{registerPasswordField.text}')";

        using (MySqlCommand cmd = new MySqlCommand(query, _dbConnection))
        {
            try
            {
                cmd.ExecuteNonQuery();
                ShowFeedback("ȸ������ ����");
            }
            catch (Exception e)
            {
                //ShowFeedback("Error: " + e.Message);
                ShowFeedback("�̹� ���� �� ���̵��Դϴ�.");
            }
        }
    }

    IEnumerator LoginUser()
    {
        if (string.IsNullOrEmpty(loginUsernameField.text))
        {
            ShowFeedback("���̵� �Է����ּ���");
            yield break;
        }

        if (string.IsNullOrEmpty(loginPasswordField.text))
        {
            ShowFeedback("�н����带 �Է����ּ���");
            yield break;
        }

        string query = $"SELECT Password FROM u_info WHERE Nickname = '{loginUsernameField.text}'";

        using (MySqlCommand cmd = new MySqlCommand(query, _dbConnection))
        {
            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string dbPassword = reader.GetString("Password");
                    reader.Close();

                    if (dbPassword == loginPasswordField.text)
                    {
                        ShowFeedback("�α��� ����");
                        isLogin = true;
                        //yield return new WaitForSeconds(1);
                        loginText.gameObject.SetActive(false);
                        authPanel.SetActive(false);
                        OnLoginSuccess?.Invoke();
                    }
                    else
                    {
                        ShowFeedback("��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
                    }
                }
                else
                {
                    ShowFeedback("���̵� �������� �ʽ��ϴ�.");
                }
            }
            catch (Exception e)
            {
                ShowFeedback("Error: " + e.Message);
            }
        }
    }

    void ShowFeedback(string message)
    {
        feedbackText.text = message;
        feedbackText.DOFade(1, 1).OnComplete(() => feedbackText.DOFade(0, 2));
    }
}
