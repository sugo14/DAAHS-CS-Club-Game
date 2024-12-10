using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerSelectScript : MonoBehaviour
{
    public GameObject playerPrefab;

    public List<GameObject> levels;

    public void BeginLevel1() {
        BeginFightScene(0);
    }
    public void BeginLevel2() {
        BeginFightScene(1);
    }

    void BeginFightScene(int i) {
        SceneManager.sceneLoaded += (scene, mode) => {
            OnSceneLoaded(i);
        };
        SceneManager.LoadScene("FightScene");
    }

    void OnSceneLoaded(int i) {
        GameObject playerClone = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        GameObject levelClone = Instantiate(levels[i], new Vector3(0, -3), Quaternion.identity);
        
        GameObject camera = GameObject.Find("Main Camera");
        CameraScript cameraScript = camera.GetComponent<CameraScript>();

        cameraScript.FocalPoints.Add(playerClone);
        cameraScript.FocalPoints.Add(levelClone);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
