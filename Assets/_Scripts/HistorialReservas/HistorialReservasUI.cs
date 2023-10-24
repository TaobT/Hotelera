using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistorialReservasUI : MonoBehaviour
{
    [SerializeField] private GameObject[] clientElements;
    [SerializeField] private GameObject[] ownerElements;

    [SerializeField] private GameObject historialReservaElementUI;
    [SerializeField] private Transform historialContentUI;


    private void Awake()
    {
        //Diferenciar entre Cliente y Dueño con ClientInfo
        //Traer toda la información para mostrarla en los elementos
    }
    
    private void ShowClientElements()
    {

    }

    private void ShowOwnerElements()
    {

    }
}
