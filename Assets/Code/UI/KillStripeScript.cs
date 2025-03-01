using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class KillStripeScript : MonoBehaviour
{
    public TMP_Text text;
    Vector3 originalPos;
    public float regularStrobeDuration = 0.25f, speed = 3f;
    public Color baseColor, strobeColor;

    float regularStrobeTimer = 0;

    public void Place(Vector3 pos)
    {
        // place the banner in front of everything
        GetComponentInChildren<SpriteRenderer>().sortingLayerName = "SemiUI";
        text.GetComponent<MeshRenderer>().sortingLayerName = "SemiUI";

        originalPos = pos;
        transform.position = pos * 2;
        Vector3 direction = (Vector3.zero - transform.position).normalized;
        float angle = (360 + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360;

        if (angle >= 90 && angle <= 270)
        {
            text.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }

        Debug.Log(angle);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void UpdateRegularStrobe()
    {
        regularStrobeTimer += Time.deltaTime;
        float piece = regularStrobeTimer % (regularStrobeDuration * 2);
        float strobeIntensity = Math.Min(piece, regularStrobeDuration * 2 - piece) / regularStrobeDuration;
        strobeIntensity *= strobeIntensity; // Smoothen it
        text.color = Color.Lerp(baseColor, strobeColor, strobeIntensity);
    }

    void Start()
    {
        Place(transform.position);
    }

    // pretty sure the moving code is bad in various framerates but thats for later
    void Update()
    {
        float c = (float)Math.Sqrt(transform.position.x * transform.position.x + transform.position.y * transform.position.y);
        if (c > 1000) { Destroy(gameObject); } // nice check lmao
        float dist = Math.Max(1f, c) * Time.deltaTime * speed;
        transform.position -= originalPos.normalized * dist;
        UpdateRegularStrobe();
    }
}
