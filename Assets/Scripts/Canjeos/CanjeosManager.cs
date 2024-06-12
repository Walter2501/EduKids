using Firebase.Database;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanjeosManager : MonoBehaviour 
{
    [SerializeField] private GameObject panelCargando;
    [SerializeField] private GameObject prefabCanjeo;
    [SerializeField] private Transform container;
    [SerializeField] private Button botonVolver;

    private List<CanjeoData> canjeoList = new List<CanjeoData>();
    private bool cargando = false;
    public bool Cargando => cargando;

    private void Start()
    {
        canjeoList = GameManager.Instance.maestro.Canjeos;
        DibujarArticulos();
        botonVolver.onClick.AddListener(Volver);
    }

    private void DibujarArticulos()
    {
        if (canjeoList.Count == 0) return;
        for (int i = 0; i < canjeoList.Count; i++)
        {
            GameObject newObj = Instantiate(prefabCanjeo, container);
            ArticuloCanjeo newArticulo = newObj.GetComponent<ArticuloCanjeo>();
            newArticulo.SetData(canjeoList[i]);
            newArticulo.DibujarInfo();
        }
    }

    public void Volver()
    {
        GameManager.Instance.CambiarEscena("MenuProfesor");
    }

    public void GetNewList(List<CanjeoData> canjeosToSave)
    {
        cargando = true;
        StartCoroutine(ActualizarListaCanjeos(canjeosToSave));
    } 

    private IEnumerator ActualizarListaCanjeos(List<CanjeoData> canjeosToSave)
    {
        GameManager.Instance.maestro.Canjeos = canjeosToSave;

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
            string json = JsonConvert.SerializeObject(GameManager.Instance.maestro);
            var newDataToSave = GameManager.Instance.database.Child("Usuarios").Child(GameManager.Instance.usuarioID).SetRawJsonValueAsync(json);

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
