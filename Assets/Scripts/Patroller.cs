using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    private const float rotationSlerp = .68f;

    [Header("References")]
    public Transform trans;
    public Transform modelTrans;

    [Header("Stats")]
    public float moveSpeed = 30;

    private int currentPointIdx;
    private Transform currentPoint;
    private Transform[] patrolPoints;

    private List<Transform> GetUnsortedPatrolPoints()
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        var points = new List<Transform>();
        for (var i = 0; i < children.Length; i++)
        {
            if (children[i].gameObject.name.StartsWith("Patrol Point ("))
                points.Add(children[i]);
        }

        return points;
    }

    private void SetCurrentPatrolPoint(int index)
    {
        currentPointIdx = index;
        currentPoint = patrolPoints[index];
    }

    // Start is called before the first frame update
    void Start()
    {
        List<Transform> points = GetUnsortedPatrolPoints();
        if (points.Count == 0) return;

        patrolPoints = new Transform[points.Count];
        for (var i = 0; i < points.Count; i++)
        {
            Transform point = points[i];
            int closingParenthesisIndex = point.gameObject.name.IndexOf(')');
            string indexSubstring = point.gameObject.name[14..closingParenthesisIndex];

            var index = Convert.ToInt32(indexSubstring);
            patrolPoints[index] = point;
            // Unparent so that patrol points do not travel with us
            point.SetParent(null);
            point.gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        SetCurrentPatrolPoint(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentPoint) return;

        trans.position = Vector3.MoveTowards(trans.position, currentPoint.position, moveSpeed * Time.deltaTime);

        // If we reached the point
        if (trans.position == currentPoint.position)
        {
            if (currentPointIdx >= patrolPoints.Length - 1)
                SetCurrentPatrolPoint(0);
            else
                SetCurrentPatrolPoint(currentPointIdx + 1);
        }
        else
        {
            Quaternion lookRotation = Quaternion.LookRotation((currentPoint.position - trans.position).normalized);
            modelTrans.rotation = Quaternion.Slerp(modelTrans.rotation, lookRotation, rotationSlerp);
        }
    }
}
