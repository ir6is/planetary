using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Serializable]
    public class RocketData
    {
        public bool IsTrigger;
        public Vector3 Velocity;
        public Vector3 Position;
        public int Type;
    }

    [SerializeField] private SaveLoadService m_saveLoadService;
    [SerializeField] private Collider m_collider;
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private float m_forcePower;
    [SerializeField] private ParticleSystem m_particleSystem;

    private Planet m_myPlanet;
    private Sun m_sun;
    private int m_rocketType;

    [field: SerializeField] public float Cooldown { get; private set; } = 1;
    [field: SerializeField] public int Damage { get; private set; } = 1;
    public float ForcePower => m_forcePower;

    public RocketData GenerateData()
    {
        var newRocketData = new RocketData();
        newRocketData.Velocity = m_rigidbody.velocity;
        newRocketData.IsTrigger = m_collider.isTrigger;
        newRocketData.Position = transform.position;
        newRocketData.Type = m_rocketType;
        return newRocketData;
    }

    public void LoadData(RocketData data, Sun sun)
    {
        m_rigidbody.velocity = data.Velocity;
        m_collider.isTrigger = data.IsTrigger;
        transform.position = data.Position;
        m_rocketType = data.Type;
        m_sun = sun;
    }

    public void Launch(Vector3 targetWorldPos, Sun sun, int type)
    {
        m_rocketType = type;
        var dir = targetWorldPos - transform.position;
        transform.LookAt(targetWorldPos);
        m_sun = sun;
        m_rigidbody.AddForce(dir.normalized * m_forcePower, ForceMode.VelocityChange);
        Destroy(gameObject, 5);
    }

    private void Awake()
    {
        m_saveLoadService.Rockets.Add(this);
    }

    private void OnDestroy()
    {
        m_saveLoadService.Rockets.Remove(this);
    }

    private void Update()
    {
        transform.LookAt(m_rigidbody.velocity + transform.position);
    }

    private void FixedUpdate()
    {
        var dist = m_sun.transform.position - transform.position;
        var lenght = dist.magnitude;
        var f = (9.8f * m_rigidbody.mass * m_sun.Mass) / (lenght * lenght);
        m_rigidbody.AddForce((dist / lenght) * f);
    }

    private void OnCollisionEnter(Collision other)
    {
        var planet = other.gameObject.GetComponent<Planet>();
        if (planet)
        {
            planet.Hp -= Damage;
        }

        m_particleSystem.Play();
        m_particleSystem.transform.SetParent(null);
        Destroy(m_particleSystem.gameObject, 1);
        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        m_collider.isTrigger = false;
    }
}