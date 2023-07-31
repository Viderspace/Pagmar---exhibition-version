using System;
using System.Collections;
using System.Collections.Generic;
using Synth_Variables.Adsr;
using UnityEngine;

public class AdsrMeshGenerator : MonoBehaviour
{
    public AdsrVariables globalAdsr;
    public Material material;
    private void Start()
    {
        // GameObject obj = new GameObject("Mesh", typeof(MeshRenderer), typeof(MeshFilter), typeof(UpdateMeshAdsr));
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshFilter>().mesh = CreateMesh();;
        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<UpdateMeshAdsr>();
        gameObject.GetComponent<UpdateMeshAdsr>().Init(globalAdsr);
    }

    private Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[7];
        Vector2[] uvs = new Vector2[7];
        int[] triangles = new int[3 * 5];
        
        
        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(.25f, 1);
        vertices[2] = new Vector3(.25f, 0);
        vertices[3] = new Vector3(.5f, .5f);
        vertices[4] = new Vector3(.5f, 0);
        vertices[5] = new Vector3(.75f, .5f);
        vertices[6] = new Vector3(1, 0);
        
        uvs[0] = new Vector3(0, 0);
        uvs[1] = new Vector3(.25f, 1);
        uvs[2] = new Vector3(.25f, 0);
        uvs[3] = new Vector3(.5f, .5f);
        uvs[4] = new Vector3(.5f, 0);
        uvs[5] = new Vector3(.75f, .5f);
        uvs[6] = new Vector3(1, 0);

        
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        
        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 3;
        
        triangles[6] = 2;
        triangles[7] = 3;
        triangles[8] = 4;
        
        triangles[9] = 4;
        triangles[10] = 3;
        triangles[11] = 5;
        
        triangles[12] = 4;
        triangles[13] = 5;
        triangles[14] = 6;

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        return mesh;
    }
}