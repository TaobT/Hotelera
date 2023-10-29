using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListaHotelesUI : MonoBehaviour
{
    [SerializeField] private GameObject[] clientElements;
    [SerializeField] private GameObject[] ownerElements;

    [SerializeField] private GameObject listaHotelUIPf;
    [SerializeField] private Transform listaHotelUIContent;

    [SerializeField] private TMP_InputField ownerHotelSearchField;

    private List<HotelInformation> hotels;

    private void Awake()
    {
        //Diferenciar entro Cliente y Dueño
        if(UserInfo.Singleton.IsClient())
        {
            ShowClientElements();
        }
        else
        {
            ShowOwnerElements();
        }
        //Traer los hoteles de acuerdo al filtro

        ownerHotelSearchField.onValueChanged.AddListener(OnOwnerHotelSearchFieldChange);
    }

    private void Start()
    {
        GetAllHotels();
    }

    private void OnEnable()
    {
        //Limpiar HotelSelected
        if (HotelSelected.Singleton == null) return;
        HotelSelected.Singleton.CleanHotelInformation();
    }


    private Coroutine loadHotelesCoroutine;
    //Esta siendo usado por el evento OnShowCallback de UIView
    public IEnumerator LoadHoteles()
    {
        //Limpiar la lista de hoteles
        foreach (Transform child in listaHotelUIContent)
        {
            Destroy(child.gameObject);
        }
        yield return new WaitForSeconds(2f);
        foreach (HotelInformation hotel in hotels)
        {
            GameObject listaHotelUI = Instantiate(listaHotelUIPf, listaHotelUIContent);
            listaHotelUI.GetComponent<HotelListElement>().SetHotelInformation(hotel);
        }
    }

    private async void GetAllHotels()
    {
        if(loadHotelesCoroutine != null)
        {
            StopCoroutine(loadHotelesCoroutine);
        }
        hotels = new List<HotelInformation>();
        hotels = await DatabaseManager.Instance.GetHoteles();
        StartCoroutine(LoadHoteles());
    }

    private async void OnOwnerHotelSearchFieldChange(string value)
    {
        if(loadHotelesCoroutine != null)
        {
            StopCoroutine(loadHotelesCoroutine);
        }
        hotels = new List<HotelInformation>();
        hotels = await DatabaseManager.Instance.GetHotelsWithFilter(value);
        loadHotelesCoroutine = StartCoroutine(LoadHoteles());
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
