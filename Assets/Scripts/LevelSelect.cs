using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private int currentSceneIdx = 0;
    private GameObject levelViewCamera;
    private AsyncOperation currentLoadOpeartion;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLoadOpeartion == null || !currentLoadOpeartion.isDone)
            return;

        currentLoadOpeartion = null;
        levelViewCamera = GameObject.Find("Level View Camera");
        if (levelViewCamera == null)
            Debug.LogError("No level view camera found in the scene!");
    }

    void OnGUI()
    {
        switch (currentSceneIdx)
        {
            case 0:
                GUILayout.Label("OBSTACLE COURSE");
                GUILayout.Label("Select a level to preview it");
                for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    if (GUILayout.Button("Level " + i))
                    {
                        if (currentLoadOpeartion == null)
                        {
                            currentLoadOpeartion = SceneManager.LoadSceneAsync(i);
                            currentSceneIdx = i;
                        }
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
