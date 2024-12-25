using System;
using System.Collections.Generic;
using UnityEngine;

// Smash Bros-like camera that adjusts viewing position to fit all focal points on screen with maximum zoom
public class CameraScript : MonoBehaviour
{
    public List<GameObject> FocalPoints;
    public float TopPadding = 3, BottomPadding = 3, LeftPadding = 3, RightPadding = 3;
    public float MinX = -15, MaxX = 15, MinY = -10, MaxY = 10;
    public float MinimumScreenHeight = 5f;
    public float CameraMoveSpeed = 10f, CameraResizeSpeed = 10f;

    float aspectRatio;

    void Awake()
    {
        aspectRatio = (float)Screen.width / (float)Screen.height;
    }

    // Needs to be ran in the same update loop as the focal points' movement
    void FixedUpdate()
    {
        if (FocalPoints.Count == 0)
        {
            return;
        }
        
        float minX = FocalPoints[0].transform.position.x, minY = FocalPoints[0].transform.position.y;
        float maxX = FocalPoints[0].transform.position.x, maxY = FocalPoints[0].transform.position.y;

        // Find the maximum and minimum x and y coordinates of all focal points
        foreach (GameObject focalPoint in FocalPoints)
        {
            minX = Mathf.Min(minX, focalPoint.transform.position.x);
            maxX = Mathf.Max(maxX, focalPoint.transform.position.x);
            minY = Mathf.Min(minY, focalPoint.transform.position.y);
            maxY = Mathf.Max(maxY, focalPoint.transform.position.y);
        }

        minX = minX - LeftPadding;
        maxX = maxX + RightPadding;
        minY = minY - BottomPadding;
        maxY = maxY + TopPadding;

        // Set viewing size of camera to fit all focal points
        float desiredXSize = (maxX - minX) * 0.5f;
        float desiredYSize = Math.Max(MinimumScreenHeight, (maxY - minY) * 0.5f);

        // Adjust the viewing size to fit the aspect ratio
        if (desiredXSize / aspectRatio > desiredYSize)
        {
            desiredYSize = desiredXSize / aspectRatio;
            /* minY -= (desiredYSize - (maxY - minY) * 0.5f);
            maxY += (desiredYSize - (maxY - minY) * 0.5f); */
        }
        else
        {
            desiredXSize = desiredYSize * aspectRatio;
            /* minX -= (desiredXSize - (maxX - minX) * 0.5f);
            maxX += (desiredXSize - (maxX - minX) * 0.5f); */
        }
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, desiredYSize, CameraResizeSpeed * Time.deltaTime);

        // Camera is placed at the center of all of the focal points
        float midX = (maxX + minX) * 0.5f, midY = (maxY + minY) * 0.5f;
        Vector3 desiredPosition = new Vector3(midX, midY, -10);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, desiredPosition, CameraMoveSpeed * Time.deltaTime);

        // Draw gizmos at (minX, minY) and (maxX, maxY)
        Debug.DrawLine(new Vector3(minX, minY, 0), new Vector3(minX, maxY, 0), Color.red);
        Debug.DrawLine(new Vector3(minX, minY, 0), new Vector3(maxX, minY, 0), Color.red);
        Debug.DrawLine(new Vector3(maxX, maxY, 0), new Vector3(minX, maxY, 0), Color.red);
        Debug.DrawLine(new Vector3(maxX, maxY, 0), new Vector3(maxX, minY, 0), Color.red);
    }
}
