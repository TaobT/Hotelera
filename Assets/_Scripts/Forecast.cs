using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forecast : MonoBehaviour
{
    public TextAsset weatherData; // Coloca los datos de la API en este campo.

    public DateTime startDate;
    public DateTime endDate;

    private List<float> temperatureData;

    private void Start()
    {
        // Analiza los datos de la API y obtén la lista de temperaturas.
        temperatureData = ParseWeatherData(weatherData.text);

        // Filtra los datos para el rango de fechas especificado.
        List<float> temperatureInRange = GetTemperatureDataInRange(temperatureData, startDate, endDate);

        // Calcula el promedio de la temperatura.
        float temperatureAverage = CalculateAverage(temperatureInRange);

        // Determina el tipo de clima.
        string weatherType = DetermineWeatherType(temperatureAverage);

        Debug.Log($"El clima promedio en el rango de fechas es {weatherType}");
    }

    // Analiza los datos de la API y obtén la lista de temperaturas.
    private List<float> ParseWeatherData(string weatherData)
    {
        // Aquí debes analizar el JSON de la API y extraer los valores de "temperature_2m".
        // Te recomiendo usar una biblioteca como JsonUtility o Newtonsoft.Json para analizar el JSON.
        // A continuación, se muestra un ejemplo simplificado:

        // Reemplaza esto con el análisis real del JSON de la API.
        List<float> temperatureData = new List<float>();

        // Ejemplo simplificado:
        temperatureData.Add(8.8f);
        temperatureData.Add(8.5f);
        // ...

        return temperatureData;
    }

    // Filtra los datos para el rango de fechas especificado.
    private List<float> GetTemperatureDataInRange(List<float> temperatureData, DateTime startDate, DateTime endDate)
    {
        List<float> temperatureInRange = new List<float>();

        for (int i = 0; i < temperatureData.Count; i++)
        {
            // Aquí compara la fecha y hora en temperatureData[i] con el rango especificado.
            // Si está dentro del rango, agrega la temperatura correspondiente.
            // Asegúrate de manejar adecuadamente la conversión de fechas y horas en tu análisis real.

            // Ejemplo simplificado:
            DateTime dataTime = DateTime.Parse("2023-10-29T00:00"); // Reemplaza con la fecha real de temperatureData[i].
            if (dataTime >= startDate && dataTime <= endDate)
            {
                temperatureInRange.Add(temperatureData[i]);
            }
        }

        return temperatureInRange;
    }

    // Calcula el promedio de la temperatura.
    private float CalculateAverage(List<float> temperatureData)
    {
        if (temperatureData.Count == 0)
        {
            return 0f;
        }

        float sum = 0;
        foreach (float temperature in temperatureData)
        {
            sum += temperature;
        }

        return sum / temperatureData.Count;
    }

    // Determina el tipo de clima en función de la temperatura promedio.
    private string DetermineWeatherType(float temperatureAverage)
    {
        // Define tus propios umbrales para clasificar el clima como soleado, nublado o lluvioso.
        // Puedes ajustar estos valores según tus criterios.

        if (temperatureAverage >= 25.0f)
        {
            return "Soleado";
        }
        else if (temperatureAverage >= 15.0f)
        {
            return "Nublado";
        }
        else
        {
            return "Lluvioso";
        }
    }
}
