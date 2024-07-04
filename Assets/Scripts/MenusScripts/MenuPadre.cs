using Firebase.Database;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPadre : MonoBehaviour
{
    [SerializeField] private GameObject panelCargando;
    [SerializeField] private GameObject panelCodigo;

    [Header("Progreso")]
    [SerializeField] private List<Image> imagenes;
    [SerializeField] private TextMeshProUGUI textoTitulo;
    [SerializeField] private ProgresoItemPrefab prefabNivelCompletado;
    [SerializeField] private Transform container;
    [SerializeField] private Sprite desbloqueado;
    [SerializeField] private Sprite noDesbloqueado;

    [Header("Codigo")]
    [SerializeField] private TMP_InputField codigoText;
    [SerializeField] private Button botonDatos;
    [SerializeField] private TextMeshProUGUI textoAlerta;

    private List<NivelCompletado> nivelesList = new List<NivelCompletado>();

    private void Start()
    {
        if (CheckIfCodeSaved())
        {
            StartCoroutine(GetEstudiante());
        }
        else
        {
            botonDatos.onClick.AddListener(GetInputCode);
            codigoText.onValueChanged.AddListener(RemoveSpaces);
            panelCargando.SetActive(false);
        }
    }

    private void GetInputCode() //recibe el codigo escrito en el input
    {
        if (codigoText.text.Length != 8) //si el codigo recibido no tiene exactamente 8 digitos manda la alerta en un texto y cancela el resto
        {
            textoAlerta.text = "Ingrese 8 digitos";
            return;
        }
        //si lo anterior no se cumple hara lo siguiente:
        panelCargando.SetActive(true); //activa el cargando mientras revisa la bdd

        StartCoroutine(SearchingCode());
    }

    private void VerProgresoHijo()
    {
        nivelesList = GameManager.Instance.estudiante.nivelesCompletados;
        textoTitulo.text = $"Progreso {GameManager.Instance.estudiante.Nombre} {GameManager.Instance.estudiante.Apellido1}";
        ChooseImages();
        DibujarNivelesCompletados();
    }

    private void ChooseImages()
    {
        for (int i = 0; i < imagenes.Count; i++)
        {
            if (GameManager.Instance.estudiante.progreso.GetDificultadMax(i + 1) < 3)
            {
                imagenes[i].sprite = noDesbloqueado;
            }
            else
            {
                imagenes[i].sprite = desbloqueado;
            }
        }
    }

    private void DibujarNivelesCompletados()
    {
        if (nivelesList.Count == 0) return;
        for (int i = nivelesList.Count - 1; i >= 0; i--)
        {
            ProgresoItemPrefab newItem = Instantiate(prefabNivelCompletado, container);
            newItem.SetTexts(nivelesList[i].nombre, nivelesList[i].dificultad, nivelesList[i].respuestasCorrectas);
        }
        panelCargando.SetActive(false);
    }

    private bool CheckIfCodeSaved() //Revisa si el padre tiene el userID del hijo guardado
    {
        if (GameManager.Instance.estudianteID != "")
        {
            return true; //si lo hay regresa verdadero
        }
        return false; //si no hay regresa falso
    }

    private IEnumerator SearchingCode()
    {
        string codigoToCheck = codigoText.text.ToUpper(); //guardo una copia del codigo por si acaso

        var serverData = GameManager.Instance.database.Child("Codigos").Child(codigoToCheck).GetValueAsync();

        yield return new WaitUntil(predicate: () => serverData.IsCompleted); //se espera a que termine la consulta

        if (serverData.IsFaulted) //si hubo un error en la consulta cancela todo y manda error
        {
            Debug.LogError("Error al verificar el Code: " + serverData.Exception);
            yield break;
        }

        DataSnapshot snapshot = serverData.Result;

        if (!snapshot.Exists) //si no encontro codigo manda mensaje y vuelve al panel para ingresar codigo
        {
            Debug.Log("Codigo no encontrado");
            panelCargando.SetActive(false);
            textoAlerta.text = "Codigo incorrecto";
            yield break;
        }

        string jsonData = snapshot.GetRawJsonValue();

        //se guarda el archivo de forma temporal para conseguir el id dentro
        CodigosUsuario codigo = JsonConvert.DeserializeObject<CodigosUsuario>(jsonData);

        GameManager.Instance.SetEstudianteUserID(codigo.UserID); //se guarda el ID del profesor en el GameManager

        StartCoroutine(GetEstudiante());
    }

    private IEnumerator GetEstudiante()
    {
        panelCodigo.SetActive(false);
        var serverData = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.estudianteID).GetValueAsync(); //hace la consulta para conseguir al usuario
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
            
            //debido a que si se llego hasta aquí habia un userID que ya no existe en la base de datos
            //se borra todo lo que se guardaba de forma local para evitar errores
            yield break; //Se cancela el resto de la corrutina
        }
        yield return null;

        Debug.Log("Usuario Encontrado");

        string jsonData = snapshot.GetRawJsonValue();

        GameManager.Instance.estudiante = JsonConvert.DeserializeObject<Estudiante>(jsonData);

        VerProgresoHijo();
    }

    private void RemoveSpaces(string text)
    {
        // Reemplaza todos los espacios en el texto con una cadena vacía
        // Para que no pongan espacios al poner el codigo
        codigoText.text = text.Replace(" ", "");
    }
}