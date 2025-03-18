using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An unused script for creating a parallax background effect.
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] int layerCount = 9;
    [SerializeField] float depthFactor = 0.7f, moveFactor = 1f;
    [SerializeField] float baseWidth = 10f, baseHeight = 5f;
    [SerializeField] Color nearColor = Color.white;
    [SerializeField] Color farColor = Color.black;
    [SerializeField] GameObject squarePrefab;

    List<GameObject> layers = new List<GameObject>();

    void Start()
    {
        DeleteLayers();
        GenerateLayers();
    }

    void Update()
    {
        UpdateLayers();
    }

    void DeleteLayers()
    {
        for (int i = 0; i < layers.Count; i++) 
        { 
            if (layers[i] != null)
                Destroy(layers[i]); 
        }
        layers.Clear();
    }

    void GenerateLayers()
    {
        for (int i = 0; i < layerCount; i++)
        {
            GameObject instance = Instantiate(squarePrefab, transform);
            layers.Add(instance);

            float depth = Mathf.Pow(depthFactor, i);
            instance.transform.localScale = new Vector3(baseWidth * depth, baseHeight * depth, 1);

            Color color = Color.Lerp(nearColor, farColor, (float)i / (layerCount - 1));
            instance.GetComponent<SpriteRenderer>().color = color;
            instance.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
        }
    }

    void UpdateLayers()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Camera.main.backgroundColor = nearColor;

        for (int i = 0; i < layers.Count; i++)
        {
            float depth = Mathf.Pow(depthFactor, i);
            Vector3 parallaxPosition = cameraPos * moveFactor * (1 - depth);

            layers[i].transform.position = parallaxPosition;
        }
    }
}
