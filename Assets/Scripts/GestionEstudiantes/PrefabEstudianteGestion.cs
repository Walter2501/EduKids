using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Firebase.Database;
using Newtonsoft.Json;

public class PrefabEstudianteGestion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombreText;
    private GameObject cargando;
    private Button seeProgress;
    private string nombre;
    private string userID;

    private void Awake()
    {
        seeProgress = GetComponent<Button>();
        cargando = GameObject.Find("PanelCargando");
    }

    private void Start()
    {
        seeProgress.onClick.AddListener(IrAlProgreso);
    }

    public void SetName(string estudianteID) //consigo el userID
    {
        userID = estudianteID;
        nombre = SplitCamelCase(estudianteID); //manda a separar el id
        nombreText.text = $"{nombre}"; //asigna el nombre en el texto
    }

    private string SplitCamelCase(string input) //Pone separaciones antes de una mayuscula
    {
        //Ejem:
        //JohnDoeDoe que sería el userID se vuelve
        //John Doe Doe
        return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
    }

    private void IrAlProgreso()
    {
        cargando.SetActive(true);
        StartCoroutine(CargarEstudiante());
    }

    private IEnumerator CargarEstudiante()
    {
        var serverData = GameManager.Instance.database.Child("Usuarios").Child(userID).GetValueAsync(); //hace la consulta para conseguir al usuario
        yield return new WaitUntil(predicate: () => serverData.IsCompleted); //espera a que se complete la consulta

        if (serverData.IsFaulted)
        {
            Debug.LogError("Error al verificar el ID: " + serverData.Exception);
            yield break; // Salir de la corrutina si hay un error
        }

        DataSnapshot snapshot = serverData.Result;

        if (!snapshot.Exists) //comprueba si el usuario existe en la base de datos, si no existe pasa a la escena para registrarse y logearse
        {
            //Esto es en caso se borre un usuario en la bdd por cualquier razón
            Debug.Log("El usuario guardado no existe en la BDD");
            cargando.SetActive(false);
            //debido a que si se llego hasta aquí habia un userID que ya no existe en la base de datos
            //se borra todo lo que se guardaba de forma local para evitar errores
            yield break; //Se cancela el resto de la corrutina
        }
        yield return null;

        Debug.Log("Usuario Encontrado");

        string jsonData = snapshot.GetRawJsonValue();

        GameManager.Instance.estudiante = JsonConvert.DeserializeObject<Estudiante>(jsonData);
        GameManager.Instance.CambiarEscena("ProgresoVistaProfesor");
    }
}