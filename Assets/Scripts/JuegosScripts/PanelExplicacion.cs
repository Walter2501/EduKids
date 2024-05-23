using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelExplicacion : MonoBehaviour
{
    [SerializeField] private GameObject panelExplicacion;

    public void Entendido()
    {
        panelExplicacion.SetActive(false);
    }
}
