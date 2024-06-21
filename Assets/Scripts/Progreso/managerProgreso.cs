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
            Debug.LogError("FirebaseManager not found in scene!");
        }
    }

    public void CompletarNivel(string nombreNivel)
    {
        Nivel nivel = new Nivel { nombre = nombreNivel, dificultad = progresoUser.getDificultad() };
        progresoUser.AgregarNivel(nivel);
    }

    public void AumentarDificultad()
    {
        progresoUser.SubirDificultad();
    }
}
