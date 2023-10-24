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
}

public struct UserInformation
{
    public string id;
    public string nombre;
    public string user_type;
    public string correo_electronico;
}
