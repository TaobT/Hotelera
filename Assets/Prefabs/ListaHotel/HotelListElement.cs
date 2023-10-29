using Doozy.Runtime.Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HotelListElement : MonoBehaviour
{
    [SerializeField] private Image hotelImage;
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
        LoadImage();
    }

    private void LoadImage()
    {
        //Download the first imagen from the hotel on the list
        if (currentHotel.hotelSpritesUrl == null) return;
        StartCoroutine(DownloadSpriteFromUrl(currentHotel.hotelSpritesUrl[0], (sprite) =>
        {
            hotelImage.sprite = sprite;
            //Cambiar el tamaño de hotelImage para que se ajuste a la imagen sin deformarla y sin de los limites del mostradorFoto los cuales son (1340x1390)
            float aspectRatio = hotelImage.sprite.rect.width / hotelImage.sprite.rect.height;
            float newWidth = 1340;
            float newHeight = newWidth / aspectRatio;
            if (newHeight > 1390)
            {
                newHeight = 1390;
                newWidth = newHeight * aspectRatio;
            }
            hotelImage.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        }));

    }

    private IEnumerator DownloadSpriteFromUrl(string url, Action<Sprite> callback)
    {
        UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url);
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.LogError("Error downloading image from url: " + url + "\n" + unityWebRequest.error);
            callback(null);
        }
        else
        {
            Texture2D texture2D = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;
            callback(Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero));
        }
    }


    public void SelectHotel()
    {
        HotelSelected.Singleton.SetHotelInformation(currentHotel);
        
        //Enviar a Formulario Hotel
        Signal.Send("HoteleraScene", "FormHotelSelected", currentHotel);
    }
}
