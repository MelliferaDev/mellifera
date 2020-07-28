using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class BoundaryBehaviour : MonoBehaviour
{

    public static float collisionDisplayTime = 3f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<BoundaryDisplayCountDown>().DisplayFor(collisionDisplayTime);
        }
    }
}
