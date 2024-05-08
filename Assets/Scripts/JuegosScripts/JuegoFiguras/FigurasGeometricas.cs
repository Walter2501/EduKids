using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "figuraGeo", menuName = "SO/figura")]
public class FigurasGeometricas : ScriptableObject
{
    [SerializeField] private string lados;
    [SerializeField] private Sprite figuraImg;

    public string Lados
    {
        get => lados;
    }

    public Sprite FiguraImg
    {
        get => figuraImg;
    }
}