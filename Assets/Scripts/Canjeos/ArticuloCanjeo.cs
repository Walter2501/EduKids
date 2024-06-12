using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArticuloCanjeo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombreEstudiante;
    [SerializeField] private TextMeshProUGUI nombreRecompensa;

    private CanjeoData data;
    private CanjeosManager manager;

    private void Start()
    {
        manager = GameObject.Find("Canvas").GetComponent<CanjeosManager>();
    }

    public void SetData(CanjeoData dataInfo)
    {
        data = dataInfo;
    }

    public void DibujarInfo()
    {
        nombreEstudiante.text = data.nombreEstudiante;
        nombreRecompensa.text = data.nombre;
    }

    public void CanjeoCompletado()
    {
        if (manager.Cargando) return;
        List<CanjeoData> CanjeoListTemp = GameManager.Instance.maestro.Canjeos;
        for (int i = 0; i < CanjeoListTemp.Count; i++)
        {
            if (CanjeoListTemp[i].Equals(data))
            {
                CanjeoListTemp.RemoveAt(i);
                manager.GetNewList(CanjeoListTemp);
                return;
            }
        }
        Debug.Log("Data no encontrada");
    }
}
