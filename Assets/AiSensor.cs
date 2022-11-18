using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSensor : MonoBehaviour
{
    public float distance = 10;
    public float angle = 60;
    public float height = 1.0f;
    public Color meshColor = Color.red;

    Mesh mesh;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    Mesh CreateWedgeMesh(){
        Mesh mesh = new Mesh();

        int numTriangles = 8;
        int numVerticies = numTriangles * 3;

        Vector3[] verticies = new Vector3[numVerticies];
        int[] triangles = new int[numVerticies];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0,-angle,0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0,angle,0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        // left side
        verticies[vert++] = bottomCenter;
        verticies[vert++] = bottomLeft;
        verticies[vert++] = topLeft;

        verticies[vert++] = topLeft;
        verticies[vert++] = topCenter;
        verticies[vert++] = bottomCenter;

        // right side
        verticies[vert++] = bottomCenter;
        verticies[vert++] = topCenter;
        verticies[vert++] = topRight;

        verticies[vert++] = topRight;
        verticies[vert++] = bottomRight;
        verticies[vert++] = bottomCenter;

        // far side
        verticies[vert++] = bottomLeft;
        verticies[vert++] = bottomRight;
        verticies[vert++] = topRight;

        verticies[vert++] = topRight;
        verticies[vert++] = topLeft;
        verticies[vert++] = bottomLeft;

        // top
        verticies[vert++] = topCenter;
        verticies[vert++] = topLeft;
        verticies[vert++] = topRight;

        //bottom
        verticies[vert++] = bottomCenter;
        verticies[vert++] = bottomRight;
        verticies[vert++] = bottomLeft;

        for(int i = 0; i < numVerticies; ++i){
            triangles[i] = 1;
        }

        mesh.vertices = verticies;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    void OnValidate(){
        mesh = CreateWedgeMesh();
    }

    void OnDrawGizmos(){
        if(mesh){
            Gizmos.color = meshColor;
            Graphics.DrawMeshNow(mesh,transform.position,transform.rotation);
            Gizmos.DrawMesh(mesh,transform.position,transform.rotation);
        }
    }
}
