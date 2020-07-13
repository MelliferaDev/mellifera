using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform lookAt;
    public float offset = 1.5f;
    public float distance = 3.5f;
    
    private Vector3 desiredPos;

    private void Start()
    {
        //Follow();
    }

    void LateUpdate()
    {
        Follow();
    }

    void Follow()
    {
        desiredPos = lookAt.position + (-transform.forward * distance) + (transform.up * offset); 
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime);
        transform.LookAt(lookAt.position + Vector3.up * offset);
    }
}
