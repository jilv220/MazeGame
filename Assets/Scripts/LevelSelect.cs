using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private int currentSceneIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnGUI()
    {
        GUI.matrix = Settings.SetupScaling();
        switch (currentSceneIdx)
        {
            case 0:
                GUILayout.Label("OBSTACLE COURSE");
                GUILayout.Label("Select a level to preview it");
                GUIManager.isLevelPreviewVisible = true;

                for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    if (GUILayout.Button("Level " + i))
                    {
                        SceneManager.LoadScene(i);
                        currentSceneIdx = i;
                    }
                }

                break;
            default:
                // Not main menu, which means loaded
                Destroy(gameObject);

                // LevelPreview is now handled by GUIManager
                break;
        }
    }
}
