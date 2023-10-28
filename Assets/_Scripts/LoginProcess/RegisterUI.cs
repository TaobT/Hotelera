using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nombreInput;
    [SerializeField] private TMP_InputField apellidosInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField passwordConfirmInput;
    [SerializeField] private UIToggle clientToggle;
    [SerializeField] private Button registrarBtn;

    private void Start()
    {
        registrarBtn.onClick.AddListener(OnRegistrarBtnClick);
    }

    private void OnRegistrarBtnClick()
    {
        if(!ValidateInputs()) return;

        string userType = clientToggle.isOn ? "client" : "owner";
        DatabaseManager.Instance.RegisterUser(nombreInput.text, apellidosInput.text, emailInput.text, passwordInput.text, userType);   

        //Clean inputs
        nombreInput.text = "";
        apellidosInput.text = "";
        emailInput.text = "";
        passwordInput.text = "";
        passwordConfirmInput.text = "";

        //Return to Login Scene?
        Signal.Send("LoginRegister", "RegisterDone");
    }

    private bool ValidateInputs()
    {
        //Verificar que todos los inputs tengan texto y que el password y el password confirm sean iguales
        //Ademas de que el correo tenga el formato correcto
        //Y el nombre o apellidos no contengan numeros o caracteres especiales
        //Si todo esta bien, llamar al metodo RegisterUser de DatabaseManager
        
        //TODO: Mostrar mensajes con los errores de aqui
        if(string.IsNullOrEmpty(nombreInput.text))
        {
            Debug.LogError("El nombre no puede estar vacio");
            return false;
        }

        if(string.IsNullOrEmpty(apellidosInput.text))
        {
            Debug.LogError("Los apellidos no pueden estar vacios");
            return false;
        }

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

        if(string.IsNullOrEmpty(passwordConfirmInput.text))
        {
            Debug.LogError("El password confirm no puede estar vacio");
            return false;
        }

        if(passwordInput.text != passwordConfirmInput.text)
        {
            Debug.LogError("El password y el password confirm no coinciden");
            return false;
        }

        if(passwordInput.text.Length < 6)
        {
            Debug.LogError("El password debe tener al menos 6 caracteres");
            return false;
        }

        if(!emailInput.text.Contains("@") || !emailInput.text.Contains("."))
        {
            Debug.LogError("El correo no tiene el formato correcto");
            return false;
        }

        if(!IsNameValid(nombreInput.text))
        {
            Debug.LogError("El nombre no puede contener numeros o caracteres especiales");
            return false;
        }

        if(!IsNameValid(apellidosInput.text))
        {
            Debug.LogError("Los apellidos no pueden contener numeros o caracteres especiales");
            return false;
        }

        return true;
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
