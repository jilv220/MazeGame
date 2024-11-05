using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Image dashImage;
    public Image cooldownOverlay;
    public Color activeColor = new Color(1f, 1f, 1f, 1f);

    public void SetDashUIActive(bool isActive)
    {
        dashImage.gameObject.SetActive(isActive);
        cooldownOverlay.gameObject.SetActive(isActive);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

