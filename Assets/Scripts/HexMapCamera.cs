using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class HexMapCamera : MonoBehaviour
{
    public float stickMinZoom, stickMaxZoom;
    public float swivelMinZoom, swivelMaxZoom;
    public float moveSpeedMinZoom;
    public float moveSpeedMaxZoom;
    [Tooltip("Speed in  degrees per seconds")]
    public float rotationSpeed;
    public HexGrid Grid;

    [NotNull]
    private Transform swivel, stick;
    private float zoom = 1.0f;
    private float rotationAngle;

    void Awake()
    {
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    float deltaZoom = Input.GetAxis("Mouse ScrollWheel");
	    float deltaX = Input.GetAxis("Horizontal");
	    float deltaZ = Input.GetAxis("Vertical");
	    float rotationDelta = Input.GetAxis("Rotation");

	    if (rotationDelta != 0f)
	        AdjustRotation(rotationDelta);

	    if (deltaZoom != 0f)
	        AdjustZoom(deltaZoom);

        if(deltaX != 0 || deltaZ != 0)
            AdjustPosition(deltaX, deltaZ);
	}

    private void AdjustRotation(float rotationDelta)
    {
        rotationAngle += rotationDelta * rotationSpeed * Time.deltaTime;

        if (rotationAngle >= 360f)
            rotationAngle -= 360f;
        else if (rotationAngle <= 0)
            rotationAngle += 360f;

        transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
    }

    private void AdjustPosition(float deltaX, float deltaZ)
    {
        float damping = Mathf.Max(Mathf.Abs(deltaX), Mathf.Abs(deltaZ));
        float speed = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom);
        Vector3 direction = transform.localRotation * new Vector3(deltaX, 0.0f, deltaZ).normalized;
        Vector3 newPos = direction * damping * Time.deltaTime * speed;
        Vector3 position = transform.localPosition;
        position += newPos;

        transform.localPosition = ClampPosition(position);


    }

    private Vector3 ClampPosition(Vector3 newPos)
    {
        float xMax = (Grid.chunkCountX * HexMetrics.chunkSizeX -0.5f) * (2f * HexMetrics.innerRadius);

        newPos.x = Mathf.Clamp(newPos.x, 0f, xMax);

        float zMax = (Grid.chunkCountZ * HexMetrics.chunkSizeZ - 1f) * (1.5f * HexMetrics.outerRadius);
        newPos.z = Mathf.Clamp(newPos.z, 0.0f, zMax );

        return newPos;

    }

    private void AdjustZoom(float deltaZoom)
    {
        zoom = Mathf.Clamp01(zoom + deltaZoom);

        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
        stick.localPosition = new Vector3(0.0f,distance,0.0f);

        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
        swivel.localRotation = Quaternion.Euler(angle,0f,0f);
    }
}
