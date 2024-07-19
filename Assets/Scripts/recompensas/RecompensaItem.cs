using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecompensaItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombreText;
    [SerializeField] private TextMeshProUGUI costoText;
    private RecompensaManager panel;


    private void Start()
    {
        panel = GameObject.Find("Canvas").GetComponent<RecompensaManager>();
    }

    public void SetDatos(string nombre, string costo)
    {
        nombreText.text = nombre;
        costoText.text = costo;
    }

    public void EliminarRecompensa(RecompensaItem obj)
    {
        if (panel.Cargando) return;

        panel.EliminarRecompensa(obj);
    }
}
