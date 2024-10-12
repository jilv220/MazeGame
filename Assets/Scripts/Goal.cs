using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var currScene = SceneManager.GetActiveScene();
            var underscoreIdx = currScene.name.IndexOf('_');
            var sceneNum =
                Convert.ToInt32(currScene.name[(underscoreIdx + 1)..]);

            int totalScenes = SceneManager.sceneCountInBuildSettings;
            Debug.Log(totalScenes);

            var nextSceneNum = sceneNum + 1;
            Debug.Log(nextSceneNum);

            SoundManager.Instance.PlayLevelCompleteSound();
            // If last scene, go back to the frist one for now
            if (nextSceneNum > totalScenes)
            {
                SceneManager.LoadScene($"Level_{1}");
            }
            else
            {
                SceneManager.LoadScene($"Level_{sceneNum + 1}");
            }
        }
    }
}