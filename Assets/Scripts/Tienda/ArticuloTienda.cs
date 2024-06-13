using Firebase.Database;
using Google.MiniJSON;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArticuloTienda : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombreText;
    [SerializeField] private TextMeshProUGUI costoText;

    private TiendaManager manager = null;
    private RecompensaData data = null;

    private void Start()
    {
        manager = GameObject.Find("Canvas").GetComponent<TiendaManager>();
    }

    public void SetData(RecompensaData dataInfo)
    {
        data = dataInfo;
    }

    public void DibujarInfo()
    {
        nombreText.text = data.nombre;
        costoText.text = $"Costo: {data.cantidad}";
    }

    public void Canjear()
    {
        int meritosActuales = GameManager.Instance.estudiante.Meritos;
        if (meritosActuales - data.cantidad < 0) return;
        StartCoroutine(AñadirCanjeo(data)); //le envio la data de la recompensa
    }

    private IEnumerator AñadirCanjeo(RecompensaData dataDelCanjeo)
    {
        manager.CanjeandoRecompensa();

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

        List<CanjeoData> canjeosTemp = maestro.Canjeos;
        CanjeoData newCanjeo = new CanjeoData($"{GameManager.Instance.estudiante.Nombre} {GameManager.Instance.estudiante.Apellido1}", dataDelCanjeo.nombre);
        maestro.Canjeos.Add(newCanjeo);

        string json = JsonConvert.SerializeObject(maestro);
        var newDataToSave = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.maestroID).SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => newDataToSave.IsCompleted);

        if (newDataToSave.IsFaulted)
        {
            Debug.LogError("Error al guardar los nuevos datos: " + newDataToSave.Exception);
        }
        else
        {
            Debug.Log("Datos guardados correctamente");
            StartCoroutine(ActualizarMeritos());
        }
    }

    private IEnumerator ActualizarMeritos()
    {
        GameManager.Instance.estudiante.Meritos -= data.cantidad;

        var serverData = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.usuarioID).GetValueAsync(); //revisa si existe el usuario
        yield return new WaitUntil(predicate: () => serverData.IsCompleted); //se espera a que termine la consulta

        if (serverData.IsFaulted) //si hubo un error en la consulta cancela todo y manda error
        {
            Debug.LogError("Error al verificar el ID: " + serverData.Exception);
            yield break;
        }

        DataSnapshot snapshot = serverData.Result;

        if (snapshot.Exists) //Si la consulta regresa que ya existe el usuarioID en la bdd procede a guardar los datos actualizados
        {
            string json = JsonConvert.SerializeObject(GameManager.Instance.estudiante);
            var newDataToSave = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.usuarioID).SetRawJsonValueAsync(json);

            yield return new WaitUntil(() => newDataToSave.IsCompleted);

            if (newDataToSave.IsFaulted)
            {
                Debug.LogError("Error al guardar los nuevos datos: " + newDataToSave.Exception);
            }
            else
            {
                Debug.Log("Datos guardados correctamente");
                GameManager.Instance.ReiniciarEscenaActual();
            }
        }
        else //Si por alguna razón ya no existe el usuario sale de la app
        {
            Application.Quit(); //esto solo funciona el el built, no en el editor
        }
    }
}