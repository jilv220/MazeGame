using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public static bool isLevelPreviewVisible = true;
    static void PlayCurrentLevel()
    {
        var playerGobj = GameObject.Find("Player");
        if (playerGobj == null)
        {
            Debug.LogError("Couldn't find player in the level!");
            return;
        }

        var playerScript = playerGobj.GetComponent<Player>();
        playerScript.enabled = true;
        playerScript.cam.SetActive(true);

        var dashHandler = playerGobj.GetComponent<DashHandler>();
        dashHandler.enabled = true;
        UIManager.Instance.SetDashUIActive(true);
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
