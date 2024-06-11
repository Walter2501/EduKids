using Firebase.Database;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecompensaManager : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private GameObject panelCargando;
    [SerializeField] public TMP_InputField inputField, inputField1;
    [Header("Botones")]
    [SerializeField] private Button botonCrearRecompensa;
    [SerializeField] private Button botonVolver;

    private List<RecompensaData> recompensasList;
    private List<GameObject> recompensasObj = new List<GameObject>();
    private bool cargando = false;
    public bool Cargando => cargando;


    private void Start()
    {
        cargando = false;
        recompensasList = GameManager.Instance.maestro.Recompensas; //consigo la lista del GameManager
        DibujarRecompensas(); //dibujo la lista
        //Asigno su funcionamiento a los botones
        botonCrearRecompensa.onClick.AddListener(AsignarTextoDesdeCampo);
        botonVolver.onClick.AddListener(Volver);
    }

    private void DibujarRecompensas()
    {
        if (recompensasList.Count == 0) return; //si no hay recompensas se cancelas
        for (int i = 0; i < recompensasList.Count; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, container); //crea el prefab
            recompensasObj.Add(newItem); //lo añade a la lista de

            //Configura sus datos
            TextMeshProUGUI nombreText = newItem.transform.Find("Fondo_nombre").GetComponentInChildren<TextMeshProUGUI>();
            nombreText.text = recompensasList[i].nombre;
            TextMeshProUGUI costoText = newItem.transform.Find("Fondo_cantidad").GetComponentInChildren<TextMeshProUGUI>();
            costoText.text = recompensasList[i].cantidad.ToString();
        }
    }

    public void AsignarTextoDesdeCampo() //guarda los datos y los manda a la funcion añadir item
    {
        if (cargando) return; //si ya se esta haciendo algo cancela esto
        cargando = true; //cargando se vuelve true porque se esta actulizando la lista
        panelCargando.SetActive(true); //se activa el panel cargando, esto es solo visual
        string textoIngresado = inputField.text;
        string textoIngresado2 = inputField1.text;
        if (textoIngresado == "" || textoIngresado2 == "") //si esta vacio no hace nada
        {
            Debug.Log("Ingrese los datos");
            return;
        }
        RecompensaData newReward = new RecompensaData(textoIngresado, int.Parse(textoIngresado2)); //crea los datos de la nueva recompensa
        AñadirRecompensa(newReward);
    }

    private void AñadirRecompensa(RecompensaData newReward)
    {
        List<RecompensaData> recompensasListTemp = recompensasList; //crea una copia de la lista

        recompensasListTemp.Add(newReward); //le añade a la copia la nueva recompensa

        StartCoroutine(ActualizarListaRecompensas(recompensasListTemp));
    }

    public void EliminarRecompensa(GameObject obj)
    {
        bool finded = false;
        cargando = true;
        panelCargando.SetActive(true); //se activa el panel cargando
        for (int i = 0; i < recompensasObj.Count; i++)
        {
            if (obj == recompensasObj[i])
            {
                finded = true;
                List<RecompensaData> recompensasListTemp = recompensasList;
                RecompensaData elementoEliminar = recompensasListTemp[i];
                recompensasListTemp.Remove(elementoEliminar);
                StartCoroutine(ActualizarListaRecompensas(recompensasListTemp));
            }
        }
        if (!finded) //si no se encontro el elemento se desactiva el cargando
        {
            cargando = false;
            panelCargando.SetActive(false);
        }
    }

    private IEnumerator ActualizarListaRecompensas(List<RecompensaData> newList) //corrutina que actualiza la lista, se le pasa una nueva
    {
        GameManager.Instance.maestro.Recompensas = newList; //guardo la nueva lista en el maestro del gamemanager

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

    public void Volver()
    {
        if (cargando) return;
        GameManager.Instance.CambiarEscena("MenuProfesor");
    }
}
