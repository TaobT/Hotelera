using Doozy.Runtime.Signals;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NativeFilePickerNamespace;

public class FormularioHotelUI : MonoBehaviour
{
    //TODO: Este elemento tambien permite editar o borrar el hotel ademas de registrarlo
    [SerializeField] private TMP_InputField nombreHotelInput;
    [SerializeField] private TMP_InputField paisHotelInput;
    [SerializeField] private TMP_InputField ciudadHotelInput;
    [SerializeField] private TMP_InputField direccionHotelInput;
    [SerializeField] private TMP_InputField descripcionHotelInput;
    [SerializeField] private TMP_InputField precioHotelMayoresInput;
    [SerializeField] private TMP_InputField precioHotelMenoresInput;
    [SerializeField] private TMP_InputField cantidadHotelInput;

    //Servicios toggle
    [SerializeField] private Toggle servicioALaHabitacion;
    [SerializeField] private Toggle servicioPiscina;
    [SerializeField] private Toggle servicioRestaurante;
    [SerializeField] private Toggle servicioGimnasio;

    [SerializeField] private GameObject borrarModal;
    [SerializeField] private Button borrarHotelBtn;
    [SerializeField] private Button saveHotelBtn;
    [SerializeField] private Button salirFormBtn;

    [SerializeField] private Image mostradorFoto;

    
    private List<Sprite> fotos = new List<Sprite>();
    private List<string> fotosUbication = new List<string>();
    private int currentPhotoIndex = 0;

    private void Awake()
    {
        saveHotelBtn.onClick.AddListener(OnSaveHotelBtnClick);
        salirFormBtn.onClick.AddListener(OnSalirFormBtnClick);
        borrarHotelBtn.onClick.AddListener(OnDeleteHotelBtnClick);
    }

    public void VerificarHotelSelected()
    {
        if (HotelSelected.Singleton == null) return;
        if (!HotelSelected.Singleton.HasHotelInformation())
        {
            LimpiarInputParaRegistrar();
        }
        else
        {
            LlenarInputsParaEditarOBorrar();
        }
    }

    public void PickPhoto()
    {
        NativeFilePicker.RequestPermissionAsync();
        NativeFilePicker.PickFile((path) =>
        {
            Debug.Log("Picked file: " + path);
            fotos.Add(LoadImageFromPath(path));
            fotosUbication.Add(path);
            LoadCurrentPhotoIndex();
        }, new string[] { "image/*" });
    }

    public void NextPhoto()
    {
        currentPhotoIndex++;
        if(currentPhotoIndex >= fotos.Count)
        {
            currentPhotoIndex = 0;
        }
        LoadCurrentPhotoIndex();
    }

    public void PreviousPhoto()
    {
        currentPhotoIndex--;
        if(currentPhotoIndex < 0)
        {
            currentPhotoIndex = fotos.Count - 1;
        }
        LoadCurrentPhotoIndex();
    }

    private void LoadCurrentPhotoIndex()
    {
        if(fotos.Count == 0)
        {
            mostradorFoto.sprite = null;
            return;
        }
        if(currentPhotoIndex < 0 || currentPhotoIndex >= fotos.Count) return;
        Sprite currentPhoto = fotos[currentPhotoIndex];
        //Cambiar el tamaño de mostradorFoto para que se ajuste a la imagen sin deformarla y sin de los limites del mostradorFoto los cuales son (1340 x 1000)
        float aspectRatio = currentPhoto.rect.width / currentPhoto.rect.height;
        float newWidth = 1340;
        float newHeight = newWidth / aspectRatio;
        if(newHeight > 1000)
        {
            newHeight = 1000;
            newWidth = newHeight * aspectRatio;
        }
        mostradorFoto.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        mostradorFoto.sprite = currentPhoto;
    }

    public void DeletePhoto()
    {
        fotos.RemoveAt(currentPhotoIndex);
        fotosUbication.RemoveAt(currentPhotoIndex);
        PreviousPhoto();
        LoadCurrentPhotoIndex();
    }

    //Load image from path
    private Sprite LoadImageFromPath(string path)
    {
        Texture2D tex = NativeGallery.LoadImageAtPath(path, 512);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        return sprite;
    }

    private void LimpiarInputParaRegistrar()
    {
        nombreHotelInput.text = "";
        paisHotelInput.text = "";
        ciudadHotelInput.text = "";
        direccionHotelInput.text = "";
        descripcionHotelInput.text = "";
        precioHotelMayoresInput.text = "";
        precioHotelMenoresInput.text = "";
        cantidadHotelInput.text = "";
        servicioALaHabitacion.isOn = false;
        servicioPiscina.isOn = false;
        servicioRestaurante.isOn = false;
        servicioGimnasio.isOn = false;
        borrarHotelBtn.gameObject.SetActive(false);
    }

