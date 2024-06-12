using Firebase.Database;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference dbReference;
    private FirebaseAuth auth;
    private FirebaseUser user;

    public List<UsuarioBase> usuariosList = new List<UsuarioBase>();
    public Rol rolScript; // Reference to the Rol script

    public FirebaseAuth GetFirebaseAuth()
    {
        return auth;
    }

    private void Start()
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

    private void LoadUsuarios()
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
                    var usuario = JsonUtility.FromJson<UsuarioBase>(userSnapshot.GetRawJsonValue());
                    usuario.Nombre = userSnapshot.Key; // Set the key as the username
                    usuariosList.Add(usuario);
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

    // Method to change the role of a user
    public void CambiarRolUsuario(string nombreUsuario, int nuevoRol)
    {
        Debug.Log($"Intentando cambiar el rol del usuario: {nombreUsuario} a {nuevoRol}");

        DatabaseReference usuariosRef = dbReference.Child("Usuarios");
        usuariosRef.Child(nombreUsuario).Child("Rol").SetValueAsync(nuevoRol)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error al cambiar el rol del usuario: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Rol del usuario cambiado exitosamente.");
                }
            });
    }

    public void buscarUsuario(string nombreUsuario)
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
                    var usuario = JsonUtility.FromJson<UsuarioBase>(userSnapshot.GetRawJsonValue());
                    usuario.Nombre = userSnapshot.Key; // Set the key as the username

                    if ((usuario.Nombre + usuario.Apellido1 + usuario.Apellido2) == nombreUsuario)
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



public void ElimincarUsuario(string nombreUsuario)
    {

        Debug.Log($"Intentando eliminar el usuario: {nombreUsuario} ");
        DatabaseReference usuarioRef = dbReference.Child("Usuarios").Child(nombreUsuario);
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
    }
}
