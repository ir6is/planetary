using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bot : MonoBehaviour
{
    private Planet m_myPlanet;
    private Planet[] m_planets;

    public void Initialize(Planet[] planets)
    {
        m_myPlanet = GetComponent<Planet>();
        m_planets = planets;
        if (m_myPlanet.Hp > 0)
        {
            StartCoroutine(Shooting());
        }
    }

    private IEnumerator Shooting()
    {
        yield return new WaitForSeconds(2);
        while (m_myPlanet.Hp > 0)
        {
            yield return new WaitForSeconds(m_myPlanet.CooldownTime);
            var planet = FindNearestPlanet();
            if (planet)
            {
                var dist = float.MaxValue;
                var cachedPos = Vector3.zero;
                for (int i = 1; i < 500; i++)
                {
                    var time = Time.fixedDeltaTime * i;
                    var pos = planet.GetPositionAfterTime(time);
                    var distance = (transform.position - pos).magnitude;
                    var diff = Mathf.Abs(distance - planet.ForcePower * time);

                    if (diff <= dist)
                    {
                        dist = diff;
                        cachedPos = pos;
                    }
                    else break;
                }

                m_myPlanet.TryShoot(cachedPos);
                yield return new WaitForSeconds(Random.Range(1, 2));
            }
            else yield break;
        }
    }

    private Planet FindNearestPlanet()
    {
        var minDistance = float.MaxValue;
        Planet nearestPlanet = null;

        for (int i = 0; i < m_planets.Length; i++)
        {
            var dist = transform.position - m_planets[i].transform.position;

            if (minDistance >= dist.sqrMagnitude && m_planets[i].Hp > 0 && m_planets[i] != m_myPlanet)
            {
                minDistance = dist.sqrMagnitude;
                nearestPlanet = m_planets[i];
            }
        }

        return nearestPlanet;
    }
}