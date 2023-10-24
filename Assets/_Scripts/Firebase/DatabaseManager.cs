using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

    private FirebaseApp app;
    private FirebaseFirestore db;

    public UnityEvent OnFirebaseInitialized = new UnityEvent();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError($"Failed to initialize Firebase with {task.Exception}");
                return;
            }

            app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);
            Debug.Log("Firebase initialized successfully");

            OnFirebaseInitialized.Invoke();

        });

    }

    #region User
    public bool RegisterUser(string nombre, string apellidos, string email, string password)
    {
        //Aqui se debe registrar el usuario en la base de datos
        //Si el registro fue exitoso, retornar true
        //Si hubo un error, retornar false
        if(Instance == null)
        {
            Debug.LogError("No se ha inicializado el DatabaseManager");
            return false;
        }

        Debug.Log("Registrando usuario en la base de datos");
        //Registrar usuario en la base de datos en la colecci�n "Usuarios"
        db.Collection("Usuarios").AddAsync(new Dictionary<string, object>
        {
            {"nombre", nombre },
            {"apellidos", apellidos },
            {"email", email },
            {"password", Password.HashPassword(password, email)}
        }).ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogError($"Error al registrar usuario: {task.Exception}");
                return;
            }
            Debug.Log("Usuario registrado exitosamente");
        });
        return true;
    }

    public void GetUser(string email)
    {
        //return user information
    }
    


    public bool RegisterReservation(string id_user, string id_hotel, string fecha_entrada, string fecha_salida, float pago)
    {
        //Aqui se debe registrar la reservación en la base de datos
        //Si el registro fue exitoso, retornar true
        //Si hubo un error, retornar false
        if(Instance == null)
        {
            Debug.LogError("No se ha inicializado el DatabaseManager");
            return false;
        }

        Debug.Log("Registrando reservacion en la base de datos");
        //Registrar usuario en la base de datos en la colecci�n "Reservaciones"
        db.Collection("Reservaciones").AddAsync(new Dictionary<string, object>
        {
            {"usuario", id_user },
            {"hotel", id_hotel },
            {"fecha_entrada", fecha_entrada },
            {"fecha_salida", fecha_salida },
            {"pago", pago }

        }).ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogError($"Error al registrar reservacion: {task.Exception}");
                return;
            }
            Debug.Log("Reservacion registrado exitosamente");
        });
        return true;
    }
    #endregion

    #region Hoteles

    #endregion
}
