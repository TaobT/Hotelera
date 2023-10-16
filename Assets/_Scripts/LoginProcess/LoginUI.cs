using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombreInput;
    [SerializeField] private TextMeshProUGUI apellidosInput;
    [SerializeField] private TextMeshProUGUI emailInput;
    [SerializeField] private TextMeshProUGUI passwordInput;
    [SerializeField] private TextMeshProUGUI passwordConfirmInput;
    [SerializeField] private Button registrarBtn;

    private void Start()
    {
        registrarBtn.onClick.AddListener(OnRegistrarBtnClick);
    }

    private void OnRegistrarBtnClick()
    {
        
    }

    private void ValidateInputs()
    {
        //Verificar que todos los inputs tengan texto y que el password y el password confirm sean iguales
        //Ademas de que el correo tenga el formato correcto
        //Y el nombre o apellidos no contengan numeros o caracteres especiales
        //Si todo esta bien, llamar al metodo RegisterUser de DatabaseManager
        if(string.IsNullOrEmpty(nombreInput.text))
        {
            Debug.LogError("El nombre no puede estar vacio");
            return;
        }

        if(string.IsNullOrEmpty(apellidosInput.text))
        {
            Debug.LogError("Los apellidos no pueden estar vacios");
            return;
        }

        if(string.IsNullOrEmpty(emailInput.text))
        {
            Debug.LogError("El correo no puede estar vacio");
            return;
        }

        if(string.IsNullOrEmpty(passwordInput.text))
        {
            Debug.LogError("El password no puede estar vacio");
            return;
        }

        if(string.IsNullOrEmpty(passwordConfirmInput.text))
        {
            Debug.LogError("El password confirm no puede estar vacio");
            return;
        }

        if(passwordInput.text != passwordConfirmInput.text)
        {
            Debug.LogError("El password y el password confirm no coinciden");
            return;
        }

        if(passwordInput.text.Length < 6)
        {
            Debug.LogError("El password debe tener al menos 6 caracteres");
            return;
        }

        if(!emailInput.text.Contains("@") || !emailInput.text.Contains("."))
        {
            Debug.LogError("El correo no tiene el formato correcto");
            return;
        }

        if(!IsNameValid(nombreInput.text))
        {
            Debug.LogError("El nombre no puede contener numeros o caracteres especiales");
            return;
        }

        if(!IsNameValid(apellidosInput.text))
        {
            Debug.LogError("Los apellidos no pueden contener numeros o caracteres especiales");
            return;
        }

        DatabaseManager.Instance.RegisterUser(nombreInput.text, apellidosInput.text, emailInput.text, passwordInput.text);
    }

    private bool IsNameValid(string name)
    {
        foreach(char c in name)
        {
            if(char.IsDigit(c) || char.IsSymbol(c) || char.IsPunctuation(c))
            {
                return false;
            }
        }
        return true;
    }
}