    private void LlenarInputsParaEditarOBorrar()
    {
        nombreHotelInput.text = HotelSelected.Singleton.GetHotelInformation().nombre;
        paisHotelInput.text = HotelSelected.Singleton.GetHotelInformation().pais;
        ciudadHotelInput.text = HotelSelected.Singleton.GetHotelInformation().ciudad;
        direccionHotelInput.text = HotelSelected.Singleton.GetHotelInformation().direccion;
        descripcionHotelInput.text = HotelSelected.Singleton.GetHotelInformation().descripcion;
        precioHotelMayoresInput.text = "" + HotelSelected.Singleton.GetHotelInformation().precioHabitacionesAdultos;
        precioHotelMenoresInput.text = "" + HotelSelected.Singleton.GetHotelInformation().precioHabitacionesMenores;
        cantidadHotelInput.text = "" + HotelSelected.Singleton.GetHotelInformation().cantidadHabitaciones;

        servicioALaHabitacion.isOn = HotelSelected.Singleton.GetHotelInformation().servicioALaHabitacion;
        servicioPiscina.isOn = HotelSelected.Singleton.GetHotelInformation().servicioPiscina;
        servicioRestaurante.isOn = HotelSelected.Singleton.GetHotelInformation().servicioRestaurante;
        servicioGimnasio.isOn = HotelSelected.Singleton.GetHotelInformation().servicioGimnasio;

        borrarHotelBtn.gameObject.SetActive(true);
    }

    private void OnDeleteHotelBtnClick()
    {
        borrarModal.gameObject.SetActive(true);
    }

    private void OnSaveHotelBtnClick()
    {
        
        if(!ValidateInputs()) return;

        if(HotelSelected.Singleton.GetHotelInformation() == null)
        {
            //Registar Hotel
            DatabaseManager.Instance.RegisterHotel(nombreHotelInput.text, paisHotelInput.text, ciudadHotelInput.text, direccionHotelInput.text, descripcionHotelInput.text,
                float.Parse(precioHotelMayoresInput.text), float.Parse(precioHotelMenoresInput.text), int.Parse(cantidadHotelInput.text), 
                servicioALaHabitacion.isOn, servicioPiscina.isOn, servicioRestaurante.isOn, servicioGimnasio.isOn, fotosUbication);
        }
        else
        {
            //Actualizar Hotel
            DatabaseManager.Instance.UpdateHotel(HotelSelected.Singleton.GetHotelInformation().id, nombreHotelInput.text, paisHotelInput.text, ciudadHotelInput.text, direccionHotelInput.text, descripcionHotelInput.text,
                float.Parse(precioHotelMayoresInput.text), float.Parse(precioHotelMenoresInput.text), int.Parse(cantidadHotelInput.text), 
                servicioALaHabitacion.isOn, servicioPiscina.isOn, servicioRestaurante.isOn, servicioGimnasio.isOn);
        }


        //Show succes message
        //Regresar a Lista de Hoteles
        Signal.Send("HoteleraScene", "RegresarAListaHotel");
    }

    private void OnSalirFormBtnClick()
    {
        //Regresar a Lista de Hoteles
        Signal.Send("HoteleraScene", "RegresarAListaHotel");
    }
    
    private bool ValidateInputs()
    {
        //Verificar que todos los inputs no esten vacios

        if(string.IsNullOrEmpty(nombreHotelInput.text))
        {
            Debug.LogError("El nombre del hotel no puede estar vacio");
            return false;
        }

        if(string.IsNullOrEmpty(paisHotelInput.text))
        {
            Debug.LogError("El país del hotel no puede estar vacia");
            return false;
        }

        if(string.IsNullOrEmpty(ciudadHotelInput.text))
        {
            Debug.LogError("La ciudad del hotel no puede estar vacia");
            return false;
        }

        if(string.IsNullOrEmpty(direccionHotelInput.text))
        {
            Debug.LogError("La direccion del hotel no puede estar vacia");
            return false;
        }

        if(string.IsNullOrEmpty(descripcionHotelInput.text))
        {
            Debug.LogError("La descripcion del hotel no puede estar vacia");
            return false;
        }

        if(string.IsNullOrEmpty(precioHotelMayoresInput.text))
        {
            Debug.LogError("El precio del hotel para mayores no puede estar vacio");
            return false;
        }

        if(string.IsNullOrEmpty(precioHotelMenoresInput.text))
        {
            Debug.LogError("El precio del hotel para menores no puede estar vacio");
            return false;
        }

        if(string.IsNullOrEmpty(cantidadHotelInput.text))
        {
            Debug.LogError("La cantidad de habitaciones del hotel no puede estar vacia");
            return false;
        }

        //Verificar que el precio y la cantidad sean numeros
        if(!float.TryParse(precioHotelMayoresInput.text, out float precio))
        {
            Debug.LogError("El precio del hotel para mayores debe ser un numero");
            return false;
        }

        if(!float.TryParse(precioHotelMayoresInput.text, out precio))
        {
            Debug.LogError("El precio del hotel para menores debe ser un numero");
            return false;
        }

        if(!int.TryParse(cantidadHotelInput.text, out int cantidad))
        {
            Debug.LogError("La cantidad de habitaciones del hotel debe ser un numero");
            return false;
        }

        return true;
    }
}
