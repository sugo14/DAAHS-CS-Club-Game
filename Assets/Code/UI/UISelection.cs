using UnityEngine;

/// <summary>
/// The base class for selection menus.
/// </summary>
public abstract class UISelection : MonoBehaviour
{
    public GameObject cellPrefab;
    public int selected;

    PlayerInput playerInput;

    public void AcceptInput(bool state)
    {
        if (playerInput == null) { playerInput = new PlayerInput(); }
        if (state) { playerInput.Enable(); }
        else { playerInput.Disable(); }
    }

    void Start()
    {
        selected = 0;
        UpdateCells();
    }

    void UpdateHover()
    {
        foreach (Transform child in transform)
        {
            Cell cell = child.GetComponent<Cell>();
            if (cell.index == selected)
            {
                cell.Hover();
            }
            else
            {
                cell.Unhover();
            }
        }
    }

    void Update()
    {
        if (playerInput == null) { return; }
        if (playerInput.UI.Navigate.triggered)
        {
            Vector2 move = playerInput.UI.Navigate.ReadValue<Vector2>();
            if (move.x > 0.5f)
            {
                selected = (selected + 1) % transform.childCount;
            }
            else if (move.x < -0.5f)
            {
                selected = (selected - 1 + transform.childCount) % transform.childCount;
            }
        }
        UpdateHover();
        if (playerInput.UI.Select.triggered)
        {
            Select();
        }
    }

    [ContextMenu("Update Cells")]
    public abstract void UpdateCells();

    [ContextMenu("Select")]
    public abstract void Select();
}
