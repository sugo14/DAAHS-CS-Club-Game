using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The states that the MenuManager can be in.
/// </summary>
public enum MenuState
{
    MainMenu,
    LevelSelection,
    CharacterSelection,
    Settings
}

/// <summary>
/// Manages the game's Selection menus.
/// </summary>
public class MenuManager : MonoBehaviour
{
    public GameObject[] menus;
    public static MenuManager Instance { get; private set; }

    Stack<MenuState> stateStack = new Stack<MenuState>();

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public static void PushState(MenuState state)
    {
        Debug.Log("Pushing state " + state.ToString());
        Instance.stateStack.Push(state);
        Instance.OpenMenu();
    }

    public static void PopState()
    {
        if (Instance.stateStack.Count <= 1) { return; }
        Debug.Log("Popping state " + Instance.stateStack.Peek().ToString());
        Instance.stateStack.Pop();
        Instance.OpenMenu();
    }

    void Start()
    {
        PushState(MenuState.MainMenu);
    }

    void OpenMenu()
    {
        TransitionManager.AddTask(SetActiveMenus);
        TransitionManager.Instance.OnTransitionEnd += AcceptInput;
        TransitionManager.StartTransition();
    }

    System.Collections.IEnumerator SetActiveMenus()
    {
        // Set only the top menu active
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive((MenuState)i == stateStack.Peek());
        }
        yield return null;
    }

    void AcceptInput()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            Debug.Log(i);
            menus[i].GetComponent<UISelection>().AcceptInput((MenuState)i == stateStack.Peek());
        }
    }
}
