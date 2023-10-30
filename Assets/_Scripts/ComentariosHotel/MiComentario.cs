using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiComentario : MonoBehaviour
{

    private bool tieneComenario = false;
    [SerializeField] private Slider estrellasSlider;
    [SerializeField] private TextMeshProUGUI estrellasText;
    [SerializeField] private TMP_InputField comentario;
    [SerializeField] private Button saveEditCommentBtn;
    [SerializeField] private Button onDeleteCommentBtn;


    private CommentInformation commentInformation;
    private void Awake()
    {
        estrellasSlider.onValueChanged.AddListener(OnSliderValueChanged);
        saveEditCommentBtn.onClick.AddListener(OnSaveEditCommentBtnClick);
        onDeleteCommentBtn.onClick.AddListener(OnDeleteCommentBtnClick);
    }

    public void VerificarComentario()
    {
        VerificarSiExisteComentario();
    }

    private void OnSaveEditCommentBtnClick()
    {
        if (tieneComenario)
        {
            //Habilitar todos los inputs para comentar 
            estrellasSlider.interactable = true;
            comentario.interactable = true;
            saveEditCommentBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Guardar";
            onDeleteCommentBtn.gameObject.SetActive(false);
        }
        else
        {
            //Guardar comentario
            SaveComment();
            //Deshabilitar todos los inputs
            estrellasSlider.interactable = false;
            comentario.interactable = false;
            saveEditCommentBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Editar";
            onDeleteCommentBtn.gameObject.SetActive(true);
        }
    }

    private void OnDeleteCommentBtnClick()
    {
        DatabaseManager.Instance.DeleteComment(commentInformation.id);
    }

    private async void VerificarSiExisteComentario()
    {
        //Si existe comentario entonces mostrarlo y permitir editar
        //Si no existe comentario entonces mostrar el formulario para crear uno
        CommentInformation comment = await DatabaseManager.Instance.GetCommentFromHotelOfUser(HotelSelected.Singleton.GetHotelInformation().id, UserInfo.Singleton.information.id);
        if (comment != null)
        {
            comentario.text = comment.comentario;
            estrellasSlider.value = comment.calificacion;
            estrellasText.text = comment.calificacion.ToString();
            tieneComenario = true;
            //Deshabilitar todos los inputs

            estrellasSlider.interactable = false;
            comentario.interactable = false;

            saveEditCommentBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Editar";
            onDeleteCommentBtn.gameObject.SetActive(true);

            commentInformation = comment;
        }
        else
        {
            comentario.text = "";
            estrellasSlider.value = 0;
            estrellasText.text = "0";
            tieneComenario = false;

            estrellasSlider.interactable = true;
            comentario.interactable = true;

            saveEditCommentBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Guardar";
        }
    }

    private void OnSliderValueChanged(float value)
    {
        estrellasText.text = value.ToString();
    }

    private void SaveComment()
    {
        if(tieneComenario)
            DatabaseManager.Instance.UpdateComment(commentInformation.id, comentario.text, (int)estrellasSlider.value);
        else DatabaseManager.Instance.RegisterComment(HotelSelected.Singleton.GetHotelInformation().id, UserInfo.Singleton.information.id, comentario.text, (int)estrellasSlider.value);

    }
}
