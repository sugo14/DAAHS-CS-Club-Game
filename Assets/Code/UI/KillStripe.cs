using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The kill stripe effect that appears on the screen when a player is knocked out.
/// </summary>
public class KillStripe : MonoBehaviour
{
    static readonly String[] knockOutMessages = new String[]
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

    public void Initialize(Vector3 deathPos)
    {
        // Choose a random message to display
        text.text = knockOutMessages[UnityEngine.Random.Range(0, knockOutMessages.Length - 1)];

        // Place the banner in front of the other gameobjects in the scene
        GetComponentInChildren<SpriteRenderer>().sortingLayerName = "SemiUI";
        text.GetComponent<MeshRenderer>().sortingLayerName = "SemiUI";

        // Choose the point that the stripe will go towards (roughly (0, 0))
        origin = new Vector3(UnityEngine.Random.Range(xVariation, -xVariation), UnityEngine.Random.Range(-yVariation, yVariation));

        // Multiply the position the player died by two to make sure the stripe starts offscreen
        originalPos = deathPos * 4;
        transform.position = originalPos;

        // Calculate the angle on the stripe to point towards the origin
        Vector3 direction = (origin - transform.position).normalized;
        float angle = (360 + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Calculate start distance to determine when to delete the stripe
        float x = transform.position.x - origin.x, y = transform.position.y - origin.y;
        startDist = (float)Math.Sqrt(x * x + y * y);

        // Vertically flip the text if it would be upside down otherwise
        if (angle >= 90 && angle <= 270)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 180));
        }
    }

    void UpdateRegularStrobe()
    {
        // Update timer
        regularStrobeTimer += Time.deltaTime;

        // Calculate the intensity of the strobe color
        float piece = regularStrobeTimer % (regularStrobeDuration * 2);
        float strobeIntensity = Math.Min(piece, regularStrobeDuration * 2 - piece) / regularStrobeDuration;

        // Smoothen the strobe effect (more time spent at the peaks)
        strobeIntensity *= strobeIntensity;

        text.color = Color.Lerp(baseColor, strobeColor, strobeIntensity);
    }

    void Kill()
    {
        Destroy(gameObject);
    }

    // TODO: remove deltaTime
    void Update()
    {
        // Calculate the distance from the origin, and delete the stripe when it's too far away
        float x = transform.position.x - origin.x, y = transform.position.y - origin.y;
        float c = (float)Math.Sqrt(x * x + y * y);
        if (c > startDist+1) { Kill(); }

        // Move the stripe towards the origin
        float dist = Math.Max(1f, c) * Time.deltaTime * speed;
        transform.position += (origin - originalPos).normalized * dist;

        UpdateRegularStrobe();
    }
}
