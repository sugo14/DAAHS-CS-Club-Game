using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionScript : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject levelSelection;

    void Start()
    {
        UpdateCharacters();
    }

    [ContextMenu("Update Characters")]
    public void UpdateCharacters()
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

    void Select(int index)
    {
        MatchSetupManagerScript.Instance.SelectCharacter(index, 0);
        TransitionManagerScript.Instance.AddTask(OnTransitionCallback);
        TransitionManagerScript.Instance.StartTransition();
    }

    private System.Collections.IEnumerator OnTransitionCallback()
    {
        gameObject.SetActive(false);
        levelSelection.SetActive(true);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
