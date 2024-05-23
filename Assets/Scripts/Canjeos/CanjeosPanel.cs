using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanjeosPanel : MonoBehaviour
{
    [SerializeField] private GameObject prefabCanjeo;
    [SerializeField] private Transform container;

    private List<CanjeoData> canjeoList => CanjeosManager.LoadCanjeos();

    private void Start()
    {
        DibujarArticulos();
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

    public void Volver(string escena)
    {
        GameManager.Instance.CambiarEscena(escena);
    }
}
