using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class MarcadorManager : MonoBehaviour
{
    [SerializeField] private MarcadorItem prefabMarcador;
    [SerializeField] private GameObject panelCargando;
    [SerializeField] private GameObject panelMarcador;
    [SerializeField] private GameObject panelCodigo;
    [SerializeField] private TMP_InputField codigoText;
    [SerializeField] private Button botonVolver;
    [SerializeField] private Button botonDatos;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI textoAlerta;

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

    private void RemoveSpaces(string text)
    {
        // Reemplaza todos los espacios en el texto con una cadena vacía
        // Para que no pongan espacios al poner el codigo
        codigoText.text = text.Replace(" ", "");
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

    public void Volver()
    {
        GameManager.Instance.CambiarEscena("MenuEstudiante");
    }

    private IEnumerator GetListFromBDD()
    {
        var maestroServerData = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.maestroID).GetValueAsync();

        yield return new WaitUntil(predicate: () => maestroServerData.IsCompleted); //se espera a que termine la consulta

        if (maestroServerData.IsFaulted) //si hubo un error en la consulta cancela todo y manda error
        {
            Debug.LogError("Error al verificar el Code: " + maestroServerData.Exception);
            yield break;
        }

        DataSnapshot snapshotMaestro = maestroServerData.Result;

        string jsonMaestro = snapshotMaestro.GetRawJsonValue();

        Maestro maestro = JsonConvert.DeserializeObject<Maestro>(jsonMaestro);
        yield return null;

        List<string> estudiantesID = maestro.EstudiantesIDs;
        List<Marcador> marcadores = new List<Marcador>();

        for (int i = 0; i < estudiantesID.Count; i++)
        {
            var serverData = GameManager.Instance.database.Child("Usuarios").Child(estudiantesID[i]).GetValueAsync();

            yield return new WaitUntil(predicate: () => serverData.IsCompleted); //se espera a que termine la consulta

            if (serverData.IsFaulted) //si hubo un error en la consulta cancela todo y manda error
            {
                Debug.LogError("Error al verificar el Code: " + serverData.Exception);
                yield break;
            }

            DataSnapshot snapshot = serverData.Result;

            string json = snapshot.GetRawJsonValue();

            Estudiante estudiante = JsonConvert.DeserializeObject<Estudiante>(json);

            Marcador newMarcador = new Marcador($"{estudiante.Nombre} {estudiante.Apellido1}", estudiante.MeritosTotal);
            marcadores.Add(newMarcador);
        }

        DibujarMarcador(marcadores);
    }

    private void DibujarMarcador(List<Marcador> marcadores)
    {
        panelMarcador.SetActive(true);

        List<Marcador> ordenados = marcadores.OrderByDescending(estudiante => estudiante.meritosTotal).ToList();

        foreach (Marcador estudiante in ordenados)
        {
            MarcadorItem newItem = Instantiate(prefabMarcador, container);
            newItem.SetText(estudiante);
        }

        panelCargando.SetActive(false);
    }
}


[Serializable]
public class Marcador
{
    public string nombreCompleto;
    public int meritosTotal;

    public Marcador(string nombreCompleto, int meritosTotal)
    {
        this.nombreCompleto = nombreCompleto;
        this.meritosTotal = meritosTotal;
    }
}