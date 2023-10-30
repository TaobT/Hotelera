using Doozy.Runtime.Signals;
using System.Collections;
using System.Collections.Generic;
using UI.Dates;
using UnityEngine;
using UnityEngine.UI;

public class FormularioReservaUI : MonoBehaviour
{
    [SerializeField] private DatePicker_DateRange dateRange;
    [SerializeField] private Button saveReserveBtn;

    private void Awake()
    {
        saveReserveBtn.onClick.AddListener(GuardarReserva);
    }

    private void GuardarReserva()
    {
        DatabaseManager.Instance.RegisterReserve(HotelSelected.Singleton.GetHotelInformation().id, UserInfo.Singleton.information.id, dateRange.FromDate.Date, dateRange.ToDate.Date);
        Signal.Send("HoteleraScene", "RegresarAListaHotel");
    }
}
