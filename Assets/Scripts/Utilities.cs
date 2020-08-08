using System;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// General Utils class
/// </summary>
public static class Utils
{
        public static float Distance2D(Vector3 v1, Vector3 v2)
        {
                Vector2 v1XZ = new Vector2(v1.x, v1.z);
                Vector2 v2XZ = new Vector2(v2.x, v2.z);
                return Vector2.Distance(v1XZ, v2XZ);
        }

        public static bool NavAgentReachedDest(NavMeshAgent agent, float tolerance, float stoppingSpeed = 5f)
        {
                float dist = agent.remainingDistance;
                float speed = Mathf.Abs(agent.velocity.magnitude);

                Debug.Log($"d: {dist}, s: {speed}");
                
                return !float.IsPositiveInfinity(dist)
                       //&& speed <= stoppingSpeed
                       && agent.pathStatus == NavMeshPathStatus.PathComplete
                       && Math.Abs(dist) < tolerance;
        }
}