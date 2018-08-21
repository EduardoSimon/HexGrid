using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridChunk : MonoBehaviour
{
    private HexCell[] cells;

    private HexMesh mesh;
    private Canvas gridCanvas;

    void Awake()
    {
        mesh = GetComponentInChildren<HexMesh>();
        gridCanvas = GetComponentInChildren<Canvas>();

        cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
    }

	// Use this for initialization
	void Start () {
		mesh.Triangulate(cells);
	}

    public void AddCell(int i, HexCell cell)
    {
        cells[i] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform,false);
        cell.rect.SetParent(gridCanvas.transform,false);
    }

    public void Refresh()
    {
        enabled = true;
    }

    void LateUpdate()
    {
        mesh.Triangulate(cells);
        enabled = false;
    }
}
