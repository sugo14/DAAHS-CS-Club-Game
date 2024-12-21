using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScript : MonoBehaviour
{
    public GameObject cellPrefab;

    void Start()
    {
        UpdateLevels();
    }

    [ContextMenu("Update Levels")]
    public void UpdateLevels()
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

    void Select(int index)
    {
        MatchSetupManagerScript.Instance.SelectLevel(index);
        MatchSetupManagerScript.Instance.BeginMatch();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
