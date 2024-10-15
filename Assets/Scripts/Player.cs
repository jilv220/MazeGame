using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    public Transform trans;
    public Transform modelTrans;
    public CharacterController characterController;

    [Header("Movement")]
    public float moveSpeed = 24;
    public float timeToMaxSpeed = .26f;
    private float VelocityGainPerSecond
    {
        get
        {
            return moveSpeed / timeToMaxSpeed;
        }
    }

    public float timeToLoseMaxSpeed = .2f;
    private float VelocityLossPerSecond
    {
        get
        {
            return moveSpeed / timeToLoseMaxSpeed;
        }
    }

    public float reverseMomentumMultiplier = 2.2f;
    private Vector3 movementVelocity = Vector3.zero;

    [Header("Death and Respawning")]
    public float respawnWaitTime = 2f;
    private bool dead = false;
    private Vector3 spawnPoint;
    private Quaternion spawnRotation;

    private bool isInvincible = false;

    void UpdateVelocity(ref float axisVelocity, KeyCode[] positiveKeys, KeyCode[] negativeKeys)
    {
        bool positivePressed = positiveKeys.Any(Input.GetKey);
        bool negativePressed = negativeKeys.Any(Input.GetKey);

        if (positivePressed)
        {
            if (axisVelocity >= 0)
                axisVelocity = Mathf.Min(moveSpeed, axisVelocity + VelocityGainPerSecond * Time.deltaTime);
            else
                axisVelocity = Mathf.Min(0, axisVelocity + VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
        }
        else if (negativePressed)
        {
            if (axisVelocity > 0)
                axisVelocity = Mathf.Max(0, axisVelocity - VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
            else
                axisVelocity = Mathf.Max(-moveSpeed, axisVelocity - VelocityGainPerSecond * Time.deltaTime);
        }
        else
        {
            if (axisVelocity > 0)
                axisVelocity = Mathf.Max(0, axisVelocity - VelocityLossPerSecond * Time.deltaTime);
            else
                axisVelocity = Mathf.Min(0, axisVelocity + VelocityLossPerSecond * Time.deltaTime);
        }
    }

    private void Movement()
    {
        KeyCode[] upKeys = { KeyCode.W, KeyCode.UpArrow };
        KeyCode[] downKeys = { KeyCode.S, KeyCode.DownArrow };
        UpdateVelocity(ref movementVelocity.z, upKeys, downKeys);

        KeyCode[] rightKeys = { KeyCode.D, KeyCode.RightArrow };
        KeyCode[] leftKeys = { KeyCode.A, KeyCode.LeftArrow };
        UpdateVelocity(ref movementVelocity.x, rightKeys, leftKeys);

        Vector3 movementInput = new(movementVelocity.x, 0, movementVelocity.z);
        if (movementInput.magnitude > 1)
            movementInput.Normalize();

        // Apply Movement and Smooth Rotation
        if (movementInput != Vector3.zero)
        {
            characterController.Move(moveSpeed * Time.deltaTime * movementInput);
            modelTrans.rotation = Quaternion.Slerp(modelTrans.rotation, Quaternion.LookRotation(movementInput), .18f);
        }
    }

    private void SetEnabled(bool value)
    {
        enabled = value;
        characterController.enabled = value;
        modelTrans.gameObject.SetActive(value);
    }

    public void Die()
    {
        if (dead || isInvincible) return;

        dead = true;
        Invoke(nameof(Respawn), respawnWaitTime);

        movementVelocity = Vector3.zero;
        SetEnabled(false);
        SoundManager.Instance.PlayDeathSound();
    }

    public void Respawn()
    {
        dead = false;
        trans.position = spawnPoint;
        modelTrans.rotation = spawnRotation;

        SetEnabled(true);
        SoundManager.Instance.PlayRespawnSound();
    }

    public void ToggleInvincible()
    {
        isInvincible = !isInvincible;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
        spawnRotation = modelTrans.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Die();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleInvincible();
                Debug.Log($"Player is invisible: {isInvincible}");
            }
        }
    }
}
