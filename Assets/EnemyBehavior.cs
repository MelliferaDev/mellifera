using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    /*
     * When the player gets within minDistance of the wasp, the wasp will attack the player.
     */
    public Transform player;
    public float minDistance = 5f;
    public float smoothingVal = 3f;
    //move these to LevelManager
    public float enemyHealth = 10;
    public float playerAttack = 2;

    float dist;
    bool attack = true;
    bool moveBack = false;


    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        dist = Vector3.Distance(player.position, transform.position);
    }

    void Update()
    {
        dist = Vector3.Distance(player.position, transform.position);
        if(moveBack)
        {
            float step = Time.deltaTime * smoothingVal;
            transform.position = Vector3.MoveTowards(transform.position, -transform.position, step);
        } else
        if (dist <= minDistance && attack)
        {
            transform.LookAt(player);

            float step = Time.deltaTime * (smoothingVal - 1);
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        }

        if(enemyHealth <=0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            moveBack = true;
            attack = false;
            Invoke("AttackAgain", 1f);
            // call to decrease player health

            //if the enemy also takes damage/ we switch this to being if the wasp is hit by the bee, then uncomment this line
            //enemyHealth -= playerAttack;
            
        }
    }

    void AttackAgain()
    {
        attack = true;
        moveBack = false;
    }
}
