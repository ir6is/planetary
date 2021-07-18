using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlanetView : MonoBehaviour
{
    [SerializeField] private Mesh m_defaultView;
    [SerializeField] private Mesh m_playerView;
    [SerializeField] private Vector3 m_rotateDirection;

    [ContextMenu("UpdateView")]
    public void UpdateView(int hash, bool isPlayer)
    {
        var rnd = new Random(hash);

        m_rotateDirection = new Vector3(rnd.Next(-150, 151),
                                        rnd.Next(-150, 151),
                                        rnd.Next(-150, 151));
        var filter = GetComponent<MeshFilter>();
        filter.mesh = isPlayer ? m_playerView : m_defaultView;
        GetComponent<MeshRenderer>().material.color = new Color(rnd.NextFloat(0f, 1f), rnd.NextFloat(0f, 1f), rnd.NextFloat(0f, 1f));
        var vertices = filter.sharedMesh.vertices;
        transform.DestroyAllChildren();
        for (int i = 0; i < 10; i++)
        {
            var newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newSphere.transform.localScale = Vector3.one * .1f;
            newSphere.transform.SetParent(transform);
            newSphere.transform.localPosition = vertices[rnd.Next(0, vertices.Length)];
            Destroy(newSphere.GetComponent<Collider>());
            var sphereRenderer = newSphere.GetComponent<MeshRenderer>();
            var tempMaterial = new Material(sphereRenderer.sharedMaterial);
            tempMaterial.color = new Color(rnd.NextFloat(0f, 1f), rnd.NextFloat(0f, 1f), rnd.NextFloat(0f, 1f));
            sphereRenderer.sharedMaterial = tempMaterial;
        }
    }

    private void Update()
    {
        transform.Rotate(m_rotateDirection * Time.deltaTime);
    }
}