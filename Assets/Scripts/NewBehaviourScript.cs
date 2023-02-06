using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public LineRenderer lineRenderer;
    private void Update()
    {
        if (agent.hasPath)
        {
            lineRenderer.positionCount = agent.path.corners.Length;
            lineRenderer.SetPositions(agent.path.corners);
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }    
    }
}
