using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidbody;

    public float Mass => m_rigidbody.mass;
}