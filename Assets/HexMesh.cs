using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour 
{
	Mesh mesh;
	MeshCollider meshCollider;
	List<Vector3> vertices;
	List<int> triangles;

	private void Awake() 
	{
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		meshCollider = gameObject.AddComponent<MeshCollider>();
		mesh.name = "Hex Mesh";
		vertices = new List<Vector3>();
		triangles = new List<int>();
	}

	public void Triangulate(HexCell[] cells)
	{
		//clear the old data in case a mesh has been generated previously
		mesh.Clear();
		vertices.Clear();
		triangles.Clear();

		for (int i = 0; i < cells.Length; i++)
		{
			Triangulate(cells[i]);
		}

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
		meshCollider.sharedMesh = mesh;
	}

	private void Triangulate(HexCell cell)
	{
		Vector3 hexCenter = cell.transform.localPosition;

		for (int i = 0; i < 6; i++)
		{
			AddTriangle(hexCenter, hexCenter + HexMetrics.HexCorners[i], hexCenter + HexMetrics.HexCorners[i + 1]);
		}
		
	}

	private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		int vertexCount = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);

		triangles.Add(vertexCount);
		triangles.Add(vertexCount + 1);
		triangles.Add(vertexCount + 2);
	}

}
