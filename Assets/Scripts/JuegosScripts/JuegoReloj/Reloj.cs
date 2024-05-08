using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "reloj", menuName = "SO/reloj")]
public class Reloj : ScriptableObject
{
    [SerializeField] private string hora;
    [SerializeField] private Sprite relojImg;

    public string Hora
    {
        get => hora;
    }

    public Sprite RelojImg
    {
        get => relojImg;
    }
}