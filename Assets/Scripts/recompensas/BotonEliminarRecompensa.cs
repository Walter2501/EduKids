using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonEliminarRecompensa : MonoBehaviour
{
    public RecompensaManager panel;

    private void Start()
    {
        panel = GameObject.Find("Canvas").GetComponent<RecompensaManager>();
    }

    public void EliminarRecompensa(GameObject obj)
    {
        if (panel.Cargando) return;

        panel.EliminarRecompensa(obj);
    }
}