
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField] private Transform eyes;
    [SerializeField] private float fovDist = -1f; // if negative, distance will not be checked
    [SerializeField] private float fovAngle = 65f;
    private void Awake()
    {
        if (fovDist < 0)
        {
            fovDist = Mathf.Infinity;
        }
    }

    public bool InFOV(Transform target)
    {
        Vector3 dirToTarget = target.transform.position - eyes.position;

        if (Vector3.Angle(dirToTarget, eyes.forward) <= fovAngle)
        {
            if (Physics.Raycast(eyes.position, dirToTarget, out RaycastHit hit, fovDist))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
