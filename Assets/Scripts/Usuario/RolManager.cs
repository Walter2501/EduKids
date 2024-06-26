using System.Collections.Generic;
using UnityEngine;

public class RolManager : MonoBehaviour
{
    public FirebaseManager firebaseManager;
    public Rol rolScript;
    //public ProgresoVista progresoVista;



    private void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManager no encontrado en la escena.");
        }
        else
        {
            // Configurar la referencia al script Rol en FirebaseManager
            firebaseManager.RolScript = FindObjectOfType<Rol>();
        }

        if (rolScript == null)
        {
            rolScript = FindObjectOfType<Rol>();
        }
    }

    //public void OnUsuariosLoaded(List<UsuarioBase> usuarios)
    //{
    //    usuariosList = usuarios;
    //    rolScript.OnUsuariosLoaded(usuarios); // Pasar los usuarios cargados a la UI
    //    //progresoVista.OnUsuariosLoaded(usuarios);
    //}

    public void CambiarRolUsuario(string usuarioID, int nuevoRol)
    {
        if (firebaseManager != null)
        {
            firebaseManager.CambiarRolUsuario(usuarioID, nuevoRol);
        }
        else
        {
            Debug.LogError("FirebaseManager es nulo.");
        }
    }

    public void EliminarUsuario(string usuarioID)
    {
        if (firebaseManager != null)
        {
            firebaseManager.EliminarUsuario(usuarioID);
        }
        else
        {
            Debug.LogError("FirebaseManager es nulo.");
        }
    }
}
