using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour 
{
    [Header("Picking Colors")]
    public Color defaultColor = Color.white;

    [Header("Grid Size")]
	public int height = 10;
	public int width = 10;

    [Header("Prefabs")]
	public HexCell cellPrefab;
	public Text labelPrefab;

	Canvas canvas;
	HexMesh hexMesh;
	HexCell[] cells;

	private void Awake() 
	{
		cells = new HexCell[height * width];
		canvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

		FillGrid();
	}

	void Start ()
	{
		hexMesh.Triangulate(cells);
	}

	void CreateCell(int x, int z, int i)
	{
		Vector3 position = new Vector3((x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2.0f),
		0f,
		z * HexMetrics.outerRadius * 1.5f);

        //position
		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform,false);
		cell.transform.localPosition = position;
		cell.hexCoordinates = HexCoordinates.FromOffsetCoordinates(x,z);

        //ui
		Text label = Instantiate<Text>(labelPrefab);
		label.rectTransform.SetParent(canvas.transform,false);
		label.rectTransform.anchoredPosition = new Vector2(position.x,position.z);
		label.text = cell.hexCoordinates.ToStringOnSeparateLines();

        //color
        cell.color = defaultColor;

        //neighbours
        if (x > 0)
            cell.SetNeighbour(HexDirection.W, cells[i - 1]);

        if (z > 0)
        {
            if ((z & 1) == 0) //even number
            {
                cell.SetNeighbour(HexDirection.SE, cells[i - width]);

                if (x > 0)//Ignore the first element of the row as it doesnt have sw neighbour
                {
                    cell.SetNeighbour(HexDirection.NW, cells[i - width - 1]);
                }
            }
            else
            {
                cell.SetNeighbour(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                {
                    cell.SetNeighbour(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

                
	}

	void FillGrid()
	{
		for (int z = 0, i = 0; z < height; z++)
		{
			for (int x = 0; x < width; x++)
			{
				CreateCell(x,z,i++);
			}
		}
	}


	public void ColorCell(Vector3 touch, Color color)
	{
		touch = transform.InverseTransformPoint(touch);
		HexCoordinates coords = HexCoordinates.FromPosition(touch);
        int index = coords.X + coords.Z * height + coords.Z / 2;
        Debug.Log(index);
        HexCell cell = cells[index];
        cell.color = color;
        hexMesh.Triangulate(cells);
		Debug.Log("Touched at: " + coords.ToString());
	}	
	
	
}
