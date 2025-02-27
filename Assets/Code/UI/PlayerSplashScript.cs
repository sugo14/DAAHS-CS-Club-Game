using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSplashScript : MonoBehaviour
{
    public Color backdropColor;
    public GameObject percentageTexts;
    public TMP_Text percentageText, percentageShadow;
    public List<GameObject> backdrops;

    public float percentUpdateSpeed = 130;
    public float freezeFactor = 0.1f, freezeDur = 0.1f;
    public float cameraShakeMagMult = 0.05f, cameraShakeDurMult = 0.1f;
    public float onHitStrobeDuration = 0.35f, regularStrobeDuration = 0.25f;

    public float upperBound;
    public Color lowerBoundColor, upperBoundColor, onHitColor;

    float percentage = 0, currPercent = 0, regularStrobeTimer = 0;
    Vector3 originalPos;
    Color baseColor;

    void Start()
    {
        originalPos = percentageTexts.transform.position;
    }

    public void SetPercent(int percentage)
    {
        this.percentage = percentage;
        float dif = percentage - currPercent;
        float curvedDif = (float)Math.Log(dif);

        StartCoroutine(ShakeEffect(curvedDif / 6f, curvedDif * 16f));
        StartCoroutine(StrobeEffect(onHitStrobeDuration, onHitColor));
        StartCoroutine(FreezeFrameEffect(freezeFactor * curvedDif, freezeDur));
        Camera.main.GetComponent<CameraScript>().BeginShake(cameraShakeMagMult * curvedDif, cameraShakeDurMult * curvedDif);
    }

    void UpdateCurrPercent()
    {
        float difference = Mathf.Abs(percentage - currPercent);
        float dynamicSpeed = percentUpdateSpeed * (difference / 10f);
        dynamicSpeed = Mathf.Max(dynamicSpeed, 0.1f);
        currPercent = Mathf.MoveTowards(currPercent, percentage, dynamicSpeed * Time.deltaTime);

        percentageText.text = Mathf.RoundToInt(currPercent) + "%";
        percentageShadow.text = Mathf.RoundToInt(currPercent) + "%";

        baseColor = Color.Lerp(lowerBoundColor, upperBoundColor, percentage / upperBound);
    }

    void UpdateRegularStrobe(Color strobeColor)
    {
        regularStrobeTimer += Time.deltaTime;
        float piece = regularStrobeTimer % (regularStrobeDuration * 2);
        float strobeIntensity = Math.Min(piece, regularStrobeDuration * 2 - piece) / regularStrobeDuration;
        strobeIntensity = strobeIntensity * strobeIntensity; // Smoothen it
        percentageText.color = Color.Lerp(baseColor, strobeColor, strobeIntensity);
    }

    IEnumerator ShakeEffect(float duration, float startMagnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float currentMagnitude = Mathf.Lerp(startMagnitude, 0f, elapsed / duration);
            float xOffset = UnityEngine.Random.Range(-currentMagnitude, currentMagnitude);
            float yOffset = UnityEngine.Random.Range(-currentMagnitude, currentMagnitude);
            percentageTexts.transform.position = originalPos + new Vector3(xOffset, yOffset, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        percentageTexts.transform.position = originalPos;
    }

    IEnumerator StrobeEffect(float duration, Color startColor)
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            regularStrobeTimer = 0; // Ensure regular strobe does not interrupt on-hit strobe
            Color currColor = Color.Lerp(startColor, baseColor, elapsed / duration);
            percentageText.color = currColor;

            elapsed += Time.deltaTime;
            yield return null;
        }

        percentageText.color = baseColor;
    }

    IEnumerator FreezeFrameEffect(float factor, float duration)
    {
        Time.timeScale = factor;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    void Update()
    {
        UpdateCurrPercent();
        UpdateRegularStrobe(Color.white);

        foreach (GameObject backdrop in backdrops)
        {
            backdrop.GetComponent<UnityEngine.UI.Image>().color = backdropColor;
        }
        percentageShadow.color = backdropColor;
    }
}
