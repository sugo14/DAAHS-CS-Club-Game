using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : SelectionScript
{
    List<MenuState> nextStates = new List<MenuState> { MenuState.CharacterSelection, MenuState.Settings };

    public override void UpdateCells()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < nextStates.Count; i++)
        {
            GameObject cell = Instantiate(cellPrefab, transform);

            CellScript cellScript = cell.GetComponent<CellScript>();
            cellScript.index = i;

            Image background = cell.GetComponent<Image>();
            /* background.color = new Color(0, 0, 0, 255); */

            TMP_Text cellText = cell.GetComponentInChildren<TMP_Text>();
            cellText.text = nextStates[i].ToString();
        }
    }

    public override void Select()
    {
        MenuManagerScript.Instance.PushState(nextStates[selected]);
    }
}
