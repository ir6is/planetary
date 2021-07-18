using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveLoadService", menuName = "CreateSaveLoadService", order = 1)]
public class SaveLoadService : ScriptableObject
{
    [Serializable]
    public class Data2Save
    {
        public List<PlanetData> PlanetDatas = new List<PlanetData>();
        public List<Rocket.RocketData> RocketDatas = new List<Rocket.RocketData>();
    }

    public HashSet<Rocket> Rockets { get; private set; } = new HashSet<Rocket>();
    public HashSet<Planet> Planets { get; private set; } = new HashSet<Planet>();

    public void ClearGame()
    {
        foreach (var item in Rockets)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Planets)
        {
            Destroy(item.gameObject);
        }
    }

    public void Save()
    {
        var data2Save = new Data2Save();

        foreach (var item in Planets)
        {
            data2Save.PlanetDatas.Add(item.GenerateData());
        }

        foreach (var item in Rockets)
        {
            data2Save.RocketDatas.Add(item.GenerateData());
        }

        var data2SavedJson = JsonUtility.ToJson(data2Save);
        PlayerPrefs.SetString(nameof(Data2Save), data2SavedJson);
    }

    public Data2Save LoadData()
    {
        try
        {
            return JsonUtility.FromJson<Data2Save>(PlayerPrefs.GetString(nameof(Data2Save)));
        }
        catch
        {
            Debug.Log("Cant load data");
            return null;
        }
    }
}