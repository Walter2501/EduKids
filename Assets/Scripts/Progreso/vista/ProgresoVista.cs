using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgresoVista : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject prefabUsuario;
    [SerializeField] private Transform container;

    private ProgresoVistaManager progresoVistaManager;

    private List<UsuarioBase> usuariosList = new List<UsuarioBase>();
    // Start is called before the first frame update


    void Start()
    {
        progresoVistaManager = FindObjectOfType<ProgresoVistaManager>();
        if (progresoVistaManager == null)
        {
            Debug.LogError("ProgresoVistaManager no encontrado en la escena.");
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
            Debug.Log(JsonUtility.ToJson(usuario.Progreso));  // Para ver la estructura completa de Progreso

            GameObject usuarioGO = Instantiate(prefabUsuario, container);


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
                    if(usuario.Progreso.dificultadActual <= 1)
                    {
                        text.text = $"Dificultad Actual: Facil";
                    } else if(usuario.Progreso.dificultadActual == 2)
                    {
                        text.text = $"Dificultad Actual: Media";
                    }
                    else
                    {
                        text.text = $"Dificultad Actual: Dificil";
                    }
                }
                else if (text.name == "CompletadoText")
                {
                  //  Debug.Log(usuario.Progreso.nivelesCompletados);
                    //Debug.Log(usuario.Progreso);

                    text.text = $"Niveles Completados:{usuario.Progreso.nivelesCompletados.Count}";
                }
            }

        }
    }

}
