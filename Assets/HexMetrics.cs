using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public const float solidFactor = 0.75f;
    public const float blendFactor = 1 - solidFactor;

	//the outerRadius is the radii of the circle you draw the hexagon into. OuterRadius = HexagonSide
	public const float outerRadius = 10.0f;
	//use trigonometry to get the innerRadius
	public const float innerRadius = outerRadius * 0.866025404f;

	public static Vector3[] HexCorners = 
	{
		new Vector3(0f,0f,outerRadius),
		new Vector3(innerRadius,0f,outerRadius * 0.5f),
		new Vector3(innerRadius,0f,outerRadius * -0.5f),
		new Vector3(0f,0f,-outerRadius),
		new Vector3(-innerRadius,0f,outerRadius * -0.5f),
		new Vector3(-innerRadius,0f,outerRadius * 0.5f),
		new Vector3(0f,0f,outerRadius)
	};

    public static Vector3 GetFirstCorner(HexDirection direction)
    {
        return HexCorners[(int)direction];
    }

    public static Vector3 GetSecondCorner(HexDirection direction)
    {
        return HexCorners[(int)direction + 1];
    }

    public static Vector3 GetFirstSolidCorner(HexDirection direction)
    {
        return HexCorners[(int)direction] * solidFactor;
    }

    public static Vector3 GetSecondSolidCorner(HexDirection direction)
    {
        return HexCorners[(int)direction + 1] * solidFactor;
    }

    public static Vector3 GetBridge(HexDirection direction)
    {
        return (HexCorners[(int)direction] + HexCorners[(int)direction + 1]) * 0.5f * blendFactor;
    }
}
