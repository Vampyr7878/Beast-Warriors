using UnityEditor;
using UnityEngine;

public class Bone : MonoBehaviour
{
    public Material material;
    
    void Start()
    {
        Mesh mesh = new();
        mesh.name = "Cube";
        Vector3[] vertices = new Vector3[]
        {
            new(-5.0f, -5.0f, -5.0f),
            new ( 5.0f, -5.0f, -5.0f),
            new (-5.0f,  5.0f, -5.0f),
            new ( 5.0f,  5.0f, -5.0f),
            new (-5.0f, -5.0f,  5.0f),
            new ( 5.0f, -5.0f,  5.0f),
            new (-5.0f,  5.0f,  5.0f),
            new ( 5.0f,  5.0f,  5.0f)
        };
        mesh.vertices = vertices;
        int[] triangles = new int[] 
        {
            0, 2, 1,
            1, 2, 3,
            4, 6, 0,
            0, 6, 2,
            5, 7, 4,
            4, 7, 6,
            1, 3, 5,
            5, 3, 7,
            4, 0, 5,
            5, 0, 1,
            2, 6, 3,
            3, 6, 7
        };
        mesh.triangles = triangles;
        gameObject.AddComponent<MeshFilter>();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        gameObject.AddComponent<MeshRenderer>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material;
        AssetDatabase.CreateAsset(mesh, "Assets/Cube.mesh");
    }
}
