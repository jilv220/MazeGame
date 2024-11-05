using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private int sceneNum = -1;

    void Start()
    {
        var currScene = SceneManager.GetActiveScene();
        var underscoreIdx = currScene.name.IndexOf('_');
        sceneNum =
            Convert.ToInt32(currScene.name[(underscoreIdx + 1)..]);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int totalScenes = SceneManager.sceneCountInBuildSettings;
            Debug.Log(totalScenes);

            var nextSceneNum = sceneNum + 1;
            Debug.Log(nextSceneNum);

            SoundManager.Instance.PlayOneShot("level_complete");

            // If last scene, go back to the main menu
            if (nextSceneNum > totalScenes - 1)
            {
                SceneManager.LoadScene("Main");
            }
            else
            {
                SceneManager.LoadScene(nextSceneNum);
            }

            // Display Level Preview and hide Game UI
            GUIManager.isLevelPreviewVisible = true;

            var playerGobj = GameObject.Find("Player");
            var dashHandler = playerGobj.GetComponent<DashHandler>();
            dashHandler.enabled = false;
            UIManager.Instance.SetDashUIActive(false);
        }
    }

    void OnGUI()
    {
        GUIManager.DisplayLevelPreview(sceneNum);
    }
}