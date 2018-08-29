using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexMapEditor : MonoBehaviour {

    //TODO brush selection of hexcells in part 5
	public Color[] colors;
	public HexGrid hexGrid;
    public ToggleGroup ColorGroup;

    private bool isDrag;
    private bool applyColor;
    private HexDirection dragDirection;
    private HexCell previousCell;
    private OptionalToggle riverMode;
    private Color activeColor;
    private int activeElevation;
    private bool applyElevation = true;
    private float brushSize = 1.0f;

    enum OptionalToggle
    {
        Ignore, Yes, No
    }

    void Awake () {
		SelectColor(0);
	}

	void Update () {
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
	    {
			HandleInput();
        }
        else
	    {
	        previousCell = null;
	    }
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit))
		{
		    HexCell currentCell = hexGrid.GetCell(hit.point);
		    if (previousCell && previousCell != currentCell)
		    {
		        ValidateDrag(currentCell);
            }
		    else
		    {
		        isDrag = false;
		    }

		    EditCell(currentCell);
		    previousCell = currentCell;
		    isDrag = true;
		}
		else
		{
		    previousCell = null;
		}
	}

    private void ValidateDrag(HexCell currentCell)
    {
        for (dragDirection = HexDirection.NE; dragDirection <= HexDirection.NW; dragDirection++)
        {
            if (previousCell.GetNeighbor(dragDirection) == currentCell)
            {
                isDrag = true;
                return;
            }
        }

        isDrag = false;
    }

    private void EditCell(HexCell hexCell)
    {
        if (hexCell)
        {
            if (applyElevation)
                hexCell.ElevationLevel = activeElevation;

            if (riverMode == OptionalToggle.No)
                hexCell.RemoveRiver();
            else if (isDrag && riverMode == OptionalToggle.Yes)
            {
                var otherCell = hexCell.GetNeighbor(dragDirection.Opposite());
                if(otherCell)
                    otherCell.SetOutgoingRiver(dragDirection);
            }

            if(applyColor)
                hexCell.Color = activeColor;
        }

    }

    public void SelectColor (int index)
    {
        applyColor = ColorGroup.AnyTogglesOn();

        if(applyColor)
		    activeColor = colors[index];
	}

    public void SetElevationLevel(float value)
    {
        activeElevation = (int) value;
    }

    public void SetRiverMode(int mode)
    {
        riverMode = (OptionalToggle) mode; 
    }

    public void SetApplyElevation(bool mode)
    {
        applyElevation = mode;
    }

    public void SetBrushSize(float brushSize)
    {
        this.brushSize = (int)brushSize;
    }
}