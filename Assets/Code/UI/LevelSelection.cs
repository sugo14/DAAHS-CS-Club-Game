using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The level selection menu.
/// </summary>
public class LevelSelectionScript : UISelection
{
    public override void UpdateCells()
    {
        Level[] levels = MatchSetupManager.Instance.levels.levels;

        // Delete old cells
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Create new cells
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.GetComponent<Cell>().index = i;
            GameObject cellImage = cell.transform.Find("CellImage").gameObject;
            cellImage.GetComponent<Image>().sprite = levels[i].sprite;
        }
    }

    public override void Select()
    {
        MatchSetupManager.SelectLevel(selected);
        MatchSetupManager.BeginMatch();
    }
}
