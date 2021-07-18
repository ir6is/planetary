using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class PlanetData
{
    public int Hp;
    public Vector3 Position;
    public float CooldownTime;
    public bool IsPlayer;
    public float RotationSpeed;
    public int HashView;
    public int RocketType;
    public bool HaveHash;
    public bool HaveRoketType;
}

public class Planet : MonoBehaviour
{
    [SerializeField] private SaveLoadService m_saveLoadService;
    [SerializeField] private Text m_hpBar;
    [SerializeField] private Image m_cooldownImage;
    [SerializeField] private PlanetView m_planetView;

    [SerializeField] private Rocket[] m_rocketPrefabs;

    private Sun m_sun;

    private int m_hp;
    private float m_rotateSpeed = 1;
    private int m_hashView;
    private int m_rocketType;

    public float CooldownTime { get; private set; }
    public float ForcePower => m_rocketPrefabs[m_rocketType].ForcePower;

    public int Hp
    {
        get => m_hp;
        set
        {
            m_hp = value;
            m_hpBar.text = $"Hp = {value}";
            if (m_hp <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public PlanetData GenerateData()
    {
        var newPlanetData = new PlanetData();
        newPlanetData.Hp = m_hp;
        newPlanetData.Position = transform.position;
        newPlanetData.CooldownTime = CooldownTime;
        newPlanetData.IsPlayer = GetComponent<Player>();
        newPlanetData.RotationSpeed = m_rotateSpeed;
        newPlanetData.HashView = m_hashView;
        newPlanetData.RocketType = m_rocketType;
        newPlanetData.HaveHash = true;
        newPlanetData.HaveRoketType = true;
        return newPlanetData;
    }

    public void LoadData(PlanetData data, Sun sun)
    {
        m_sun = sun;

        Hp = data.Hp;
        transform.position = data.Position;
        CooldownTime = data.CooldownTime;
        m_rotateSpeed = data.RotationSpeed;
        if (data.HaveHash) m_hashView = data.HashView;
        else m_hashView = Random.Range(-9000, 9000);

        if (data.HaveRoketType) m_rocketType =  data.RocketType;
        else m_rocketType = Random.Range(0, m_rocketPrefabs.Length);

        m_planetView.UpdateView(m_hashView,data.IsPlayer);
    }

    public void TryShoot(Vector3 position)
    {
        if (CooldownTime == 0)
        {
            var newRocket = Instantiate(m_rocketPrefabs[m_rocketType], transform.position, Quaternion.identity);
            CooldownTime = newRocket.Cooldown;
            newRocket.Launch(position, m_sun, m_rocketType);
        }
    }

    public Vector3 GetPositionAfterTime(float time)
    {
        var pos = transform.position;
        return Quaternion.Euler(0, m_rotateSpeed * time, 0) * pos;
    }

    private void Awake()
    {
        m_saveLoadService.Planets.Add(this);
    }

    private void OnDestroy()
    {
        m_saveLoadService.Planets.Remove(this);
    }

    private void Update()
    {
        transform.position = GetPositionAfterTime(Time.deltaTime);
        CooldownTime -= Time.deltaTime;
        CooldownTime = Mathf.Max(CooldownTime, 0);
        m_cooldownImage.fillAmount = 1 - CooldownTime / m_rocketPrefabs[m_rocketType].Cooldown;
    }
}