using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Firestore;
using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

    private FirebaseApp app;
    private FirebaseFirestore db;
    private FirebaseStorage storage;

    public UnityEvent OnFirebaseInitialized = new UnityEvent();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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

            if(task.Result == DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                db = FirebaseFirestore.GetInstance(app);
                storage = FirebaseStorage.GetInstance(app);
                OnFirebaseInitialized.Invoke();
                Debug.Log("Firebase initialized successfully");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }


        });

    }

    #region User
    public bool RegisterUser(string nombre, string apellidos, string email, string password, string user_type)
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
            {"email", email.ToLower() },
            {"password", Password.HashPassword(password, email)},
            {"user_type", user_type }
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

    public async Task<UserInformation> GetUser(string email, string password)
    {
        //return user information
        if (Instance == null)
        {
            Debug.LogError("No se ha inicializado el DatabaseManager");
            return null;
        }

        email = email.ToLower();

        //Verificar que el hash del password sea igual al que esta en la base de datos
        string localHash = Password.HashPassword(password, email);

        Debug.Log("Obteniendo usuario de la base de datos");
        UserInformation userInformation = null;
        //Obtener usuario de la base de datos en la colección "Usuarios" que coincida con el email
        await db.Collection("Usuarios").WhereEqualTo("email", email).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Error al obtener usuario: {task.Exception}");
            }
            Debug.Log("Usuario obtenido exitosamente");

            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    Debug.Log("Usuario encontrado");
                    string remoteHash = document.GetValue<string>("password");
                    if (!localHash.Equals(remoteHash))
                    {
                        Debug.LogError("El password es incorrecto");
                        return;
                    }
                    userInformation = new UserInformation(document.Id, document.GetValue<string>("nombre") + document.GetValue<string>("apellidos"), document.GetValue<string>("user_type"), document.GetValue<string>("email"));
                }
            }
        });

        return userInformation;
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

    public async void RegisterHotel(string nombre, string pais, string ciudad, string direccion, string descripcion, float precioMayores, float precioMenores, int cantidadHabitaciones,
        bool servicioALaHabitacion, bool servicioPiscina, bool servicioRestaurante, bool servicioGimnasio, List<string> fotosUbication)
    {
        //Aqui se debe registrar el hotel en la base de datos
        //Si el registro fue exitoso, retornar true
        //Si hubo un error, retornar false
        if(Instance == null)
        {
            Debug.LogError("No se ha inicializado el DatabaseManager");
            return;
        }

        string hotelId = "";

        Debug.Log("Registrando hotel en la base de datos");
        //Registrar usuario en la base de datos en la coleccion "Hoteles"
        await db.Collection("Hoteles").AddAsync(new Dictionary<string, object>
        {
            {"nombre", nombre },
            {"pais", pais },
            {"ciudad", ciudad },
            {"direccion", direccion },
            {"descripcion", descripcion },
            {"calificacion", 0 },
            {"precioMayores", precioMayores },
            {"precioMenores", precioMenores },
            {"cantidadHabitaciones", cantidadHabitaciones },
            {"servicioALaHabitacion", servicioALaHabitacion },
            {"servicioPiscina", servicioPiscina },
            {"servicioRestaurante", servicioRestaurante },
            {"servicioGimnasio", servicioGimnasio },
            {"ownerId", UserInfo.Singleton.information.id }
        }).ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogError($"Error al registrar hotel: {task.Exception}");
                return;
            }

            DocumentReference document = task.Result;
            hotelId = document.Id;

            
            Debug.Log("Hotel registrado exitosamente");
        });

        List<string> fotosUrl = new List<string>();
        //Guardar fotos en FirebaseStorage en una carpeta con la id del hotel
        foreach (string foto in fotosUbication)
        {
            //Convertir la imagen a un arreglo de bytes
            byte[] bytes = File.ReadAllBytes(foto);
            //Crear una referencia al archivo en FirebaseStorage
            string fileName = Path.GetFileName(foto);
            StorageReference storageReference = storage.RootReference.Child("Hoteles").Child(hotelId).Child(fileName);
            //Subir el archivo a FirebaseStorage
            await storageReference.PutBytesAsync(bytes).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"Error al subir foto: {task.Exception}");
                    return;
                }
                //Obtener la url de la foto
                storageReference.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError($"Error al obtener url de foto: {task.Exception}");
                        return;
                    }
                    fotosUrl.Add(task.Result.ToString());

                    
                });
                Debug.Log("Foto subida exitosamente");
            });

        }

        Dictionary<string, object> hotelFotos = new Dictionary<string, object>();

        for(int i = 0; i < fotosUrl.Count + 1; i++)
        {
            hotelFotos.Add($"foto{i}", fotosUrl[i]);
        }

        await db.Collection("Hoteles").Document(hotelId).Collection("Fotos").AddAsync(hotelFotos).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Error al guardar fotos en la base de datos: {task.Exception}");
                return;
            }
            Debug.Log("Fotos guardadas exitosamente");
        });
    }

    public List<HotelInformation> GetHoteles()
    {
        List<HotelInformation> hoteles = new List<HotelInformation>();
        //return user information
        if (Instance == null)
        {
            Debug.LogError("No se ha inicializado el DatabaseManager");
            return hoteles;
        }
        Debug.Log("Obteniendo hoteles de la base de datos");
        //Obtener usuario de la base de datos en la colección "Usuarios" que coincida con el email
        db.Collection("Hoteles").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Error al obtener hoteles: {task.Exception}");
            }
            Debug.Log("Hoteles obtenidos exitosamente");
            QuerySnapshot snapshot = task.Result;
            //int hotelsAdded = 0;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    Debug.Log("Hotel encontrado");
                    hoteles.Add(new HotelInformation(document.Id, document.GetValue<string>("nombre"),
                        document.GetValue<string>("direccion"), document.GetValue<string>("ciudad"), document.GetValue<string>("direccion"), document.GetValue<string>("descripcion"),
                        document.GetValue<int>("calificacion"), document.GetValue<int>("cantidadHabitaciones"),
                        document.GetValue<float>("precioMayores"), document.GetValue<float>("precioMenores"),
                        document.GetValue<bool>("servicioALaHabitacion"), document.GetValue<bool>("servicioPiscina"),
                        document.GetValue<bool>("servicioRestaurante"), document.GetValue<bool>("servicioGimnasio")));
                }
            }
        });
        return hoteles;
    }

    public List<HotelInformation> GetHotelsWithFilter(string filter, bool includeHotelName = false)
    {
        //Obtener cualquier coincidencia con el filtro en los campos de pais, ciudad, direccion y nombre
        List<HotelInformation> hoteles = new List<HotelInformation>();
        //return user information
        if (Instance == null)
        {
            Debug.LogError("No se ha inicializado el DatabaseManager");
            return hoteles;
        }

        Debug.Log("Obteniendo hoteles de la base de datos");
        //Obtener hoteles de la base de datos en la colección "Hoteles" que coincida con el filtro de ubicación
        
        //Agregar hoteles donde el pais coincida con el filtro
        db.Collection("Hoteles").WhereEqualTo("pais", filter).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Error al obtener hoteles: {task.Exception}");
            }
            Debug.Log("Hoteles obtenidos exitosamente");
            QuerySnapshot snapshot = task.Result;
            //int hotelsAdded = 0;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    Debug.Log("Hotel encontrado");
                    hoteles.Add(new HotelInformation(document.Id, document.GetValue<string>("nombre"),
                    document.GetValue<string>("direccion"), document.GetValue<string>("ciudad"), document.GetValue<string>("direccion"), document.GetValue<string>("descripcion"),
                    document.GetValue<int>("calificacion"), document.GetValue<int>("cantidadHabitaciones"),
                    document.GetValue<float>("precioMayores"), document.GetValue<float>("precioMenores"),
                    document.GetValue<bool>("servicioALaHabitacion"), document.GetValue<bool>("servicioPiscina"),
                    document.GetValue<bool>("servicioRestaurante"), document.GetValue<bool>("servicioGimnasio")));
                }
            }
        });

        //Agregar hoteles donde la ciudad coincida con el filtro
        db.Collection("Hoteles").WhereEqualTo("ciudad", filter).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Error al obtener hoteles: {task.Exception}");
            }
            Debug.Log("Hoteles obtenidos exitosamente");
            QuerySnapshot snapshot = task.Result;
            //int hotelsAdded = 0;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    Debug.Log("Hotel encontrado");
                    hoteles.Add(new HotelInformation(document.Id, document.GetValue<string>("nombre"),
                    document.GetValue<string>("direccion"), document.GetValue<string>("ciudad"), document.GetValue<string>("direccion"), document.GetValue<string>("descripcion"),
                    document.GetValue<int>("calificacion"), document.GetValue<int>("cantidadHabitaciones"),
                    document.GetValue<float>("precioMayores"), document.GetValue<float>("precioMenores"),
                    document.GetValue<bool>("servicioALaHabitacion"), document.GetValue<bool>("servicioPiscina"), document.GetValue<bool>("servicioRestaurante"), document.GetValue<bool>("servicioGimnasio")));
                }
            }
        });

        //Agregar hoteles donde la direccion coincida con el filtro
        db.Collection("Hoteles").WhereEqualTo("direccion", filter).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Error al obtener hoteles: {task.Exception}");
            }
            Debug.Log("Hoteles obtenidos exitosamente");
            QuerySnapshot snapshot = task.Result;
            //int hotelsAdded = 0;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    Debug.Log("Hotel encontrado");
                    hoteles.Add(new HotelInformation(document.Id, document.GetValue<string>("nombre"),
                    document.GetValue<string>("direccion"), document.GetValue<string>("ciudad"), document.GetValue<string>("direccion"), document.GetValue<string>("descripcion"),
                    document.GetValue<int>("calificacion"), document.GetValue<int>("cantidadHabitaciones"),
                    document.GetValue<float>("precioMayores"), document.GetValue<float>("precioMenores"),
                    document.GetValue<bool>("servicioALaHabitacion"), document.GetValue<bool>("servicioPiscina"),
                    document.GetValue<bool>("servicioRestaurante"), document.GetValue<bool>("servicioGimnasio")));
                }
            }
        });

        //Agregar hoteles donde el nombre coincida con el filtro
        if (includeHotelName)
        {
            db.Collection("Hoteles").WhereEqualTo("nombre", filter).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"Error al obtener hoteles: {task.Exception}");
                }
                Debug.Log("Hoteles obtenidos exitosamente");
                QuerySnapshot snapshot = task.Result;
                //int hotelsAdded = 0;
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    if (document.Exists)
                    {
                        Debug.Log("Hotel encontrado");
                        hoteles.Add(new HotelInformation(document.Id, document.GetValue<string>("nombre"),
                        document.GetValue<string>("direccion"), document.GetValue<string>("ciudad"), document.GetValue<string>("direccion"), document.GetValue<string>("descripcion"),
                        document.GetValue<int>("calificacion"), document.GetValue<int>("cantidadHabitaciones"),
                        document.GetValue<float>("precioMayores"), document.GetValue<float>("precioMenores"),
                        document.GetValue<bool>("servicioALaHabitacion"), document.GetValue<bool>("servicioPiscina"),
                        document.GetValue<bool>("servicioRestaurante"), document.GetValue<bool>("servicioGimnasio")));
                    }
                }
            });
        }

        return hoteles;
    }

    public void DeleteHotel(string id)
    {
        if (Instance == null)
        {
            Debug.LogError("No se ha inicializado el DatabaseManager");
            return;
        }
        Debug.Log("Eliminando hotel de la base de datos");
        //Eliminar hotel de la base de datos en la colección "Hoteles" con el id especificado
        db.Collection("Hoteles").Document(id).DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Error al eliminar hotel: {task.Exception}");
            }
            Debug.Log("Hotel eliminado exitosamente");
        });
    }

    public void UpdateHotel(string id, string nombre, string pais, string ciudad, string direccion, string descripcion, float precioMayores, float precioMenores, int cantidadHabitaciones,
        bool servicioALaHabitacion, bool servicioPiscina, bool servicioRestaurante, bool servicioGimnasio)
    {
        if (Instance == null)
        {
            Debug.LogError("No se ha inicializado el DatabaseManager");
            return;
        }

        Debug.Log("Actualizando hotel en la base de datos");
        //Actualizar hotel en la base de datos en la colección "Hoteles" con el id especificado
        db.Collection("Hoteles").Document(id).UpdateAsync(new Dictionary<string, object>
        {
            {"nombre", nombre},
            {"pais", pais},
            {"ciudad", ciudad},
            {"direccion", direccion},
            {"descripcion", descripcion},
            {"precioMayores", precioMayores},
            {"precioMenores", precioMenores},
            {"cantidadHabitaciones", cantidadHabitaciones},
            {"servicioALaHabitacion", servicioALaHabitacion},
            {"servicioPiscina", servicioPiscina},
            {"servicioRestaurante", servicioRestaurante},
            {"servicioGimnasio", servicioGimnasio}
        }).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Error al actualizar hotel: {task.Exception}");
            }
            Debug.Log("Hotel actualizado exitosamente");
        });
    }
    #endregion
}
