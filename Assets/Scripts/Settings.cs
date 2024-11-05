using System.Runtime.CompilerServices;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static float referenceWidth = 800f;
    public static float referenceHeight = 600f;

    public static Matrix4x4 SetupScaling()
    {
        // Calculate scale factor based on the current screen resolution
        float scaleX = Screen.width / referenceWidth;
        float scaleY = Screen.height / referenceHeight;
        float scale = Mathf.Min(scaleX, scaleY); // Use the smaller scale factor to maintain aspect ratio

        // Apply scaling
        return Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(scale, scale, 1));
    }
}