using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Vector3 originalScale;
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f);
    public float animationSpeed = 6;
    public int index;

    bool isHovered;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        isHovered = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
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
