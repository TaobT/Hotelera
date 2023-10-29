using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public static UserInfo Singleton;

    public UserInformation information;

    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetInformation(UserInformation info)
    {
        information = info;
    }

    public bool IsClient()
    {
        return information.user_type == "client";
    }
}

[System.Serializable]
public class UserInformation
{
    public string id;
    public string nombre;
    public string user_type;
    public string correo_electronico;

    public UserInformation(string id, string nombre, string user_type, string correo_electronico)
    {
        this.id = id;
        this.nombre = nombre;
        this.user_type = user_type;
        this.correo_electronico = correo_electronico;
    }
}

[System.Serializable]
public class HotelInformation
{
    public string id;
    public string nombre;
    public string pais;
    public string ciudad;
    public string direccion;
    public string descripcion;
    public float calificacionPromedio;
    public int cantidadHabitaciones;
    public float precioHabitacionesAdultos;
    public float precioHabitacionesMenores;
    public bool servicioALaHabitacion;
    public bool servicioPiscina;
    public bool servicioRestaurante;
    public bool servicioGimnasio;
    public List<string> hotelSpritesUrl;

    public HotelInformation(string id, string nombre, string pais, string ciudad, string direccion, string descripcion, float calificacionPromedio, int cantidadHabitaciones, float precioHabitacionesAdultos, float precioHabitacionesMenores, bool servicioALaHabitacion,
        bool servicioPiscina, bool servicioRestaurante, bool servicioGimnasio)
    {
        this.id = id;
        this.nombre = nombre;
        this.pais = pais;
        this.ciudad = ciudad;
        this.direccion = direccion;
        this.descripcion = descripcion;
        this.calificacionPromedio = calificacionPromedio;
        this.cantidadHabitaciones = cantidadHabitaciones;
        this.precioHabitacionesAdultos = precioHabitacionesAdultos;
        this.precioHabitacionesMenores = precioHabitacionesMenores;
        this.servicioALaHabitacion = servicioALaHabitacion;
        this.servicioPiscina = servicioPiscina;
        this.servicioRestaurante = servicioRestaurante;
        this.servicioGimnasio = servicioGimnasio;
    }

    public HotelInformation(string id, string nombre, string pais, string ciudad, string direccion, string descripcion, float calificacionPromedio, int cantidadHabitaciones, float precioHabitacionesAdultos, float precioHabitacionesMenores, bool servicioALaHabitacion, 
        bool servicioPiscina, bool servicioRestaurante, bool servicioGimnasio, List<string> hotelSpritesUrl)
    {
        this.id = id;
        this.nombre = nombre;
        this.pais = pais;
        this.ciudad = ciudad;
        this.direccion = direccion;
        this.descripcion = descripcion;
        this.calificacionPromedio = calificacionPromedio;
        this.cantidadHabitaciones = cantidadHabitaciones;
        this.precioHabitacionesAdultos = precioHabitacionesAdultos;
        this.precioHabitacionesMenores = precioHabitacionesMenores;
        this.servicioALaHabitacion = servicioALaHabitacion;
        this.servicioPiscina = servicioPiscina;
        this.servicioRestaurante = servicioRestaurante;
        this.servicioGimnasio = servicioGimnasio;
        this.hotelSpritesUrl = hotelSpritesUrl;
    }

    public void SetSpritesUrl(List<string> spriteUrls)
    {
        hotelSpritesUrl = spriteUrls;
    }
}