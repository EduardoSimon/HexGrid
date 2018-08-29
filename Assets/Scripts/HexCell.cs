using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class HexCell : MonoBehaviour {
    public HexCoordinates coordinates;

    private Color color;

    private bool hasIncomingRiver, hasOutgoingRiver;

    private HexDirection incomingRiver, outgoingRiver;

    public RectTransform rect;

    public HexGridChunk chunk;

    private int elevationLevel = int.MinValue;

    [SerializeField]
	HexCell[] neighbors;

    #region Properties
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

            if(hasOutgoingRiver && elevationLevel < GetNeighbor(outgoingRiver).elevationLevel)
                RemoveOutgoingRiver();

            if(hasIncomingRiver && elevationLevel > GetNeighbor(incomingRiver).elevationLevel)
                RemoveIncomingRiver();

            Refresh();
        }
    }

    public float StreamBedY
    {
        get
        {
            return
                (elevationLevel + HexMetrics.streamBedElevationOffset) *
                HexMetrics.elevationStep;
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

    public bool HasIncomingRiver
    {
        get
        {
            return hasIncomingRiver;
        }

        set
        {
            hasIncomingRiver = value;
        }
    }

    public bool HasOutgoingRiver
    {
        get
        {
            return hasOutgoingRiver;
        }

        set
        {
            hasOutgoingRiver = value;
        }
    }

    public HexDirection IncomingRiver
    {
        get
        {
            return incomingRiver;
        }

        set
        {
            incomingRiver = value;
        }
    }

    public HexDirection OutgoingRiver
    {
        get
        {
            return outgoingRiver;
        }

        set
        {
            outgoingRiver = value;
        }
    }

    public bool HasRiver
    {
        get { return hasIncomingRiver || hasOutgoingRiver; }
    }

    public bool HasRiverBeginOrEnd
    {
        get { return hasIncomingRiver != hasOutgoingRiver; }
    }

    public bool HasRiverThroughEdge(HexDirection direction)
    {
        return hasIncomingRiver && incomingRiver == direction ||
               hasOutgoingRiver && outgoingRiver == direction;
    }
    #endregion

    public void RemoveOutgoingRiver()
    {
        if (!hasOutgoingRiver)
        {
            return;
        }

        hasOutgoingRiver = false;
        RefreshSelfOnly();

        HexCell neighbor = GetNeighbor(outgoingRiver);
        neighbor.hasIncomingRiver = false;
        neighbor.RefreshSelfOnly();
    }

    public void RemoveIncomingRiver()
    {
        if (!hasOutgoingRiver)
        {
            return;
        }

        hasIncomingRiver = false;
        RefreshSelfOnly();

        HexCell neighbor = GetNeighbor(incomingRiver);
        neighbor.hasOutgoingRiver = false;
        neighbor.RefreshSelfOnly();
    }

    public void RemoveRiver()
    {
        RemoveOutgoingRiver();
        RemoveIncomingRiver();
    }

    public void SetOutgoingRiver(HexDirection direction)
    {
        if (hasOutgoingRiver && outgoingRiver == direction)
            return;

        HexCell neighbor = GetNeighbor(direction);

        //it only flows downhill
        if (!neighbor || neighbor.elevationLevel < elevationLevel)
            return;

        RemoveOutgoingRiver();

        if (hasIncomingRiver && incomingRiver == direction)
            RemoveIncomingRiver();

        hasOutgoingRiver = true;
        outgoingRiver = direction;
        RefreshSelfOnly();

        neighbor.RemoveIncomingRiver();
        neighbor.hasIncomingRiver = true;
        neighbor.incomingRiver = direction;
        neighbor.RefreshSelfOnly();
    }

    private void RefreshSelfOnly()
    {
        chunk.Refresh();
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