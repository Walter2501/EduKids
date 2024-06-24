using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgresoVista : MonoBehaviour
{
    [SerializeField] private GameObject prefabUsuario;
    [SerializeField] private Transform container;


    private List<UsuarioBase> usuariosList = new List<UsuarioBase>();
    // Start is called before the first frame update

    public FirebaseManager firebaseManager;

    void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        if (firebaseManager == null)
        {
            Debug.LogError("RolManager no encontrado en la escena.");
        }
        else
        {
            firebaseManager.LoadUsuarios(); // Cargar usuarios en el inicio
        }
    }

    public void OnUsuariosLoaded(List<UsuarioBase> usuarios)
    {
        usuariosList = usuarios;
        Debug.Log("Usuarios cargados: " + usuariosList.Count);

        DibujarUsuarios();
    }


    private void DibujarUsuarios()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        foreach (var usuario in usuariosList)
        {
            GameObject usuarioGO = Instantiate(prefabUsuario, container);
            //TextMeshProUGUI nombreText = usuarioGO.GetComponentInChildren<TextMeshProUGUI>();
            //nombreText.text = $"{usuario.Nombre} {usuario.Apellido1} {usuario.Apellido2}";

            //TextMeshProUGUI dificultad = usuarioGO.GetComponentInChildren<TextMeshProUGUI>();
            //dificultad.text = $"{usuario.Progreso.dificultad}";

            //TextMeshProUGUI completado = usuarioGO.GetComponentInChildren<TextMeshProUGUI>();
            //completado.text = $"{usuario.Progreso.niveles}";

            TextMeshProUGUI[] texts = usuarioGO.GetComponentsInChildren<TextMeshProUGUI>();

            // Asigna los textos a los componentes correctos
            foreach (var text in texts)
            {
                if (text.name == "NombreText")
                {
                    text.text = $"{usuario.Nombre} {usuario.Apellido1} {usuario.Apellido2}";
                }
                else if (text.name == "DificultadText")
                {
                    text.text = $"{usuario.Progreso.dificultad}";
                }
                else if (text.name == "CompletadoText")
                {
                    text.text = $"{usuario.Progreso.niveles}";
                }
            }

        }
    }

}
