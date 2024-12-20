using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScript : MonoBehaviour
{
    public LevelList levelList;
    public GameObject cellPrefab;

    void Start()
    {
        UpdateLevels();
    }

    [ContextMenu("Update Levels")]
    public void UpdateLevels()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < levelList.levels.Length; i++)
        {
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.GetComponent<CellElementScript>().index = i;
            GameObject cellImage = cell.transform.Find("CellImage").gameObject;
            cellImage.GetComponent<Image>().sprite = levelList.levels[i].sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
