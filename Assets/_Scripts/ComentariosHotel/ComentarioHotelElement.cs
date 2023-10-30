using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComentarioHotelElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI usuario;
    [SerializeField] private TextMeshProUGUI calificacion;
    [SerializeField] private TextMeshProUGUI comentario;

    private CommentInformation commentInformation;

    public void SetCommentInformation(CommentInformation commentInformation)
    {
        this.commentInformation = commentInformation;
        usuario.text = commentInformation.id_usuario;
        calificacion.text = commentInformation.calificacion.ToString();
        comentario.text = commentInformation.comentario;
    }
}
