using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderRegion : MonoBehaviour
{
    public Vector3 size;
    public Vector3 GetRandomPointWithin()
    {
        float x = transform.position.x + Random.Range(size.x * -.5f, size.x * .5f);
        float z = transform.position.z + Random.Range(size.z * -.5f, size.z * .5f);
        return new Vector3(x, transform.position.y, z);
    }

    void Awake()
    {
        var wanderers = gameObject.GetComponentsInChildren<Wanderer>();
        foreach (var wanderer in wanderers)
        {
            wanderer.region = this;
        }
    }
}
