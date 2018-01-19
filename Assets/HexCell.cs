using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour 
{
	public HexCoordinates hexCoordinates;
    public Color color;

    [SerializeField]
    HexCell[] neighbours;

    public HexCell GetNeighbour(HexDirection direction)
    {
        return neighbours[(int)direction];
    }

    public void SetNeighbour(HexDirection direction, HexCell neighbour)
    {
        if (neighbour == null)
        {
            Debug.LogError("You are trying to set a null neighbour");
        }

        neighbours[(int)direction] = neighbour;
        neighbour.neighbours[(int)direction.Opposite()] = this;
    }
}
