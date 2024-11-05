using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public static bool isLevelPreviewVisible = true;

    static void PlayCurrentLevel()
    {
        // Enable Player
        if (Player.Instance)
        {
            var currentLevel = LevelManager.GetCurrentLevel();
            Debug.Log($"GUIManager: current level is: {currentLevel}");

            Player.Instance.ResetAttrs();
            Player.Instance.SetSpawnPoint(currentLevel);
            Player.Instance.Show();
            Player.Instance.SetEnabled(true);
        }
        else
        {
            Debug.LogError("Player instance not found!");
        }

        // Enable dashing functionality via DashHandler
        if (DashHandler.Instance != null)
        {
            DashHandler.Instance.EnableDashing();
        }
        else
        {
            Debug.LogError("DashHandler instance not found!");
        }
    }

    public static void ToggleLevelPreviewVisability()
    {
        isLevelPreviewVisible = !isLevelPreviewVisible;
    }

    public static void DisplayLevelPreview(int levelNum)
    {
        var style = new GUIStyle(GUI.skin.label);
        style.normal.textColor = Color.black;
        style.hover.textColor = Color.black;

        if (isLevelPreviewVisible)
        {
            GUILayout.Label($"Currently Viewing Level {levelNum}", style);
            if (GUILayout.Button("PLAY"))
            {
                PlayCurrentLevel();
                isLevelPreviewVisible = false;
            }
        }
    }
}
