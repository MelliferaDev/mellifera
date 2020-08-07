using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDRBehavior : MonoBehaviour
{

    public AudioClip aClip;
    public AudioClip sClip;
    public AudioClip dClip;
    public AudioClip fClip;

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
            if (fClip != null)
            {
                AudioSource.PlayClipAtPoint(fClip, Camera.main.transform.position, 0.5f);
            }
            ddrManager.HandleKeyPress("F");
        } 
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (dClip != null)
            {
                AudioSource.PlayClipAtPoint(dClip, Camera.main.transform.position, 0.5f);
            }
            ddrManager.HandleKeyPress("D");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (sClip != null)
            {
                AudioSource.PlayClipAtPoint(sClip, Camera.main.transform.position, 0.5f);
            }
            ddrManager.HandleKeyPress("S");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (aClip != null)
            {
                AudioSource.PlayClipAtPoint(aClip, Camera.main.transform.position, 0.5f);
            }
            ddrManager.HandleKeyPress("A");
        }
    }
}
