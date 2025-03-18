using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the setup of a match. Currently not used.
/// </summary>
public class MatchSetupManager : MonoBehaviour
{
    public static MatchSetupManager Instance { get; private set; }

    public LevelList levels;
    public CharacterList characters;

    [SerializeField] GameObject playerPrefab;
    
    int levelIndex;
    List<int> characterIndices;

    // Singleton
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void SelectLevel(int index)
    {
        Instance.levelIndex = index;
    }

    public static void SelectCharacter(int index, int player)
    {
        while (Instance.characterIndices.Count <= player)
        {
            Instance.characterIndices.Add(-1);
        }
        Instance.characterIndices[player] = index;
    }

    // Called externally to begin a match
    public static void BeginMatch()
    {
        MatchInfo matchInfo = new MatchInfo();
        matchInfo.level = Instance.levelIndex;
        TransitionManager.AddTask(() => Instance.LoadMatch(matchInfo));
        TransitionManager.StartTransition();
    }

    // Actually loads the scene (after scene transition entry is finished)
    IEnumerator LoadMatch(MatchInfo matchInfo)
    {
        void SceneLoadedCallback(Scene scene, LoadSceneMode mode)
        {
            OnSceneLoaded(matchInfo);
            SceneManager.sceneLoaded -= SceneLoadedCallback;
        }
        SceneManager.sceneLoaded += SceneLoadedCallback;
        SceneManager.LoadScene("FightScene");
        yield return null;
    }

    // Called when the scene is loaded to instantiate the player and level
    void OnSceneLoaded(MatchInfo matchInfo)
    {
        GameObject playerClone = Instantiate
        (
            playerPrefab,
            Vector3.zero,
            Quaternion.identity
        );

        GameObject levelClone = Instantiate
        (
            levels.levels[matchInfo.level].levelPrefab,
            new Vector3(0, -1f),
            Quaternion.identity
        );
        
        GameObject camera = GameObject.Find("Main Camera");
        StageCamera cameraScript = camera.GetComponent<StageCamera>();
        cameraScript.FocalPoints.Add(playerClone);
        cameraScript.FocalPoints.Add(levelClone);
    }
}
