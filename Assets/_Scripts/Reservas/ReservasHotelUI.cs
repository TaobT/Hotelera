using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReservasHotelUI : MonoBehaviour
{
    [SerializeField] private GameObject reservaUIPf; //Prefab de la reserva
    [SerializeField] private Transform reservaUIListContent;

    void OnEnable()
    {
        //List<> datos = DatabaseManager.GetReservasOfUser();
        
    }

    public void LlenarLista()
    {
        LlenarListaDeReservas();
    }

    private async void LlenarListaDeReservas()
    {
        if (UserInfo.Singleton.IsClient())
        {
            //Si es cliente traer todas sus reservas
            List<ReserveInformation> reservas = await DatabaseManager.Instance.GetReservesOfUser(UserInfo.Singleton.information.id);
            foreach (ReserveInformation reserva in reservas)
            {
                GameObject reservaUI = Instantiate(reservaUIPf, reservaUIListContent);
                reservaUI.GetComponent<ReservaListElement>().SetInformation(reserva);
            }
        }
        else //Si es dueño traer todas las reservas de sus hoteles
        {
            List<ReserveInformation> reservas = await DatabaseManager.Instance.GetReservesOfOwner(UserInfo.Singleton.information.id);
            foreach (ReserveInformation reserva in reservas)
            {
                GameObject reservaUI = Instantiate(reservaUIPf, reservaUIListContent);
                reservaUI.GetComponent<ReservaListElement>().SetInformation(reserva);
            }
        }
    }
}
