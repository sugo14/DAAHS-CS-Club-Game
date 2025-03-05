using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchManagerScript : MonoBehaviour
{
    public PlayerSpawnerScript player1Spawner, player2Spawner;
    public GameObject text;
    public Image backdrop;

    bool ending;
    
    void Start()
    {
        ending = false;
        transform.localScale = new Vector3(0, 0, 0);
    }

    // this is incredibly hacky
    IEnumerator OnGameEnd(int winner)
    {
        AudioManager.PlaySound("Victory1");
        if (winner == 1)
        {
            foreach (TMP_Text txt in text.GetComponentsInChildren<TMP_Text>())
            {
                txt.text = "PLAYER 1 WIN!!!";
            }
            backdrop.color = player1Spawner.splashScript.backdropColor;
        }
        if (winner == 2)
        {
            foreach (TMP_Text txt in text.GetComponentsInChildren<TMP_Text>())
            {
                txt.text = "PLAYER 2 WIN!!!";
            }
            backdrop.color = player2Spawner.splashScript.backdropColor;
        }

        float elapsed = 0f, duration = 2f;
        float startScale = 0f, endScale = 1f;
        
        while (elapsed < duration)
        {
            Time.timeScale = 0f;
            startScale = Mathf.Lerp(startScale, endScale, elapsed / duration);
            transform.localScale = new Vector3(startScale, startScale, startScale);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        while (!Input.GetKey(KeyCode.Space))
        {
            yield return null;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    void GameEnd(int winner)
    {
        StartCoroutine(OnGameEnd(winner));
    }

    void Update()
    {
        float sine = (float)Math.Sin(Time.unscaledTime * 2.5);
        sine *= sine * sine;
        sine += 1;
        sine = 0.75f + sine / 4f;
        text.transform.localScale = new Vector3(sine, sine, sine);

        if (ending) { return; }
        if (player1Spawner.IsDead())
        {
            GameEnd(2);
            ending = true;
        }
        else if (player2Spawner.IsDead())
        {
            GameEnd(1);
            ending = true;
        }
    }
}
