using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;

    public HexGrid hexGrid;

    private Color activeColor;

    void Awake()
    {
        SelectColor(0);
    }


    private void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }                            
    }

    void HandleInput()
    {
        Ray hitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(hitRay, out hit))
        {
            hexGrid.ColorCell(hit.point, activeColor);
        }
    }
    
    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }


}
