using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOverGraphicMovement : MonoBehaviour
{

    public float moveDistance = 10f;
    public float moveSpeed = 5f;
    public float spinSpeed = 30f;

    float maxY;
    float minY;
    bool up;

    // Start is called before the first frame update
    void Start()
    {
        maxY = transform.position.y + moveDistance;
        minY = transform.position.y - moveDistance;
        up = true;
    }

    // Update is called once per frame
    void Update()
    {
        float nextY;
        if (up)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime, transform.position.z);
        }
        if (transform.position.y < minY || transform.position.y > maxY) 
        {
            up = !up;
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);
        }
        transform.Rotate(Vector3.up * Time.deltaTime * spinSpeed);
    }
}
