using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class RecompensasManager
{
    private const string RecompensasDataKey = "RecompensasData";

    public static void SaveRecompensas(List<RecompensaData> recompensasToSave)
    {
        List<RecompensaData> recompensasList = new List<RecompensaData>();
        foreach (RecompensaData obj in recompensasToSave)
        {
            recompensasList.Add(obj);
        }

        string json = JsonConvert.SerializeObject(recompensasList);
        PlayerPrefs.SetString(RecompensasDataKey, json);
        PlayerPrefs.Save();
    }

    public static List<RecompensaData> LoadRecompensas()
    {
        string json = PlayerPrefs.GetString(RecompensasDataKey, "");
        if (!string.IsNullOrEmpty(json))
        {
            return JsonConvert.DeserializeObject<List<RecompensaData>>(json);
        }
        return new List<RecompensaData>();
    }
}