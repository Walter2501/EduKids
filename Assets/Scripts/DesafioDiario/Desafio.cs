using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desafio : MonoBehaviour
{

    public string nombre;
    public int recompensa;
    public bool condicion;

    public Desafio(string nombre, int recompensa, bool condicion = false)
    {
        this.nombre = nombre;
        this.recompensa = recompensa;
        this.condicion = condicion;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void guardar()
    {

    }
}
