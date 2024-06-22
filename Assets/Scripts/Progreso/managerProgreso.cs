using UnityEngine;

public class managerProgreso : MonoBehaviour
{
    public ProgresoUser progresoUser;
    public FirebaseManager firebaseManager;

    private void Awake()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        if (firebaseManager == null)
        {
            Debug.LogError("FirebaseManager no encontrado en la escena.");
        }

        progresoUser = FindObjectOfType<ProgresoUser>();
        if (progresoUser == null)
        {
            Debug.LogError("ProgresoUser no encontrado en la escena.");
        }
    }

    public void CompletarNivel(string nombreNivel)
    {
        Nivel nivel = new Nivel { nombre = nombreNivel, dificultad = progresoUser.getDificultad() };
        progresoUser.AgregarNivel(nivel);
        progresoUser.GuardarProgreso();

    }

    public void AumentarDificultad()
    {
        progresoUser.SubirDificultad();
        progresoUser.GuardarProgreso();
    }
}
