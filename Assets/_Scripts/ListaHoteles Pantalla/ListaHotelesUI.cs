using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListaHotelesUI : MonoBehaviour
{
    [SerializeField] private GameObject[] clientElements;
    [SerializeField] private GameObject[] ownerElements;

    [SerializeField] private GameObject listaHotelUIPf;
    [SerializeField] private Transform listaHotelUIContent;

    #region Client
    #endregion

    #region Owner
    [SerializeField] private Button createNewHotelBtn;
    #endregion

    private void Awake()
    {
        //Diferenciar entro Cliente y Dueño
        //Traer los hoteles de acuerdo al filtro       
    }

    private void OnCreateNewHotelBtnClick()
    {
        //Mostrar formulario del hotel
    }

    private void ShowClientElements()
    {

    }

    private void ShowOwnerElements()
    {

    }
}
