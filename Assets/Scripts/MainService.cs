using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainService : MonoBehaviour
{
    [SerializeField] private SaveLoadService m_saveLoadService;
    [SerializeField] [Min(2)] private int m_minPlayersCount = 2;
    [SerializeField] [Min(6)] private int m_manPlayersCount = 5;

    [SerializeField] private Planet m_planetPrefab;
    [SerializeField] private Rocket[] m_rocketPrefabs;

    [SerializeField] private Sun m_sun;
    [SerializeField] private Camera m_camera;

    public bool IsPause
    {
        get => Time.timeScale == 0;
        set => _ = value ? Time.timeScale = 0 : Time.timeScale = 1;
    }

    public void Restart()
    {
        m_saveLoadService.ClearGame();
        var planetsData = GenerateRandomData();
        CreatePlanets(planetsData);
    }

    public void Save()
    {
        m_saveLoadService.Save();
    }

    public void Load()
    {
        m_saveLoadService.ClearGame();
        var data = m_saveLoadService.LoadData();
        if (data != null)
        {
            CreatePlanets(data.PlanetDatas);
            for (int i = 0; i < data.RocketDatas.Count; i++)
            {
                var rocketData = data.RocketDatas[i];
                var newRocket = Instantiate(m_rocketPrefabs[rocketData.Type]);
                newRocket.LoadData(rocketData, m_sun);
            }
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        Restart();
    }

    private void CreatePlanets(IReadOnlyList<PlanetData> planetsData)
    {
        var planets = new Planet[planetsData.Count];

        for (int i = 0; i < planetsData.Count; i++)
        {
            var newPlanet = Instantiate(m_planetPrefab);
            var planetData = planetsData[i];

            newPlanet.LoadData(planetData, m_sun);
            planets[i] = newPlanet;

            if (planetData.IsPlayer)
            {
                var player = newPlanet.gameObject.AddComponent<Player>();
                player.Initialize(m_camera);
            }
            else
            {
                var bot = newPlanet.gameObject.AddComponent<Bot>();
                bot.Initialize(planets);
            }
        }
    }

    private PlanetData[] GenerateRandomData()
    {
        var playersCount = Random.Range(m_minPlayersCount, m_manPlayersCount);
        var playerIndex = Random.Range(0, playersCount);
        var planets = new PlanetData[playersCount];
        var possibleDirections = new List<Vector3>();
        for (int i = 0; i < 10; i++)
        {
            possibleDirections.Add(Quaternion.Euler(0, i * 36, 0) * m_sun.transform.forward);
        }

        for (int i = 0; i < playersCount; i++)
        {
            var planetsData = new PlanetData();
            planetsData.Position = (i + 2) * possibleDirections[Random.Range(0, possibleDirections.Count)] * -2f;
            planetsData.IsPlayer = i == playerIndex;
            planetsData.Hp = planetsData.IsPlayer ? 20 : 10;
            planetsData.RotationSpeed = (i + 1) * Random.Range(18, 30);
            planets[i] = planetsData;
        }

        return planets;
    }
}