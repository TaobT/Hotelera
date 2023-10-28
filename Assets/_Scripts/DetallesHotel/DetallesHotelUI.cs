using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetallesHotelUI : MonoBehaviour
{
    [SerializeField] private GameObject[] clientElements;
    [SerializeField] private GameObject[] ownerElements;

    [SerializeField] private Button makeReservationBtn;

    [SerializeField] private GameObject commentaryElementUIPf;
    [SerializeField] private Transform commentaryContentUI;

    private void Awake()
    {
        //Diferenciar entre Cliente y Dueño
        //Traer toda la información para mostrarla en los elementos
    }

    private void OnMakeReservationBtnClick()
    {
        //Show payment method ui
    }
    
    private void ShowClientElements()
    {

    }

    private void ShowOwnerElements()
    {

    }
}
