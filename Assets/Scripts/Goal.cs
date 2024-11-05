using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private int sceneNum = -1;

    void Start()
    {
        sceneNum = LevelManager.GetCurrentLevel();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int totalScenes = SceneManager.sceneCountInBuildSettings;
            Debug.Log(totalScenes);

            var nextSceneNum = LevelManager.GetNextLevel() % totalScenes;
            Debug.Log(nextSceneNum);

            SoundManager.Instance.PlayOneShot("level_complete");
            SceneManager.LoadScene(nextSceneNum);

            // Display Level Preview and hide Game UI
            GUIManager.isLevelPreviewVisible = true;

            if (DashHandler.Instance != null)
            {
                DashHandler.Instance.DisableDashing();
            }
            else
            {
                Debug.LogError("Dashhandler Instance not found!");
            }

            if (Player.Instance != null)
            {
                Player.Instance.Hide();
            }
        }
    }

    void OnGUI()
    {
        GUI.matrix = Settings.SetupScaling();
        GUIManager.DisplayLevelPreview(sceneNum);
    }
}