using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecompensaData
{
    public string nombre = "";
    public int cantidad  = 0;

    public RecompensaData(string nombre, int cantidad)
    {
        this.nombre = nombre;
        this.cantidad = cantidad;
    }
}
