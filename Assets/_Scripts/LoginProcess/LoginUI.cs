using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI emailInput;
    [SerializeField] private TextMeshProUGUI passwordInput;
    
    [SerializeField] private Button loginBtn;

    private void Awake()
    {
        loginBtn.onClick.AddListener()
    }

    private void OnLoginBtnClick()
    {
        if(!ValidateInputs()) return;

        UserInfo.Singleton.LogUser(DatabaseManager.Instance.GetUser());

        //Send to Lista Hoteles screen
    }

    private void ValidateInputs()
    {
        //SAME AS RegisterUI.cs

        //Verify correct password and email
    }
}
