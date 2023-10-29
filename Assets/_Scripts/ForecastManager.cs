using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForecastManager : MonoBehaviour
{
    //Simular una API que obtiene el promedio de pronostico del clima en un rango de fechas
    //Los tipos de clima pueden ser soleado, nublado, lluvioso, tormenta, etc.
    public enum Clima
    {
        Soleado,
        Nublado,
        Lluvioso
    }

    public static ForecastManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    //Obtener el promedio de clima en un rango de fechas
    public Clima GetForecastAverage()
    {
        //Simular que se obtiene el promedio de clima en un rango de fechas
        //En este caso se obtiene un numero aleatorio entre 0 y 2
        int forecast = Random.Range(0, 3);
        //Convertir el numero aleatorio a un tipo de clima
        switch (forecast)
        {
            case 0:
                return Clima.Soleado;
            case 1:
                return Clima.Nublado;
            case 2:
                return Clima.Lluvioso;
            default:
                return Clima.Soleado;
        }
    }
}
