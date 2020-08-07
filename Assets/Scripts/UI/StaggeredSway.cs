using System;
using UnityEngine;

public class StaggeredSway : MonoBehaviour
{
    [SerializeField] private float angle1;
    [SerializeField] private float angle2;
    [SerializeField] private float delay;

    private int currAngle;
    private float timer;

    private void OnEnable()
    {
        Start();
    }

    private void Start()
    {
        Vector3 leuler = transform.localEulerAngles;
        leuler.z = angle1;
        transform.localEulerAngles = leuler;

        currAngle = 1;
        timer = Time.time;
    }

    void Update()
    {
        if (Time.time - timer >= delay)
        {
            Vector3 leuler = transform.localEulerAngles;
            leuler.z = (currAngle == 1) ? angle2 : angle1;
            transform.localEulerAngles = leuler;

            currAngle = (currAngle == 1) ? 2 : 1;
            timer = Time.time;
        }
    }
}
