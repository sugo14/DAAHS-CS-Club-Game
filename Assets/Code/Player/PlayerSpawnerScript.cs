using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawnerScript : MonoBehaviour
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
        "Well done.",
        "TAKE THAT!!!",
        "EJECTED!!!",
        "GAME OVER!!!",
        "NEVER GONNA GIVE YOU UP!!! " +
        "NEVER GONNA LET YOU DOWN!!! " +
        "NEVER GONNA RUN AROUND AND DESERT YOU!!! " +
        "NEVER GONNA"
    };

    public GameObject playerPrefab;
    public PlayerSplashScript splashScript;
    public GameObject killStripe;
    GameObject currPlayer;

    public float killDur = 1, killPow = 1;

    public float leftBound = 25, rightBound = 25, bottomBound = 15, topBound = 100;

    void Respawn()
    {
        currPlayer = Instantiate(playerPrefab, transform);
        Camera.main.GetComponent<CameraScript>().FocalPoints.Add(currPlayer);
        currPlayer.GetComponent<AttackPhysicsScript>().playerSplashScript = splashScript;
    }

    void Kill()
    {
        CameraScript cameraScript = Camera.main.GetComponent<CameraScript>();

        // Kill effects
        cameraScript.BeginShake(killDur, killPow);
        cameraScript.BeginFreezeFrame(0.1f);
        GameObject killStripeInstance = Instantiate(killStripe);
        killStripeInstance.GetComponent<KillStripeScript>().Place(currPlayer.transform.position);
        killStripeInstance.GetComponentInChildren<SpriteRenderer>().color = splashScript.backdropColor;
        killStripeInstance.GetComponentInChildren<TMP_Text>().text = knockOutMessages[UnityEngine.Random.Range(0, knockOutMessages.Count - 1)];
        MeshRenderer meshRenderer = killStripeInstance.GetComponentInChildren<MeshRenderer>();
        meshRenderer.sortingLayerName = "Default"; // Or whatever layer your sprites use
        meshRenderer.sortingOrder = 0; // Match your sprites' sorting order
        killStripeInstance.GetComponentInChildren<TMP_Text>().fontMaterial.renderQueue = 3000; // Default render queue for opaque sprites

        cameraScript.FocalPoints.Remove(cameraScript.FocalPoints.Find(fp => fp == currPlayer));
        GameObject.Destroy(currPlayer);
        Respawn();

        splashScript.SetPercent(0);
    }

    void Start() { Respawn(); }

    void Update()
    {
        Vector3 currPos = currPlayer.transform.position;
        if (currPos.x < -leftBound) { Kill(); }
        else if (currPos.x > rightBound) { Kill(); }
        else if (currPos.y < -bottomBound) { Kill(); }
        else if (currPos.y > topBound) { Kill(); }
    }
}
