using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class RecompensasManager
{
    private const string GameObjectsDataKey = "GameObjectsData";

    public static void SaveGameObjects(List<GameObject> objectsToSave)
    {
        List<GameObjectData> dataList = new List<GameObjectData>();
        foreach (GameObject obj in objectsToSave)
        {
            dataList.Add(new GameObjectData(obj));
        }

        string json = JsonConvert.SerializeObject(dataList);
        PlayerPrefs.SetString(GameObjectsDataKey, json);
        PlayerPrefs.Save();
    }

    public static List<GameObjectData> LoadGameObjects()
    {
        string json = PlayerPrefs.GetString(GameObjectsDataKey, "");
        if (!string.IsNullOrEmpty(json))
        {
            return JsonConvert.DeserializeObject<List<GameObjectData>>(json);
        }
        return new List<GameObjectData>();
    }
}

[System.Serializable]
public class GameObjectData
{
    public string name;
    public List<float> position;

    public GameObjectData(GameObject obj)
    {
        name = obj.name;
        position = new List<float>
        {
            obj.transform.position.x,
            obj.transform.position.y,
            obj.transform.position.z
        };
    }
}