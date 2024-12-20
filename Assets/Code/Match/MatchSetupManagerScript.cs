using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchSetupManagerScript : MonoBehaviour
{
    public static MatchSetupManagerScript Instance { get; private set; }
    public GameObject playerPrefab;
    public List<AttackScript> characters; // temp, will replace with character scriptable object list
    public LevelList levels;

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

    // Called externally to begin a match
    public void BeginMatch(int i)
    {
        MatchInfo matchInfo = new MatchInfo();
        matchInfo.level = i;
        TransitionManagerScript.Instance.AddTask(() => LoadMatch(matchInfo));
        TransitionManagerScript.Instance.StartTransition();
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
        CameraScript cameraScript = camera.GetComponent<CameraScript>();
        cameraScript.FocalPoints.Add(playerClone);
        cameraScript.FocalPoints.Add(levelClone);
    }
}
