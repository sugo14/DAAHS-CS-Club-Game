using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerSelectScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject levelPrefab;

    public void BeginFightScene() {
        SceneManager.sceneLoaded += (scene, mode) => {
            beginFightScene();
        };
        SceneManager.LoadScene("FightScene");
    }

    void beginFightScene() {
        GameObject playerClone = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        GameObject levelClone = Instantiate(levelPrefab, new Vector3(0, -3), Quaternion.identity);
        
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
