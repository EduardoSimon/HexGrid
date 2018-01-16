using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
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


}
