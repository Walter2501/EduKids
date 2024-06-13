using Firebase.Database;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TiendaManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabArticulo;
    [SerializeField] private GameObject panelCargando;
    [SerializeField] private GameObject panelRecompensas;
    [SerializeField] private GameObject panelCodigo;
    [SerializeField] private TMP_InputField codigoText;
    [SerializeField] private Button botonVolver;
    [SerializeField] private Button botonDatos;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI meritos;
    [SerializeField] private TextMeshProUGUI textoAlerta;

    private List<RecompensaData> recompensasList;

    private void Start()
    {
        botonVolver.onClick.AddListener(Volver);
        if (!CheckIfCodeSaved())
        {
            panelCodigo.SetActive(true); //si no hay codigo guardado muestra un panel para introducirlo
            panelCargando.SetActive(false);

            botonDatos.onClick.AddListener(GetInputCode);
            codigoText.onValueChanged.AddListener(RemoveSpaces);
        }
        else //si hay codigo simplemente consigue las recompensa y las dibuja;
        {
            StartCoroutine(GetListFromBDD()); //Consigue la lista de la bdd
        }
    }

    private bool CheckIfCodeSaved() //Revisa si el estudiante tiene el userID del profe guardado
    {
        if (GameManager.Instance.maestroID != "")
        {
            return true; //si lo hay regresa verdadero
        }
        return false; //si no hay regresa falso
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

        GameManager.Instance.SetProfesorUserID(codigo.UserID); //se guarda el ID del profesor en el GameManager

        yield return StartCoroutine(AddingUserIDToTheMaestroList());

        GameManager.Instance.ReiniciarEscenaActual();
    }

    private IEnumerator AddingUserIDToTheMaestroList()
    {
         //Ahora entramos al maestro en bdd y le añadimos a su lista string de estudiantesID el id de esta cuenta
        var maestroServerData = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.maestroID).GetValueAsync();

        yield return new WaitUntil(predicate: () => maestroServerData.IsCompleted); //se espera a que termine la consulta

        if (maestroServerData.IsFaulted) //si hubo un error en la consulta cancela todo y manda error
        {
            Debug.LogError("Error al verificar el Code: " + maestroServerData.Exception);
            yield break;
        }

        DataSnapshot snapshot2 = maestroServerData.Result;

        string json = snapshot2.GetRawJsonValue();

        Maestro maestro = JsonConvert.DeserializeObject<Maestro>(json);
        maestro.EstudiantesIDs.Add(GameManager.Instance.usuarioID);
        json = JsonConvert.SerializeObject(maestro);

        var newDataToSave = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.maestroID).SetRawJsonValueAsync(json);
        yield return new WaitUntil(() => newDataToSave.IsCompleted);

        if (newDataToSave.IsFaulted)
        {
            Debug.LogError("Error al guardar los nuevos datos: " + newDataToSave.Exception);
            yield break;
        }
        else
        {
            Debug.Log("Datos guardados correctamente");
        }
    }

    private IEnumerator GetListFromBDD()
    {
        var serverData = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.maestroID).GetValueAsync(); //hace la consulta para conseguir al usuario
        yield return new WaitUntil(predicate: () => serverData.IsCompleted); //espera a que se complete la consulta

        if (serverData.IsFaulted)
        {
            Debug.LogError("Error al verificar el ID: " + serverData.Exception);
            yield break; // Salir de la corrutina si hay un error
        }

        DataSnapshot snapshot = serverData.Result;

        if (!snapshot.Exists) //Si no existe, porque el profe se elimino o algo, borra userID del profe y cancela el resto
        {
            Debug.Log("No se encontro al maestro");
            GameManager.Instance.SetProfesorUserID("");
            GameManager.Instance.ReiniciarEscenaActual();
            yield break;
        }

        string jsonData = snapshot.GetRawJsonValue();

        Maestro maestro = JsonConvert.DeserializeObject<Maestro>(jsonData);
        yield return new WaitForSeconds(0.1f);
        panelRecompensas.SetActive(true);
        recompensasList = maestro.Recompensas;
        meritos.text = $"Méritos: {GameManager.Instance.estudiante.Meritos}";
        DibujarArticulos();
    }

    private void DibujarArticulos()
    {
        if (recompensasList.Count == 0) return;
        for (int i = 0; i < recompensasList.Count; i++)
        {
            GameObject newObj = Instantiate(prefabArticulo, container);
            ArticuloTienda newArticulo = newObj.GetComponent<ArticuloTienda>();
            newArticulo.SetData(recompensasList[i]);
            newArticulo.DibujarInfo();
        }
        panelCargando.SetActive(false);
    }


    private void RemoveSpaces(string text)
    {
        // Reemplaza todos los espacios en el texto con una cadena vacía
        // Para que no pongan espacios al poner el codigo
        codigoText.text = text.Replace(" ", "");
    }

    public void Volver()
    {
        GameManager.Instance.CambiarEscena("MenuEstudiante");
    }

    public void CanjeandoRecompensa() //Esta funcion la llaman los articulos de la tienda al ser canjeados
    {
        panelCargando.SetActive(true);
    }
}
