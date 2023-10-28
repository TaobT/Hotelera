using Doozy.Runtime.Signals;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotelListElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ubicationText;
    [SerializeField] private TextMeshProUGUI califText;
    [SerializeField] private TextMeshProUGUI precioText;

    [SerializeField] private Button selectHotelBtn;

    private HotelInformation currentHotel;

    private void Awake()
    {
        selectHotelBtn.onClick.AddListener(SelectHotel);
    }

    public void SetHotelInformation(HotelInformation hotelInformation)
    {
        currentHotel = hotelInformation;
        ubicationText.text = currentHotel.ciudad + ", " + currentHotel.pais;
        califText.text = "" + currentHotel.calificacionPromedio;
        precioText.text = "$" + currentHotel.precioHabitacionesAdultos + " MXN Noche";
    }

    public void SelectHotel()
    {
        HotelSelected.Singleton.SetHotelInformation(currentHotel);
        
        //Enviar a Formulario Hotel
        Signal.Send("HoteleraScene", "FormHotelSelected", currentHotel);
    }
}
