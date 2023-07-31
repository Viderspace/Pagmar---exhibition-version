using System;
using System.Collections;
using System.Collections.Generic;
using Lasp;
using UnityEngine;

public class SpectrumAnalyzerMeshGenerator : MonoBehaviour
{
    const int TriangleEdges = 3;
    [SerializeField][Range(0.01f,1)] private float height = 0.5f;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material material;
    [SerializeField] private SpectrumAnalyzer spectrumAnalyzer;
    [SerializeField] private RectTransform panelRectTransform;
    private Rect Rect => panelRectTransform.rect;
    
    
    
    private int[] triangles = null;
    private Vector2[] uvs = null;
    private Vector3[] vertices = null;

    private int _spectrumResolution = 128;
    private int TriangleAmount => 2 * (_spectrumResolution - 1);
    private int VertexAmount => _spectrumResolution * 2;

    private void Start()
    {
        // GameObject obj = new GameObject("Mesh", typeof(MeshRenderer), typeof(MeshFilter), typeof(UpdateMeshAdsr));
        meshFilter.mesh = CreateMesh();
        meshRenderer.material = material;
    }

    private void GenerateTriangles()
    {
        int vertIndex = 0;
        for (int i = 0; i < TriangleAmount * TriangleEdges; i += 6)
        {
            GenerateBlockOf2Triangles(i, vertIndex);
            vertIndex += 2;
        }
    }

    private void GenerateBlockOf2Triangles(int i, int vertIndex)
    {
        triangles[i] = vertIndex;
        triangles[i + 1] = vertIndex + 1;
        triangles[i + 2] = vertIndex + 2;

        triangles[i + 3] = vertIndex + 2;
        triangles[i + 4] = vertIndex + 1;
        triangles[i + 5] = vertIndex + 3;
    }

    private void GenerateVerticesAndUvs()
    {
        for (int i = 0; i < _spectrumResolution; i++)
        {
            var loweVertex = i * 2;
            vertices[loweVertex] = new Vector3(i / (float) (_spectrumResolution - 1), 0);
            uvs[loweVertex] = new Vector3(i / (float) (_spectrumResolution - 1), 0);

            var upperVertex = i * 2 + 1;
            vertices[upperVertex] = new Vector3(i / (float) (_spectrumResolution - 1), 1);
            uvs[upperVertex] = new Vector3(i / (float) (_spectrumResolution - 1), 1);
        }
    }

    private Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        vertices = new Vector3[VertexAmount];
        uvs = new Vector2[VertexAmount];
        triangles = new int[TriangleAmount * TriangleEdges];

        GenerateVerticesAndUvs();
        GenerateTriangles();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        return mesh;
    }


    private void FixedUpdate()
    {
        var mesh = meshFilter.mesh;
        var currentVertices = mesh.vertices;
        var spectrum = spectrumAnalyzer.logSpectrumSpan;
        for (int i = 0; i < _spectrumResolution; i++)
        {
            var upperVertex = i * 2 + 1;
            currentVertices[upperVertex].y = spectrum[i]*height;
        }
        mesh.vertices = currentVertices;
    }
    
}