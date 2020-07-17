using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingPlayer : MonoBehaviour
{
    /*
     * This file is more of an idea. If the stinger specifically hits the player, then the player will take more damage.
     */
    public Transform player;
    Transform waspBody;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        waspBody = transform.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Stung!");
            // call to lower player health

        }
    }
}
