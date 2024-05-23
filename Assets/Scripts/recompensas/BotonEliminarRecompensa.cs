using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonEliminarRecompensa : MonoBehaviour
{
    public recompensas panel;

    private void Start()
    {
        panel = GameObject.Find("Panel").GetComponent<recompensas>();
    }

    public void EliminarRecompensa(GameObject obj)
    {
        if (!panel.EliminarRecompensa(obj))
        {
            Debug.Log("No existe");
            return;
        }
        Destroy(obj);
    }
}