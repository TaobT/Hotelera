using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    
    [SerializeField] private UIButton loginBtn;

    private void Awake()
    {
        loginBtn.onClickEvent.AddListener(OnLoginBtnClick);
    }

    private async void OnLoginBtnClick()
    {
        if(!ValidateInputs()) return;

        UserInformation user = await DatabaseManager.Instance.GetUser(emailInput.text.Trim(), passwordInput.text);

        if (user == null) return;

        UserInfo.Singleton.SetInformation(user);

        //Clear inputs
        emailInput.text = "";
        passwordInput.text = "";

        //Send to Lista Hoteles screen
        SceneManager.LoadScene("HoteleraScene");
    }

    private bool ValidateInputs()
    {
        //Validar que los inputs no esten vacios y sean validos
        if(string.IsNullOrEmpty(emailInput.text))
        {
            Debug.LogError("El correo no puede estar vacio");
            return false;
        }

        if(string.IsNullOrEmpty(passwordInput.text))
        {
            Debug.LogError("El password no puede estar vacio");
            return false;
        }

        //Verificar que el correo tenga el formato correcto
        if(!emailInput.text.Contains("@"))
        {
            Debug.LogError("El correo no tiene el formato correcto");
            return false;
        }

        return true;
    }
}
