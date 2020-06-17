using System.Collections.Generic;
using UnityEngine;

public class FabrikIK : MonoBehaviour
{
    public bool applyIK;

    public Transform ikPos;

    public Transform[] controlPoints;

    private List<float> _dists;

    private int _lastPointIndex;

    private Vector3 _rootPos;

    private void Start()
    {
        _dists = new List<float>();

        _rootPos = controlPoints[0].position;

        _lastPointIndex = controlPoints.Length - 1;

        for (int i = 1; i < controlPoints.Length; i++)
        {
            float dist = Vector3.Distance(controlPoints[i].position, controlPoints[i - 1].position);
            _dists.Add(dist);
        }
    }

    private void Update()
    {
        if (applyIK)
        {
            if (Vector3.Distance(controlPoints[_lastPointIndex].position, ikPos.position) > 0.1f)
            {
                ForwadIteration();
                BackwardIteration();
            }
        }
    }

    private void ForwadIteration()
    {
        controlPoints[_lastPointIndex].position = ikPos.position;

        for (int i = _lastPointIndex - 1; i >= 0; i--)
        {
            Vector3 dir = (controlPoints[i].position - controlPoints[i + 1].position).normalized;
            controlPoints[i].position = controlPoints[i + 1].position + dir * _dists[i];
        }
    }

    private void BackwardIteration()
    {
        controlPoints[0].position = _rootPos;

        for (int i = 1; i < controlPoints.Length; i++)
        {
            Vector3 dir = (controlPoints[i].position - controlPoints[i - 1].position).normalized;
            controlPoints[i].position = controlPoints[i - 1].position + dir * _dists[i - 1];
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (controlPoints != null && controlPoints.Length > 0)
        {
            Gizmos.color = Color.blue;

            for (int i = 0; i < controlPoints.Length; i++)
            {
                if (controlPoints[i] != null)
                {
                    Gizmos.DrawSphere(controlPoints[i].position, 0.2f);
                }
                
            }
        }
    }
#endif
}
