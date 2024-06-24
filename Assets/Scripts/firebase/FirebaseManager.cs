using Firebase.Database;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Collections;

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference dbReference;
    private FirebaseAuth auth;
    private FirebaseUser user;

    private ProgresoVista progresoVista;
    public List<UsuarioBase> usuariosList = new List<UsuarioBase>();
    private Rol rolScript; // Reference to the Rol script
    public Rol RolScript
    {
        set { rolScript = value; }
    }

    public FirebaseAuth GetFirebaseAuth()
    {
        return auth;
    }

    private void Awake()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                auth = FirebaseAuth.DefaultInstance;
                SignInAnonymously();
            }
            else
            {
                Debug.LogError("Error checking Firebase dependencies: " + task.Exception);
            }
        });
    }

    private void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            // Extraer el resultado real del tipo AuthResult
            AuthResult authResult = task.Result;
            user = authResult.User;

            if (user != null)
            {
                Debug.Log("User signed in successfully: " + user.UserId);
                LoadUsuarios();
            }
            else
            {
                Debug.LogError("User is null after signing in.");
            }
        });
    }

    public void LoadUsuarios()
    {
        dbReference.Child("Usuarios").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error loading users: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    var usuario = JsonConvert.DeserializeObject<UsuarioBase>(userSnapshot.GetRawJsonValue());
                    usuariosList.Add(usuario);
                }

                progresoVista.OnUsuariosLoaded(usuariosList);

                // Notify Rol script
                if (rolScript != null)
                {
                    rolScript.OnUsuariosLoaded(usuariosList);
                }
                //else
                //{
                //    Debug.LogError("Rol script reference is not set in FirebaseManager.");
                //}
            }
        });
    }

    public void CambiarRolUsuario(string usuarioID, int nuevoRol)
    {
        Debug.Log($"Intentando cambiar el rol del usuario: {usuarioID} a {nuevoRol}");

        dbReference.Child("Usuarios").Child(usuarioID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error loading users: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string jsonData = snapshot.GetRawJsonValue();
                UsuarioBase usuario = JsonConvert.DeserializeObject<UsuarioBase>(jsonData); //guardo el resultado como usuario base para lo siguiente

                if (usuario.Rol == nuevoRol) return; //Si el nuevo rol es igual al que ya tiene se cancela

                string newCode = UniqueCodeGenerator.GenerateCode(usuarioID);
                //De hecho el codigo no cambia ya que se genera en base al usuarioID,
                //pero como usuarioBase no tiene acceso al codigo, porque no todos los usuarios lo tienen, lo genero de nuevo

                if (nuevoRol == 0)
                {
                    Estudiante newEstudiante = new Estudiante(usuario.Nombre, usuario.Apellido1, usuario.Apellido2, usuario.Password, newCode); //vuelvo a crear el usuario pero como estudiante
                    string json = JsonConvert.SerializeObject(newEstudiante); //lo serializo
                    dbReference.Child("Usuarios").Child(usuarioID).SetRawJsonValueAsync(json); //lo guardo
                }
                else if (nuevoRol == 1)
                {
                    //por decidirse
                }
                else if (nuevoRol == 2)
                {
                    Maestro newMaestro = new Maestro(usuario.Nombre, usuario.Apellido1, usuario.Apellido2, usuario.Password, newCode); //vuelvo a crear el usuario pero como maestro
                    string json = JsonConvert.SerializeObject(newMaestro); //lo serializo
                    dbReference.Child("Usuarios").Child(usuarioID).SetRawJsonValueAsync(json); //lo guardo
                }
            }
        });

        StartCoroutine(ReiniciarEscena());
    }

    public void buscarUsuario(string usuarioID)
    {
        UsuarioBase objUsuario = null;  // Mover la inicialización aquí
        dbReference.Child("Usuarios").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error loading users: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                usuariosList.Clear();  // Limpiar la lista antes de llenarla

                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    var usuario = JsonConvert.DeserializeObject<UsuarioBase>(userSnapshot.GetRawJsonValue());
                    usuario.Nombre = userSnapshot.Key; // Set the key as the username

                    if ((usuario.Nombre + usuario.Apellido1 + usuario.Apellido2) == usuarioID)
                    {
                        objUsuario = usuario;  // Asignar el usuario encontrado
                    }

                    usuariosList.Add(usuario);
                }

                if (objUsuario != null)
                {
                    // Hacer algo con objUsuario si es necesario
                    Debug.Log("Usuario encontrado: " + objUsuario.Nombre);
                }
                else
                {
                    Debug.LogWarning("Usuario no encontrado.");
                }

                // Notify Rol script
                if (rolScript != null)
                {
                    rolScript.OnUsuariosLoaded(usuariosList);
                }
                else
                {
                    Debug.LogError("Rol script reference is not set in FirebaseManager.");
                }
            }
        });
    }

    public void EliminarUsuario(string usuarioID)
    {
        Debug.Log($"Intentando eliminar el usuario: {usuarioID} ");
        DatabaseReference usuarioRef = dbReference.Child("Usuarios").Child(usuarioID);

        //Tambien hay que borrar el codigo si es que tiene
        //Como el codigo se genera en base al usuarioId en lugar de buscarlo
        //puedo volver a generarlo
        string userCode = UniqueCodeGenerator.GenerateCode(usuarioID);

        //Si no hay codigo porque no todos los usuarios tienen (los padres) simplemente este no hará nada
        dbReference.Child("Codigos").Child(userCode).RemoveValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al eliminar el codigo del usuario: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Codigo del Usuario eliminado exitosamente.");
            }
        });

        //Se borra el usuario
        usuarioRef.RemoveValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al eliminar el usuario: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Usuario eliminado exitosamente.");
            }
        });

        StartCoroutine(ReiniciarEscena());
    }

    //Esto es porque al  hacer un cambio no se nota en la escena
    //con esto se reiniciar la escena luego de unos segunos
    //reflejadno los cambios
    private IEnumerator ReiniciarEscena()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GuardarProgresoUsuario(int valorActual, List<Nivel> nivelesCompletados, int dificultadActual)
    {
        var progresoData = new
        {
            valorActual = valorActual,
            nivelesCompletados = nivelesCompletados,
            dificultadActual = dificultadActual
        };
        string userID = $"{GameManager.Instance.estudiante.Nombre}{GameManager.Instance.estudiante.Apellido1}{GameManager.Instance.estudiante.Apellido2}";
        string json = JsonConvert.SerializeObject(progresoData);
        dbReference.Child("Usuarios").Child(userID).Child("Progreso").SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Progreso guardado exitosamente.");
            }
            else
            {
                Debug.LogError("Error guardando el progreso: " + task.Exception);
            }
        });

    }
}
