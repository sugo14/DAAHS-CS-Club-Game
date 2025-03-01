using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawnerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public PlayerSplashScript splashScript;
    public GameObject killStripe;
    public GameObject platform;
    GameObject currPlayer;

    public float killDur = 1, killPow = 1;

    public float leftBound = 25, rightBound = 25, bottomBound = 15, topBound = 100;
    public float platformDur = 2;
    public int startStocks = 3;

    int currStocks = 0;

    float platformTimer;

    void Respawn()
    {
        // Initialize player
        currPlayer = Instantiate(playerPrefab, transform);
        Camera.main.GetComponent<CameraScript>().FocalPoints.Add(currPlayer);
        currPlayer.GetComponent<AttackPhysicsScript>().playerSplashScript = splashScript;

        // Update platform and splash
        platformTimer = platformDur;
        splashScript.SetStocks(currStocks);
        splashScript.SetPercent(0);
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

        // Delete player
        GameObject instance = cameraScript.FocalPoints.Find(fp => fp == currPlayer);
        if (instance != null)
        {
            cameraScript.FocalPoints.Remove(instance);
        }
        GameObject.Destroy(currPlayer);

        currStocks = Math.Max(0, currStocks - 1);
        Respawn();
    }

    void Reset()
    {
        currStocks = startStocks;
        Respawn();
    }

    public bool IsDead() { return currStocks <= 0; }

    void Start() { Reset(); }

    void Update()
    {
        // Kill player if out of bounds
        Vector3 currPos = currPlayer.transform.position;
        if (currPos.x < -leftBound) { Kill(); }
        else if (currPos.x > rightBound) { Kill(); }
        else if (currPos.y < -bottomBound) { Kill(); }
        else if (currPos.y > topBound) { Kill(); }

        // Enable platform if player recently respawned
        if (platformTimer > 0)
        {
            platformTimer -= Time.deltaTime;
            platform.GetComponentInChildren<SpriteRenderer>().color = splashScript.backdropColor;
            platform.SetActive(true);
        }
        else { platform.SetActive(false); }
    }
}
