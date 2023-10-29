using Doozy.Runtime.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmBorradoHotelUI : MonoBehaviour
{
    public void BorrarHotelSeleccionado()
    {
        DatabaseManager.Instance.DeleteHotel(HotelSelected.Singleton.GetHotelInformation().id);
        Signal.Send("HoteleraScene", "RegresarAListaHotel");
    }
}
