using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScript : SelectionScript
{
    public override void UpdateCells()
    {
        Level[] levels = MatchSetupManagerScript.Instance.levels.levels;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < levels.Length; i++)
        {
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.GetComponent<CellScript>().index = i;
            GameObject cellImage = cell.transform.Find("CellImage").gameObject;
            cellImage.GetComponent<Image>().sprite = levels[i].sprite;
        }
    }

    public override void Select()
    {
        MatchSetupManagerScript.Instance.SelectLevel(selected);
        MatchSetupManagerScript.Instance.BeginMatch();
    }
}
