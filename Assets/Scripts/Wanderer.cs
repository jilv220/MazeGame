using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    private enum State
    {
        Idle,
        Rotating,
        Moving
    }
    private State state = State.Idle;
    [HideInInspector] public WanderRegion region;

    [Header("References")]
    public Transform trans;
    public Transform modelTrans;

    [Header("Stats")]
    public float moveSpeed = 18;
    public float minRetargetInterval = 4.4f;
    public float maxRetargetInterval = 6.2f;
    public float rotationTime = .6f;
    public float postRotationTime = .3f;

    private Vector3 currTarget;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float rotationStartTime;

    void Retarget()
    {
        currTarget = region.GetRandomPointWithin();

        initialRotation = modelTrans.rotation;
        targetRotation = Quaternion.LookRotation((currTarget - trans.position).normalized);
        state = State.Rotating;
        rotationStartTime = Time.time;

        Invoke(nameof(BeginMoving), rotationTime + postRotationTime);
    }

    void BeginMoving()
    {
        modelTrans.rotation = targetRotation;
        state = State.Moving;
    }

    // Start is called before the first frame update
    void Start()
    {
        Retarget();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Moving:
                float delta = moveSpeed * Time.deltaTime;
                trans.position = Vector3.MoveTowards(trans.position, currTarget, delta);
                if (trans.position == currTarget)
                {
                    state = State.Idle;
                    Invoke(nameof(Retarget), Random.Range(minRetargetInterval, maxRetargetInterval));
                }
                break;
            case State.Rotating:
                float timeSpentRotating = Time.time - rotationStartTime;
                modelTrans.rotation = Quaternion.Slerp(initialRotation, targetRotation, timeSpentRotating / rotationTime);
                break;
            default: break;
        }
    }
}
