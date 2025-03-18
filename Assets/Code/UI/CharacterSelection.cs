using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The character selection menu.
/// </summary>
public class CharacterSelection : UISelection
{
    public GameObject levelSelection;

    public override void UpdateCells()
    {
        Character[] characters = MatchSetupManager.Instance.characters.characters;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < characters.Length; i++)
        {
            GameObject cell = Instantiate(cellPrefab, transform);

            Cell cellScript = cell.GetComponent<Cell>();
            cellScript.index = i;

            Image background = cell.GetComponent<Image>();
            background.color = new Color(0, 0, 0, 255);

            GameObject cellImageObj = cell.transform.Find("CellImage").gameObject;
            Image cellImage = cellImageObj.GetComponent<Image>();
            cellImage.sprite = characters[i].sprite;
        }
    }

    public override void Select()
    {
        /* MatchSetupManagerScript.Instance.SelectCharacter(selected, 0);
        TransitionManagerScript.Instance.AddTask(OnTransitionCallback);
        TransitionManagerScript.Instance.StartTransition(); */

        MatchSetupManager.SelectCharacter(selected, 0);
        MenuManager.PushState(MenuState.LevelSelection);
    }
}
