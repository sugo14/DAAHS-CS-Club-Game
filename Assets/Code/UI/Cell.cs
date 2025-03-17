using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The cells in Selection menus.
/// </summary>
public class Cell : MonoBehaviour, IPointerClickHandler
{
    public int index;
    
    Vector3 originalScale;
    Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f);
    float animationSpeed = 6;

    bool isHovered = false;

    void Awake()
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
