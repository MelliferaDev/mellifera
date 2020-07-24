using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDRBehavior : MonoBehaviour
{

    DDRManager ddrManager;
    // Start is called before the first frame update
    void Start()
    {
        ddrManager = FindObjectOfType<DDRManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F");
            ddrManager.HandleKeyPress("F");
        } 
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D");
            ddrManager.HandleKeyPress("D");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S");
            ddrManager.HandleKeyPress("S");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A");
            ddrManager.HandleKeyPress("A");
        }
    }
}
