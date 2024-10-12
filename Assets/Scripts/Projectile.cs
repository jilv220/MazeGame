using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("References")]
    public Transform trans;

    [Header("Stats")]
    public float speed = 34;
    public float range = 70;

    private Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = trans.position;
    }

    // Update is called once per frame
    void Update()
    {
        trans.Translate(0, 0, speed * Time.deltaTime, Space.Self);
        if (Vector3.Distance(trans.position, spawnPoint) >= range)
            Destroy(gameObject);
    }
}
