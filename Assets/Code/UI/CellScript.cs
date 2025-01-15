using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellScript : MonoBehaviour, IPointerClickHandler
{
    public int index;
    
    Vector3 originalScale;
    Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f);
    float animationSpeed = 6;

    bool isHovered = false;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    public void Hover()
    {
        isHovered = true;
    }

    public void Unhover()
    {
        isHovered = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SendMessageUpwards("Select", index);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetScale = isHovered ? hoverScale : originalScale;
        transform.localScale = Vector3.Lerp
        (
            transform.localScale,
            targetScale,
            animationSpeed * Time.deltaTime
        );
    }
}
