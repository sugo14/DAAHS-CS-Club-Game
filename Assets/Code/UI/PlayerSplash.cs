using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The UI splash for each battling player, primarily showing percentage and stocks.
/// </summary>
public class PlayerSplash : MonoBehaviour
{
    public Color backdropColor;
    public GameObject percentageTexts;
    public TMP_Text percentageText, percentageShadow;
    public List<GameObject> backdrops;
    public GameObject stockPrefab;
    public GameObject stocksObject;

    public float percentUpdateSpeed = 130;
    public float freezeDur = 0.1f;
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
        // Set percentage and ensure it is increasing
        float lastPercent = this.percentage;
        this.percentage = percentage;
        if (lastPercent >= percentage)
        {
            return;
        }

        // Calculate the difference in percentages and apply effects
        float dif = percentage - lastPercent;
        float curvedDif = (float)Math.Log(dif);
        StageCamera cameraScript = Camera.main.GetComponent<StageCamera>();
        StartCoroutine(ShakeEffect(curvedDif / 6f, curvedDif * 16f));
        StartCoroutine(StrobeEffect(onHitStrobeDuration, onHitColor));
        cameraScript.BeginFreezeFrame(freezeDur * curvedDif);
        cameraScript.BeginShake(cameraShakeMagMult * curvedDif, cameraShakeDurMult * curvedDif);
    }

    // Smoothly increases displayed percentage until it matches the current percentage of the character
    void UpdateCurrPercent()
    {
        // Determine next percentage number based on the difference in percentages
        float difference = Mathf.Abs(percentage - currPercent);
        float dynamicSpeed = percentUpdateSpeed * (difference / 10f);
        dynamicSpeed = Mathf.Max(dynamicSpeed, 0.1f);
        currPercent = Mathf.MoveTowards(currPercent, percentage, dynamicSpeed * Time.deltaTime);

        // Update text
        percentageText.text = Mathf.RoundToInt(currPercent) + "%";
        percentageShadow.text = Mathf.RoundToInt(currPercent) + "%";

        baseColor = Color.Lerp(lowerBoundColor, upperBoundColor, percentage / upperBound);
    }

    void UpdateRegularStrobe(Color strobeColor)
    {
        // Update timer
        regularStrobeTimer += Time.deltaTime;

        // Calculate the intensity of the strobe color
        float piece = regularStrobeTimer % (regularStrobeDuration * 2);
        float strobeIntensity = Math.Min(piece, regularStrobeDuration * 2 - piece) / regularStrobeDuration;

        // Smoothen the strobe effect (more time spent at the peaks)
        strobeIntensity *= strobeIntensity;

        percentageText.color = Color.Lerp(baseColor, strobeColor, strobeIntensity);
    }

    IEnumerator ShakeEffect(float duration, float startMagnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Calculate the current maximum magnitude and apply a random offset within it
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
            // Ensure regular strobe does not interrupt on-hit strobe
            regularStrobeTimer = 0;

            // Linear interpolation between the start color and the base color
            Color currColor = Color.Lerp(startColor, baseColor, elapsed / duration);
            percentageText.color = currColor;

            elapsed += Time.deltaTime;
            yield return null;
        }

        percentageText.color = baseColor;
    }

    public void SetStocks(int stockCount)
    {
        int currentCount = stocksObject.transform.childCount;

        // Delete stocks if there are too many
        if (currentCount > stockCount)
        {
            for (int i = currentCount - 1; i >= stockCount; i--)
            {
                Destroy(stocksObject.transform.GetChild(i).gameObject);
            }
        }

        // Add stocks if there are too few
        else if (currentCount < stockCount)
        {
            int itemsToAdd = stockCount - currentCount;
            for (int i = 0; i < itemsToAdd; i++)
            {
                Instantiate(stockPrefab, stocksObject.transform);
            }
        }
    }

    void Update()
    {
        UpdateCurrPercent();
        UpdateRegularStrobe(Color.white);

        // TODO: this does not need to be done each frame
        foreach (GameObject backdrop in backdrops)
        {
            backdrop.GetComponent<UnityEngine.UI.Image>().color = backdropColor;
        }
        percentageShadow.color = backdropColor;
    }
}
