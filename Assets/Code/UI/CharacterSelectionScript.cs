using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionScript : SelectionScript
{
    public GameObject levelSelection;

    public override void UpdateCells()
    {
        Character[] characters = MatchSetupManagerScript.Instance.characters.characters;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < characters.Length; i++)
        {
            GameObject cell = Instantiate(cellPrefab, transform);

            CellScript cellScript = cell.GetComponent<CellScript>();
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

        MatchSetupManagerScript.Instance.SelectCharacter(selected, 0);
        MenuManagerScript.Instance.PushState(MenuState.LevelSelection);
    }
}
