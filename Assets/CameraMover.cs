using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private bool runLateral;
    [SerializeField] private Vector3 lateralSpeed;
    [SerializeField] private Transform center;
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (runLateral)
        {
            UpdateLateral();
        }
    }

    void UpdateLateral()
    {
        transform.RotateAround(center.position, Vector3.up, lateralSpeed.y * Time.deltaTime);
    }
}
