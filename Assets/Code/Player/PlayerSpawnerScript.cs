using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerScript : MonoBehaviour
{
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

        cameraScript.FocalPoints.Remove(cameraScript.FocalPoints.Find(fp => fp == currPlayer));
        GameObject.Destroy(currPlayer);
        Respawn();
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
