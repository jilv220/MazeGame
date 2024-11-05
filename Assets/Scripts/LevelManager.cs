using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static Dictionary<int, Vector3> SpawnPoints = new()
    {
        { 1, new(17, 0, -2) },
        { 2, new(23, 0, -15) }
    };

    public static int GetCurrentLevel()
    {
        var currScene = SceneManager.GetActiveScene();
        var sceneName = currScene.name;
        if (sceneName == "Main")
            return 0;

        var underscoreIdx = sceneName.IndexOf('_');

        // Check if underscore exists
        if (underscoreIdx == -1)
        {
            Debug.LogError(
                $"GetCurrentLevel: Scene name '{sceneName}' does not contain an underscore '_'. Unable to determine level number."
            );
            return -1;
        }

        // Ensure there are characters after the underscore
        if (underscoreIdx + 1 >= sceneName.Length)
        {
            Debug.LogError(
                $"GetCurrentLevel: Scene name '{sceneName}' ends with an underscore '_'. No level number found."
            );
            return -1;
        }

        string levelString = sceneName[(underscoreIdx + 1)..];
        if (int.TryParse(levelString, out int levelNumber))
        {
            return levelNumber;
        }
        else
        {
            Debug.LogError(
                $"GetCurrentLevel: Unable to parse level number from substring '{levelString}' in scene name '{sceneName}'."
            );
            return -1;
        }
    }

    public static int GetNextLevel()
    {
        var currLevel = GetCurrentLevel();
        if (currLevel == -1)
            return -1;
        else
            return currLevel + 1;
    }
}
