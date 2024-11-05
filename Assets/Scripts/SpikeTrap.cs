using Unity.VisualScripting;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("References")]
    public Transform spikeHolder;
    public GameObject hitboxGobj;
    public GameObject colliderGobj;

    [Header("Stats")]
    public float interval = 2f;
    public float raiseWaitTime = .3f;
    public float lowerTime = .6f;
    public float raiseTime = 0.08f;
    private float lastSwitchTime = Mathf.NegativeInfinity;

    private enum State
    {
        Lowered,
        Lowering,
        Raising,
        Raised
    }
    private State state = State.Lowered;
    private const float SpikeHeight = 3.6f;
    private const float LoweredSpikeHeight = .08f;

    void StartRaising()
    {
        lastSwitchTime = Time.time;
        state = State.Raising;
        hitboxGobj.SetActive(true);
    }
    void StartLowering()
    {
        lastSwitchTime = Time.time;
        state = State.Lowering;
    }

    Vector3 UpdateSpikeScale(float a, float b, float t)
    {
        Vector3 scale = spikeHolder.localScale;
        scale.y = Mathf.Lerp(a, b, t);
        spikeHolder.localScale = scale;
        return scale;
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(StartRaising), interval);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Lowering:
                {
                    var newScale
                        = UpdateSpikeScale(SpikeHeight, LoweredSpikeHeight, (Time.time - lastSwitchTime) / lowerTime);

                    // Finished lowering
                    if (newScale.y == LoweredSpikeHeight)
                    {
                        Invoke(nameof(StartRaising), interval);
                        state = State.Lowered;
                        colliderGobj.SetActive(false);
                    }
                }
                break;
            case State.Raising:
                {

                    var newScale
                        = UpdateSpikeScale(LoweredSpikeHeight, SpikeHeight, (Time.time - lastSwitchTime) / lowerTime);

                    // Finished raising
                    if (newScale.y == SpikeHeight)
                    {
                        Invoke(nameof(StartLowering), raiseWaitTime);
                        state = State.Raised;
                        colliderGobj.SetActive(true);
                        hitboxGobj.SetActive(false);
                    }
                }
                break;
            default:
                break;
        }
    }
}
