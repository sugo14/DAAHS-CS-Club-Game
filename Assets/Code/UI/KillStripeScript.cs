using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.UIElements;

public class KillStripeScript : MonoBehaviour
{
    List<String> knockOutMessages = new List<String>
    {
        "KNOCK OUT!!!",
        "BLASTED!!!",
        "DESTROYED!!!",
        "PULVERIZED!!!",
        "SMASHED!!!",
        "ANNIHILATED!!!",
        "GG!!!",
        "OUTTA HERE!!!",
        "SENT FLYING!!!",
        "KABOOM!!!",
        "BAM!!!",
        "POW!!!",
        "CRUSHED!!!",
        "OBLITERATED!!!",
        "DEVASTATED!!!",
        "TOASTED!!!",
        "WASTED!!!",
        "DELETED!!!",
        "REKT!!!",
        "TERMINATED!!!",
        "VAPORIZED!!!",
        "EXECUTED!!!",
        "OBLITERATED!!!",
        "HOME RUN!!!",
        "ERASED!!!",
        "SHATTERED!!!",
        "FLATTENED!!!",
        "SLAMMED!!!",
        "CRATERED!!!",
        "EXTERMINATED!!!",
        "TOTALLED!!!",
        "DEMOLISHED!!!",
        "LAUNCHED!!!",
        "SEE YA!!!",
        "BOOM!!!",
        "TORCHED!!!",
        "FRIED!!!",
        "BYE BYE!!!",
        "D'OOOOOOOOH!!!",
        "TAKE THAT!!!",
        "EJECTED!!!",
        "NEVER GONNA GIVE YOU UP!!! NEVER GONNA LET YOU DOWN!!! NEVER GONNA RUN AROUND AND DESERT YOU!!! NEVER GONNA",
        "GAME OVER!!!",
    };

    public TMP_Text text;
    Vector3 originalPos;
    public float regularStrobeDuration = 0.25f, speed = 3f;
    public Color baseColor, strobeColor;
    public float xVariation = 3, yVariation = 2;
    public Vector3 origin;

    float regularStrobeTimer = 0;
    float startDist;

    public void Place(Vector3 pos)
    {
        text.text = knockOutMessages[UnityEngine.Random.Range(0, knockOutMessages.Count - 1)];

        // place the banner in front of the other gameobjects in the scene
        GetComponentInChildren<SpriteRenderer>().sortingLayerName = "SemiUI";
        text.GetComponent<MeshRenderer>().sortingLayerName = "SemiUI";

        // choose the point that the stripe will go towards (roughly (0, 0))
        origin = new Vector3(UnityEngine.Random.Range(xVariation, -xVariation), UnityEngine.Random.Range(-yVariation, yVariation));

        // multiply the position the player died by two to make sure the stripe starts offscreen
        originalPos = pos * 2;
        transform.position = originalPos;

        // calculate the angle on the stripe to point towards the origin
        Vector3 direction = (origin - transform.position).normalized;
        float angle = (360 + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float x = transform.position.x - origin.x, y = transform.position.y - origin.y;
        startDist = (float)Math.Sqrt(x * x + y * y);

        // vertically flip the text if it would be upside down otherwise
        if (angle >= 90 && angle <= 270)
        {
            text.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
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

    void Kill()
    {
        Destroy(gameObject);
    }

    // pretty sure the moving code is bad in various framerates but thats for later
    void Update()
    {
        float x = transform.position.x - origin.x, y = transform.position.y - origin.y;
        float c = (float)Math.Sqrt(x * x + y * y);

        float dist = Math.Max(1f, c) * Time.deltaTime * speed;
        transform.position += (origin - originalPos).normalized * dist;

        if (c > startDist+1) { Kill(); }
        UpdateRegularStrobe();
    }
}
