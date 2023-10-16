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
        //Registrar usuario en la base de datos en la colección "Usuarios"
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
    #endregion

    #region Hoteles

    #endregion
}
