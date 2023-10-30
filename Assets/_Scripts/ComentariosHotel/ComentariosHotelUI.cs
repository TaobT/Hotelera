using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComentariosHotelUI : MonoBehaviour
{
    [SerializeField] private GameObject comentarioPf;
    [SerializeField] private Transform comentariosContainer;

    public void UpdateComments()
    {
        LlenarComentarios();
    }

    private async void LlenarComentarios()
    {
        foreach (Transform child in comentariosContainer)
        {
            Destroy(child.gameObject);
        }
        List<CommentInformation> comentarios = await DatabaseManager.Instance.GetCommentsOfHotel(HotelSelected.Singleton.GetHotelInformation().id);
        foreach (CommentInformation comment in comentarios)
        {
            GameObject comentario = Instantiate(comentarioPf, comentariosContainer);
            comentario.GetComponent<ComentarioHotelElement>().SetCommentInformation(comment);
        }
    }
}
