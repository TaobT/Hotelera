using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormularioHotelUI : MonoBehaviour
{
    //TODO: Este elemento tambien permite editar o borrar el hotel ademas de registrarlo

    [SerializeField] private Button registerHotelBtn;

    private void Awake()
    {

    }

    private void OnRegisterHotelBtnClick()
    {
        
        if(!ValidateInputs()) return;

        DatabaseManager.Instance.RegisterHotel();    
        
        //Show succes message
        //Regresar a Lista de Hoteles
    }
    
    private bool ValidateInputs()
    {

    }
}
