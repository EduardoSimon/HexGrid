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
    List<Color> colors;

	private void Awake() 
	{
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		meshCollider = gameObject.AddComponent<MeshCollider>();
		mesh.name = "Hex Mesh";
		vertices = new List<Vector3>();
		triangles = new List<int>();
        colors = new List<Color>();
	}

	public void Triangulate(HexCell[] cells)
	{
		//clear the old data in case a mesh has been generated previously
		mesh.Clear();
		vertices.Clear();
		triangles.Clear();
        colors.Clear();

		for (int i = 0; i < cells.Length; i++)
		{
			Triangulate(cells[i]);
		}

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();
		mesh.RecalculateNormals();
		meshCollider.sharedMesh = mesh;
	}

	private void Triangulate(HexCell cell)
	{
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            Triangulate(d, cell);
        }
	}

    private void Triangulate(HexDirection direction, HexCell cell)
    {
        Vector3 hexCenter = cell.transform.localPosition;
        Vector3 v1 = hexCenter + HexMetrics.GetFirstSolidCorner(direction);
        Vector3 v2 = hexCenter + HexMetrics.GetSecondSolidCorner(direction);
        AddTriangle(hexCenter,v1,v2);
        AddTriangleColor(cell.color);

        Vector3 bridge = HexMetrics.GetBridge(direction);
        Vector3 v3 = v1 + bridge;
        Vector3 v4 = v2 + bridge;

        AddQuad(v1, v2, v3, v4);

        HexCell prevNeighbour = cell.GetNeighbour(direction.Previous()) ?? cell;
        HexCell neighbour = cell.GetNeighbour(direction) ?? cell;
        HexCell nextNeighbour = cell.GetNeighbour(direction.Next()) ?? cell;

        AddQuadColor(cell.color, (cell.color + neighbour.color) * 0.5f);
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

    private void AddTriangleColor (Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }

    private void AddTriangleColor(Color c1, Color c2, Color c3)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }

    void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
    }

    void AddQuadColor(Color c1, Color c2, Color c3, Color c4)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
        colors.Add(c4);
    }

    void AddQuadColor(Color c1, Color c2)
    {
        colors.Add(c1);
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c2);
    }
}
