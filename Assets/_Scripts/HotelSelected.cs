using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotelSelected : MonoBehaviour
{
    //Esta clase permite seleccionar un hotel para mostrar su informaci�n en la pantalla de informaci�n de hotel
    //O hacer otra operaci�n como borrado o actualizado

    public static HotelSelected Singleton;

    private HotelInformation hotelInformation;
    private bool hasHotelInformation;

    private void Awake()
    {
        Singleton = this;
    }

    public void SetHotelInformation(HotelInformation hotelInformation)
    {
        hasHotelInformation = true;
        this.hotelInformation = hotelInformation;
    }

    public HotelInformation GetHotelInformation()
    {
        return hotelInformation;
    }

    public void CleanHotelInformation()
    {
        hotelInformation = null;
        hasHotelInformation = false;
    }

    public bool HasHotelInformation()
    {
        return hasHotelInformation;
    }
}
