using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArticuloTienda : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombreText;
    [SerializeField] private TextMeshProUGUI costoText;

    private RecompensaData data = null;

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
        int meritosActuales = GameManager.Instance.cantidadMeritos;
        if (meritosActuales - data.cantidad < 0) return;
        GameManager.Instance.AddMeritos(-data.cantidad);
        AñadirCanjeo(data);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void AñadirCanjeo(RecompensaData newCanjeo)
    {
        List<CanjeoData> CanjeoListTemp = CanjeosManager.LoadCanjeos();
        CanjeoData canje = new CanjeoData($"{GameManager.Instance.nombre} {GameManager.Instance.apellido}", data.nombre);
        CanjeoListTemp.Add(canje);
        CanjeosManager.SaveCanjeos(CanjeoListTemp);
    }
}