using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The on-game-start main menu.
/// </summary>
public class MainMenu : UISelection
{
    List<MenuState> nextStates = new List<MenuState> { MenuState.CharacterSelection, MenuState.Settings };

    public override void UpdateCells()
    {
        // Delete old cells
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Create new cells
        for (int i = 0; i < nextStates.Count; i++)
        {
            GameObject cell = Instantiate(cellPrefab, transform);

            Cell cellScript = cell.GetComponent<Cell>();
            cellScript.index = i;

            Image background = cell.GetComponent<Image>();
            /* background.color = new Color(0, 0, 0, 255); */

            TMP_Text cellText = cell.GetComponentInChildren<TMP_Text>();
            cellText.text = nextStates[i].ToString();
        }
    }

    public override void Select()
    {
        MenuManager.PushState(nextStates[selected]);
    }
}
