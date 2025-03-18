using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns and kills a player character in a match.
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    public PlayerSplash splashScript;
    public GameObject playerPrefab;
    public GameObject killStripePrefab;
    public GameObject platform;
    public GameObject spawnCircle;
    public Color playerTint;

    [SerializeField] float killDur = 1, killPow = 1;

    [SerializeField] float leftBound = 25, rightBound = 25, bottomBound = 15, topBound = 100;
    [SerializeField] float platformDur = 2, respawnTime = 2f;
    [SerializeField] int startStocks = 3;

    GameObject currPlayer;
    int currStocks = 0;
    float platformTimer;

    void Start() { Reset(); }

    void Update()
    {
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
            platform.GetComponent<PlatformBehavior>().SetColor(splashScript.backdropColor);
            currPlayer.GetComponentInChildren<SpriteRenderer>().color = splashScript.backdropColor;
            spawnCircle.GetComponentInChildren<SpriteRenderer>().color = splashScript.backdropColor;
            platform.SetActive(true);
        }
        else { platform.SetActive(false); }
    }

    public bool IsDead() { return currStocks <= 0; }

    void Respawn()
    {
        // Initialize player
        currPlayer = Instantiate(playerPrefab, transform);
        Camera.main.GetComponent<StageCamera>().AddFocalPoint(currPlayer);
        currPlayer.GetComponent<AttackPhysics>().playerSplash = splashScript;
        currPlayer.layer = LayerMask.NameToLayer("Players");
        currPlayer.GetComponentInChildren<SpriteRenderer>().color = playerTint;
        
        // Initialize platform and splash
        platformTimer = platformDur;
        splashScript.SetStocks(currStocks);
        splashScript.SetPercent(0);
    }

    void Kill() { StartCoroutine(OnKill()); }

    void Reset()
    {
        currStocks = startStocks;
        Respawn();
    }

    IEnumerator OnKill()
    {
        StageCamera cameraScript = Camera.main.GetComponent<StageCamera>();

        // Kill effects
        cameraScript.BeginShake(killDur, killPow);
        cameraScript.BeginFreezeFrame(0.1f);
        GameObject killStripeInstance = Instantiate(killStripePrefab);
        killStripeInstance.GetComponent<KillStripe>().Initialize(currPlayer.transform.position);
        killStripeInstance.GetComponentInChildren<SpriteRenderer>().color = splashScript.backdropColor;
        AudioManager.PlaySound("Death1");

        // Delete player and update stocks
        cameraScript.RemoveFocalPoint(currPlayer);
        Destroy(currPlayer);
        currStocks = Math.Max(0, currStocks - 1);

        // Update splash
        splashScript.SetStocks(currStocks);
        splashScript.SetPercent(0);

        // Respawn effects
        if (currStocks > 0)
        {
            AudioManager.PlaySound("Respawn1");
            cameraScript.AddFocalPoint(gameObject);
            spawnCircle.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(respawnTime);

        // Respawn player
        if (currStocks > 0)
        {
            Respawn();
            cameraScript.RemoveFocalPoint(gameObject);
            spawnCircle.SetActive(false);
        }
    }
}
