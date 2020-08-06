using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Axis axis;

    void Update()
    {
        float step = speed * Time.deltaTime;
        Vector3 localEulers = transform.localEulerAngles;
        switch (axis)
        {
            case Axis.X: localEulers.x += step;
                break;
            case Axis.Y: localEulers.y += step;
                break;
            case Axis.Z: localEulers.z += step;
                break;
            default: return;
        }

        transform.localEulerAngles = localEulers;
    }

    private enum Axis
    {
        X, Y, Z, None
    }
}
