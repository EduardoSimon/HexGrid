using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;
    private Color color;
    public RectTransform rect;
    public HexGridChunk chunk;

    private int elevationLevel = int.MinValue;

    [SerializeField]
	HexCell[] neighbors;

    public int ElevationLevel
    {
        get
        {
            return elevationLevel;
        }

        set
        {
            if (elevationLevel == value)
                return;

            elevationLevel = value;

            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            position.y += (HexMetrics.SampleNoise(position).y * 2f - 1f) * HexMetrics.elevationPerturbStrength;
            transform.localPosition = position;

            Vector3 uiPosition = rect.localPosition;
            uiPosition.z = -position.y;
            rect.localPosition = uiPosition;

            Refresh();
        }
    }

    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

    public Color Color
    {
        get
        {
            return color;
        }

        set
        {
            if (color == value)
                return;
            color = value;
            Refresh();
        }
    }

    public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

    public HexMetrics.HexEdgeType GetEdgeType (HexDirection direction)
    {
        return HexMetrics.GetEdgeType(elevationLevel, neighbors[(int) direction].elevationLevel);
    }

    public HexMetrics.HexEdgeType GetEdgeType(HexCell otherCell)
    {
        return HexMetrics.GetEdgeType(elevationLevel, otherCell.ElevationLevel);
    }

    void Refresh()
    {
        if(chunk)
            chunk.Refresh();

        for (int i = 0; i < neighbors.Length; i++)
        {
            HexCell neighbor = neighbors[i];
            if (neighbor != null && neighbor.chunk != chunk)
            {
                neighbor.chunk.Refresh();
            }
        }
    }

}