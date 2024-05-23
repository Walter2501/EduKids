using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class recompensas : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform container;

    private List<RecompensaData> recompensasList => RecompensasManager.LoadRecompensas();
    private List<GameObject> recompensasObj = new List<GameObject>();

    public TMP_InputField inputField, inputField1;

    private void Start()
    {
        recompensasObj.Clear();
        DibujarRecompensas();
        Debug.Log(recompensasList.Count);
    }

    private void DibujarRecompensas()
    {
        if (recompensasList.Count == 0) return;
        for (int i = 0; i < recompensasList.Count; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, container);
            recompensasObj.Add(newItem);
            TextMeshProUGUI nombreText = newItem.transform.Find("Fondo_nombre").GetComponentInChildren<TextMeshProUGUI>();
            nombreText.text = recompensasList[i].nombre;

            TextMeshProUGUI costoText = newItem.transform.Find("Fondo_cantidad").GetComponentInChildren<TextMeshProUGUI>();
            costoText.text = recompensasList[i].cantidad.ToString();
        }
    }

    public void AsignarTextoDesdeCampo()
    {
        string textoIngresado = inputField.text;
        string textoIngresado2 = inputField1.text;
        AddItem(textoIngresado, textoIngresado2);
    }

    public void AddItem(string value, string value2)
    {
        GameObject newItem = Instantiate(itemPrefab, container);
        RecompensaData newReward = new RecompensaData(value, int.Parse(value2));

        TextMeshProUGUI nombreText= newItem.transform.Find("Fondo_nombre").GetComponentInChildren<TextMeshProUGUI>();
        nombreText.text = value;

        TextMeshProUGUI costoText = newItem.transform.Find("Fondo_cantidad").GetComponentInChildren<TextMeshProUGUI>();
        costoText.text = value2;

        AñadirRecompensa(newReward);
        recompensasObj.Add(newItem);
    }

    private void AñadirRecompensa(RecompensaData newReward)
    {
        List<RecompensaData> RecompensasListTemp = recompensasList;

        RecompensasListTemp.Add(newReward);
        RecompensasManager.SaveRecompensas(RecompensasListTemp);
    }

    public bool EliminarRecompensa(GameObject obj)
    {
        for (int i = 0; i < recompensasObj.Count; i++)
        {
            if (obj == recompensasObj[i])
            {
                recompensasObj.Remove(obj);
                List<RecompensaData> RecompensasListTemp = recompensasList;
                RecompensaData elementoEliminar = RecompensasListTemp[i];
                RecompensasListTemp.Remove(elementoEliminar);
                RecompensasManager.SaveRecompensas(RecompensasListTemp);
                return true;
            }
        }
        return false;
    }

    public void Volver(string escena)
    {
        GameManager.Instance.CambiarEscena(escena);
    }
}
