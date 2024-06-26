using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgresoVistaManager : MonoBehaviour
{

    public ProgresoVista progresoVista;
    public FirebaseManager firebaseManager;

    void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManager no encontrado en la escena.");
        }
        else
        {
            // Configurar la referencia al script Rol en FirebaseManager
            firebaseManager.ProgresoVista = FindObjectOfType<ProgresoVista>();
        }

        if (progresoVista == null)
        {
            progresoVista = FindObjectOfType<ProgresoVista>();
        }
    }

}
