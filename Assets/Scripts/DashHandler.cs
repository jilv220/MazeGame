using UnityEngine;

public class DashHandler : MonoBehaviour
{
    public static DashHandler Instance { get; private set; }

    [Header("Dash Settings")]
    public float dashDistance = 17;
    public float dashTime = .26f;
    public float dashCooldown = 1.8f;

    private Color overlayColor;

    [HideInInspector] public float dashBeginTime = Mathf.NegativeInfinity;
    private Vector3 dashDirection;
    private CharacterController characterController;

    public bool IsDashing
    {
        get { return Time.time < dashBeginTime + dashTime; }
    }

    public bool CanDashNow
    {
        get { return Time.time > dashBeginTime + dashTime + dashCooldown; }
    }

    private bool dashEnabled = true;

    public void EnableDashing()
    {
        dashEnabled = true;
        UIManager.Instance.SetDashUIActive(true);
    }

    public void DisableDashing()
    {
        dashEnabled = false;
        UIManager.Instance.SetDashUIActive(false);
    }

    void UpdateCooldownUI()
    {
        var cooldownEndTime = dashBeginTime + dashTime + dashCooldown;
        var cooldownRemaining = cooldownEndTime - Time.time;

        if (cooldownRemaining > 0)
        {
            float fillAmount = cooldownRemaining / dashCooldown;
            UIManager.Instance.cooldownOverlay.fillAmount = fillAmount;
            UIManager.Instance.cooldownOverlay.color = overlayColor;
        }
        else
        {
            UIManager.Instance.cooldownOverlay.fillAmount = 0f;
            UIManager.Instance.cooldownOverlay.color = new Color(0, 0, 0, 0);
            UIManager.Instance.dashImage.color = UIManager.Instance.activeColor;
        }
    }

    public void HandleDash()
    {
        if (!dashEnabled)
            return;

        if (IsDashing)
        {
            characterController.Move(dashDistance / dashTime * Time.deltaTime * dashDirection);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && CanDashNow)
        {
            Vector3 movementDir = Vector3.zero;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                movementDir += Vector3.forward;
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                movementDir += Vector3.back;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                movementDir += Vector3.right;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                movementDir += Vector3.left;

            var player = GetComponent<Player>();
            if (movementDir == Vector3.zero)
                movementDir = player.modelTrans.forward;

            dashDirection = movementDir.normalized;
            dashBeginTime = Time.time;
            player.movementVelocity = dashDirection * player.moveSpeed;
        }
    }

    void Awake()
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

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (UIManager.Instance != null)
        {
            if (dashBeginTime == Mathf.NegativeInfinity)
            {
                UIManager.Instance.dashImage.fillAmount = 1f;
                UIManager.Instance.dashImage.color = UIManager.Instance.activeColor;

                // Save the color
                overlayColor = UIManager.Instance.cooldownOverlay.color;
                UIManager.Instance.cooldownOverlay.fillAmount = 0f;
                UIManager.Instance.cooldownOverlay.color = new Color(0, 0, 0, 0);
            }
            else
            {
                UpdateCooldownUI();
            }
        }
    }

    void Update()
    {
        UpdateCooldownUI();
        HandleDash();
    }
}

