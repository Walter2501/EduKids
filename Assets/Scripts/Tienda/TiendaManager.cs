using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TiendaManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabArticulo;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI meritos;

    private List<RecompensaData> recompensasList => RecompensasManager.LoadRecompensas();

    private void Start()
    {
        ActualizarMeritos();
        DibujarArticulos();
    }

    private void ActualizarMeritos()
    {
        meritos.text = $"Méritos: {GameManager.Instance.cantidadMeritos}";
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
    }

    public void Volver(string escena)
    {
        GameManager.Instance.CambiarEscena(escena);
    }
}
