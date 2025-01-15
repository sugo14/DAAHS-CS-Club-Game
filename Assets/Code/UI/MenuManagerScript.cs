using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Internal;

public enum MenuState {
    MainMenu,
    LevelSelection,
    CharacterSelection,
    Settings
}

public class MenuManagerScript : MonoBehaviour
{
    public GameObject[] menus;
    public static MenuManagerScript Instance { get; private set; }

    Stack<MenuState> stateStack = new Stack<MenuState>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PushState(MenuState state)
    {
        Debug.Log("Pushing state " + state.ToString());
        stateStack.Push(state);
        OpenMenu();
    }

    public void PopState()
    {
        if (stateStack.Count <= 1) { return; }
        Debug.Log("Popping state " + stateStack.Peek().ToString());
        stateStack.Pop();
        OpenMenu();
    }

    void Start()
    {
        stateStack.Push(MenuState.MainMenu);
        OpenMenu();
    }

    void OpenMenu()
    {
        TransitionManagerScript.Instance.AddTask(SetActiveMenus);
        TransitionManagerScript.Instance.OnTransitionEnd += AcceptInput;
        TransitionManagerScript.Instance.StartTransition();
    }

    System.Collections.IEnumerator SetActiveMenus()
    {
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
            menus[i].GetComponent<SelectionScript>().AcceptInput((MenuState)i == stateStack.Peek());
        }
    }
}
