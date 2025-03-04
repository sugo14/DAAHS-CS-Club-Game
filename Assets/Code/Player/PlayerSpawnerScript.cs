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
    public GameObject killStripePrefab;
    public GameObject platform;
    public GameObject spawnCircle;
    GameObject currPlayer;

    public float killDur = 1, killPow = 1;

    public float leftBound = 25, rightBound = 25, bottomBound = 15, topBound = 100;
    public float platformDur = 2, respawnTime = 2f;
    public int startStocks = 3;

    int currStocks = 0;

    float platformTimer;

    void Respawn()
    {
        // Initialize player
        currPlayer = Instantiate(playerPrefab, transform);
        Camera.main.GetComponent<CameraScript>().AddFocalPoint(currPlayer);
        currPlayer.GetComponent<AttackPhysicsScript>().playerSplashScript = splashScript;

        // Update platform and splash
        platformTimer = platformDur;
        splashScript.SetStocks(currStocks);
        splashScript.SetPercent(0);
    }

    IEnumerator OnKill()
    {
        CameraScript cameraScript = Camera.main.GetComponent<CameraScript>();

        // Kill effects
        cameraScript.BeginShake(killDur, killPow);
        cameraScript.BeginFreezeFrame(0.1f);
        GameObject killStripeInstance = Instantiate(killStripePrefab);
        killStripeInstance.GetComponent<KillStripeScript>().Place(currPlayer.transform.position);
        killStripeInstance.GetComponentInChildren<SpriteRenderer>().color = splashScript.backdropColor;

        // Delete player
        cameraScript.RemoveFocalPoint(currPlayer);
        cameraScript.AddFocalPoint(gameObject);
        Destroy(currPlayer);

        currStocks = Math.Max(0, currStocks - 1);
        splashScript.SetStocks(currStocks);
        spawnCircle.SetActive(true);
        AudioManager.PlaySound("Death1");

        yield return new WaitForSecondsRealtime(respawnTime);
        Respawn();
        cameraScript.RemoveFocalPoint(gameObject);
        spawnCircle.SetActive(false);
    }

    void Kill() { StartCoroutine(OnKill()); }

    void Reset()
    {
        currStocks = startStocks;
        Respawn();
    }

    public bool IsDead() { return currStocks <= 0; }

    void Start() { Reset(); }

    void Update()
    {
        // Animate the spawn circle
        float scale = 1.75f + Mathf.Sin(Time.time * 8) * 0.75f;
        spawnCircle.transform.localScale = new Vector3(scale, scale, scale);

        if (currPlayer == null) { return; }

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

        // TODO: don't need to do this every frame
        currPlayer.GetComponentInChildren<SpriteRenderer>().color = splashScript.backdropColor;
        spawnCircle.GetComponentInChildren<SpriteRenderer>().color = splashScript.backdropColor;
    }
}
