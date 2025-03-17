using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public int layerCount = 9;
    public float depthFactor = 0.7f, moveFactor = 1f;
    public float baseWidth = 10f, baseHeight = 5f;
    public Color nearColor = Color.white;
    public Color farColor = Color.black;
    public GameObject squarePrefab;

    List<GameObject> layers = new List<GameObject>();

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

    void Start()
    {
        DeleteLayers();
        GenerateLayers();
    }

    void Update()
    {
        UpdateLayers();
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

            /* float scaleFactor = cameraSize / 5f;
            layers[i].transform.localScale = new Vector3(
                baseWidth * depth * scaleFactor, 
                baseHeight * depth * scaleFactor, 
                1
            ); */
        }
    }

}
