using System;
using System.Collections.Generic;
using UnityEngine;

// Smash Bros-like camera that adjusts viewing position to
// fit all focal points on screen with maximum zoom
public class CameraScript : MonoBehaviour
{
    public List<GameObject> focalPoints;
    public float padding = 20f;
    public float minimumScreenHeight = 5f;

    float aspectRatio;

    void Start() {
        aspectRatio = (float)Screen.width / (float)Screen.height;
    }

    void Update() {
        float minX = focalPoints[0].transform.position.x, minY = focalPoints[0].transform.position.y;
        float maxX = focalPoints[0].transform.position.x, maxY = focalPoints[0].transform.position.y;

        // find the maximum and minimum x and y coordinates
        // of all focal points
        foreach (GameObject focalPoint in focalPoints) {
            minX = Mathf.Min(minX, focalPoint.transform.position.x);
            maxX = Mathf.Max(maxX, focalPoint.transform.position.x);
            minY = Mathf.Min(minY, focalPoint.transform.position.y);
            maxY = Mathf.Max(maxY, focalPoint.transform.position.y);
        }

        // add padding so that there's space around
        // the focal points on the screen
        minX -= padding;
        maxX += padding;
        minY -= padding;
        maxY += padding;

        // set size of camera to fit all focal points
        float ySize = Math.Max((maxX - minX) * 0.5f / aspectRatio, (maxY - minY) * 0.5f);
        ySize = Math.Max(ySize, minimumScreenHeight);
        Camera.main.orthographicSize = ySize;

        // camera is placed at the center of all of the focal points
        float midX = (maxX + minX) * 0.5f, midY = (maxY + minY) * 0.5f;
        Camera.main.transform.position = new Vector3(midX, midY, -10);
    }

    // draws a box around the correct borders of the screen
    void OnDrawGizmos() {
        float minX = focalPoints[0].transform.position.x, minY = focalPoints[0].transform.position.y;
        float maxX = focalPoints[0].transform.position.x, maxY = focalPoints[0].transform.position.y;

        foreach (GameObject focalPoint in focalPoints) {
            minX = Mathf.Min(minX, focalPoint.transform.position.x);
            maxX = Mathf.Max(maxX, focalPoint.transform.position.x);
            minY = Mathf.Min(minY, focalPoint.transform.position.y);
            maxY = Mathf.Max(maxY, focalPoint.transform.position.y);
        }
        minX -= padding;
        maxX += padding;
        minY -= padding;
        maxY += padding;

        // slightly different math to achieve the same thing as in Update
        // except for minimum screen height
        float currAspectRatio = (maxX - minX) / (maxY - minY);
        if (currAspectRatio > aspectRatio) {
            float correctHeight = (maxX - minX) / aspectRatio;
            float diff = (correctHeight - (maxY - minY)) * 0.5f;
            minY -= diff;
            maxY += diff;
        }
        else {
            float correctWidth = (maxY - minY) * aspectRatio;
            float diff = (correctWidth - (maxX - minX)) * 0.5f;
            minX -= diff;
            maxX += diff;
        }

        Vector3 topLeft = new Vector3(minX, maxY, 0);
        Vector3 topRight = new Vector3(maxX, maxY, 0);
        Vector3 bottomRight = new Vector3(maxX, minY, 0);
        Vector3 bottomLeft = new Vector3(minX, minY, 0);

        Gizmos.color = Color.green;

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
