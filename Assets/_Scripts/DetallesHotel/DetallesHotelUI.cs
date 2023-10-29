using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static Doozy.Editor.EditorUI.EditorTextures.EditorUI;

public class DetallesHotelUI : MonoBehaviour
{
    [SerializeField] private GameObject[] clientElements;
    [SerializeField] private GameObject[] ownerElements;

    [SerializeField] private Button makeReservationBtn;

    private List<Sprite> fotos = new List<Sprite>();

    [SerializeField] private Image mostradorFoto;
    [SerializeField] private TextMeshProUGUI hotelNameTxt;
    [SerializeField] private TextMeshProUGUI ubicationTxt;
    [SerializeField] private TextMeshProUGUI directionTxt;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private TextMeshProUGUI averageRatingTxt;
    [SerializeField] private TextMeshProUGUI adultPriceTxt;
    [SerializeField] private TextMeshProUGUI minorPriceTxt;
    [SerializeField] private TextMeshProUGUI climaPromedioTxt;
    [SerializeField] private Toggle serviceRoom;
    [SerializeField] private Toggle servicePool;
    [SerializeField] private Toggle serviceRestaurant;
    [SerializeField] private Toggle serviceGym;

    [SerializeField] private GameObject commentaryElementUIPf;
    [SerializeField] private Transform commentaryContentUI;

    private int currentPhotoIndex = 0;

    private void Awake()
    {
        //Diferenciar entre Cliente y Dueño
        if (UserInfo.Singleton.IsClient())
        {
            ShowClientElements();
        }
        else
        {
            ShowOwnerElements();
        }
        //Traer toda la información para mostrarla en los elementos
    }

    private void OnEnable()
    {
        //Llenar toda la información de los elementos con HotelSelected
        LlenarInformacion();
    }

    private void Start()
    {
        LlenarInformacion();
    }

    public void ObtenerClima()
    {
        climaPromedioTxt.text = ForecastManager.Instance.GetForecastAverage().ToString();
    }

    public void LlenarInformacion()
    {
        if (HotelSelected.Singleton == null || !HotelSelected.Singleton.HasHotelInformation()) return;
        //Llenar toda la información de los elementos con HotelSelected
        HotelInformation hotelInformation = HotelSelected.Singleton.GetHotelInformation();
        hotelNameTxt.text = hotelInformation.nombre;
        ubicationTxt.text = hotelInformation.pais + ", " + hotelInformation.ciudad;
        directionTxt.text = hotelInformation.direccion;
        descriptionTxt.text = hotelInformation.descripcion;
        averageRatingTxt.text = hotelInformation.calificacionPromedio.ToString();
        adultPriceTxt.text = hotelInformation.precioHabitacionesAdultos.ToString();
        minorPriceTxt.text = hotelInformation.precioHabitacionesMenores.ToString();
        serviceRoom.isOn = hotelInformation.servicioALaHabitacion;
        servicePool.isOn = hotelInformation.servicioPiscina;
        serviceRestaurant.isOn = hotelInformation.servicioRestaurante;
        serviceGym.isOn = hotelInformation.servicioGimnasio;

        DownloadHotelImages();
    }

    public void NextPhoto()
    {
        currentPhotoIndex++;
        if (currentPhotoIndex >= fotos.Count)
        {
            currentPhotoIndex = 0;
        }
        LoadCurrentPhotoIndex();
    }

    public void PreviousPhoto()
    {
        currentPhotoIndex--;
        if (currentPhotoIndex < 0)
        {
            currentPhotoIndex = fotos.Count - 1;
        }
        LoadCurrentPhotoIndex();
    }

    private void LoadCurrentPhotoIndex()
    {
        if (fotos.Count == 0)
        {
            mostradorFoto.sprite = null;
            return;
        }
        if (currentPhotoIndex < 0 || currentPhotoIndex >= fotos.Count) return;
        Sprite currentPhoto = fotos[currentPhotoIndex];
        //Cambiar el tamaño de mostradorFoto para que se ajuste a la imagen sin deformarla y sin de los limites del mostradorFoto los cuales son (1340 x 1000)
        float aspectRatio = currentPhoto.rect.width / currentPhoto.rect.height;
        float newWidth = 1340;
        float newHeight = newWidth / aspectRatio;
        if (newHeight > 1000)
        {
            newHeight = 1000;
            newWidth = newHeight * aspectRatio;
        }
        mostradorFoto.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        mostradorFoto.sprite = currentPhoto;
    }

    private void DownloadHotelImages()
    {
        foreach(string url in HotelSelected.Singleton.GetHotelInformation().hotelSpritesUrl)
        {
            StartCoroutine(DownloadSpriteFromUrl(url, (sprite) =>
            {
                fotos.Add(sprite);
            }));
        }
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

    private void OnMakeReservationBtnClick()
    {
        //Show payment method ui
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
