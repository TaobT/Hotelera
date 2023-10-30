using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReservaListElement : MonoBehaviour
{
    [SerializeField] private GameObject[] clientElements;
    [SerializeField] private GameObject[] ownerElements;

    [SerializeField] private TextMeshProUGUI hotelName;
    [SerializeField] private TextMeshProUGUI fechas; //Con el formato: "DD/MM/AAAA - DD/MM/AAAA"
    [SerializeField] private Button cancelarReserva;
    [SerializeField] private Button aceptarReserva; //Solo para el dueño del hotel

    [SerializeField] private GameObject aceptadaIndicador;
    [SerializeField] private GameObject pendienteIndicador;
    [SerializeField] private GameObject canceladaIndicador;


    private ReserveInformation information;

    private void Awake()
    {
        cancelarReserva.onClick.AddListener(OnCancelarBtnClick);
        aceptarReserva.onClick.AddListener(OnAceptarBtnClick);

        //Diferenciar entre Cliente y Dueño
        if (UserInfo.Singleton.IsClient())
        {
            ShowClientElements();
        }
        else
        {
            ShowOwnerElements();
        }
    }
    public async void SetInformation(ReserveInformation information)
    {
        this.information = information;
        HotelInformation hotel = await DatabaseManager.Instance.GetHotel(information.id_hotel);
        hotelName.text = hotel.nombre;
        fechas.text = information.fecha_entrada.ToString("dd/MM/yyyy") + " - " + information.fecha_salida.ToString("dd/MM/yyyy");
        AjustarEstado();
    }

    private void AjustarEstado()
    {
        aceptadaIndicador.SetActive(information.estado == "Aceptada");
        pendienteIndicador.SetActive(information.estado == "Pendiente");
        canceladaIndicador.SetActive(information.estado == "Cancelada");
        cancelarReserva.gameObject.SetActive(information.estado == "Pendiente");
        aceptarReserva.gameObject.SetActive(information.estado == "Pendiente");
    }

    private void OnAceptarBtnClick()
    {
        //Activar indicador de aceptada y desactivar el resto
        //Tambien desactivar todos los botones
        //Cambiar el estado de la reserva en la base de datos
        pendienteIndicador.gameObject.SetActive(false);
        canceladaIndicador.gameObject.SetActive(false);
        aceptadaIndicador.gameObject.SetActive(true);

        cancelarReserva.gameObject.SetActive(false);
        aceptarReserva.gameObject.SetActive(false);

        DatabaseManager.Instance.UpdateReserveState(information.id, "Aceptada");
    }

    private void OnCancelarBtnClick()
    {
        //Activar indicador de cancelada y desactivar el resto
        //Tambien desactivar todos los botones
        //Cambiar el estado de la reserva en la base de datos
        pendienteIndicador.gameObject.SetActive(false);
        canceladaIndicador.gameObject.SetActive(true);
        aceptadaIndicador.gameObject.SetActive(false);
        cancelarReserva.gameObject.SetActive(false);
        aceptarReserva.gameObject.SetActive(false);

        DatabaseManager.Instance.UpdateReserveState(information.id, "Cancelada");
    }

    private void ShowClientElements()
    {
        foreach (GameObject element in clientElements)
        {
            element.SetActive(true);
        }
        //Ocultar elementos de dueño
        foreach (GameObject element in ownerElements)
        {
            element.SetActive(false);
        }
    }

    private void ShowOwnerElements()
    {
        foreach (GameObject element in ownerElements)
        {
            element.SetActive(true);
        }
        //Ocultar elementos de cliente
        foreach (GameObject element in clientElements)
        {
            element.SetActive(false);
        }
    }
}
