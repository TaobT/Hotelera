using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReservasHotelUI : MonoBehaviour
{
    private GameObject reservaUIPf; //Prefab de la reserva
    [SerializedField] private Transform reservaUIListContent;

    void OnEnable()
    {
        //List<> datos = DatabaseManager.GetReservasOfUser();

    }

}
