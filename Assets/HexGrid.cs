using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour 
{
	public int height = 10;
	public int width = 10;

	public HexCell cellPrefab;
	public Text labelPrefab;

	Canvas canvas;
	HexCell[] cells;

	private void Awake() 
	{
		cells = new HexCell[height * width];
		canvas = GetComponentInChildren<Canvas>();

		FillGrid();

	}

	void CreateCell(int x, int z, int i)
	{
		Vector3 position = new Vector3(
		(x + z * 0.5f - z / 2) * HexMetrics.innerRadius * 2f,
		0f,
		z * HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform,false);
		cell.transform.localPosition = position;

		Text label = Instantiate<Text>(labelPrefab);
		label.rectTransform.SetParent(canvas.transform,false);
		label.rectTransform.anchoredPosition = new Vector2(position.x,position.z);
		label.text = x.ToString() + "\n" + z.ToString();
	}

	void FillGrid()
	{
		for (int x = 0, i = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				CreateCell(x,z,i++);
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
