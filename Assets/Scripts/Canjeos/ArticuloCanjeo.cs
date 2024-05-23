using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArticuloCanjeo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombreEstudiante;
    [SerializeField] private TextMeshProUGUI nombreRecompensa;

    private CanjeoData data;

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
        List<CanjeoData> CanjeoListTemp = CanjeosManager.LoadCanjeos();
        for (int i = 0; i < CanjeoListTemp.Count; i++)
        {
            if (CanjeoListTemp[i].Equals(data))
            {
                CanjeoListTemp.RemoveAt(i);
                CanjeosManager.SaveCanjeos(CanjeoListTemp);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                return;
            }
        }
        Debug.Log("Data no encontrada");
    }
}
