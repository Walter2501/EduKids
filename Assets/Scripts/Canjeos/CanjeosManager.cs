using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CanjeosManager
{
    private const string CanjeosDataKey = "CanjeosData";

    public static void SaveCanjeos(List<CanjeoData> canjeosToSave)
    {
        List<CanjeoData> canjeosList = new List<CanjeoData>();
        foreach (CanjeoData obj in canjeosToSave)
        {
            canjeosList.Add(obj);
        }

        string json = JsonConvert.SerializeObject(canjeosList);
        PlayerPrefs.SetString(CanjeosDataKey, json);
        PlayerPrefs.Save();
    }

    public static List<CanjeoData> LoadCanjeos()
    {
        string json = PlayerPrefs.GetString(CanjeosDataKey, "");
        if (!string.IsNullOrEmpty(json))
        {
            return JsonConvert.DeserializeObject<List<CanjeoData>>(json);
        }
        return new List<CanjeoData>();
    }
}
