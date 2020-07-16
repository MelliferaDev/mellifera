using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspBehavior : MonoBehaviour
{
    /*
     * When the player gets within minDistance of the wasp, the wasp will attack the player.
     * The wasp will hover up and down when it's not attacking.
     */
    public Transform player;
    public float minDistance = 5f;
    public float recoilDist = 1f;
    public float smoothingVal = 3f;
    public float enemyHealth = 10;
    public float playerAttack = 2;
    public float hoverDist = 1f;  // Amount to move left and right from the start point
    public float hoverSpeed = 1.5f;

    float dist;
    bool attack = true;
    bool moveBack = false;
    Vector3 initPos;


    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        dist = Vector3.Distance(player.position, transform.position);
        initPos = transform.position;
    }

    void Update()
    {
        dist = Vector3.Distance(player.position, transform.position);
        if(moveBack)
        {
            float step = Time.deltaTime * smoothingVal;
            transform.position = Vector3.MoveTowards(transform.position, -transform.position * recoilDist, step);
        } else
        if (dist <= minDistance && attack)
        {
            transform.LookAt(player);

            float step = Time.deltaTime * (smoothingVal - 1);
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        } 
        else
        {
            Vector3 v = initPos;
            v.y += hoverDist * Mathf.Sin(Time.time * hoverSpeed);
            transform.position = v;
        }

        if(enemyHealth <=0)
        {
            Destroy(gameObject);
        }
    }

    public void AttackRecoil(float recoilDamage)
    {
        enemyHealth -= recoilDamage;
        moveBack = true;
        attack = false;
        Invoke("AttackAgain", 1f);
    }

    void AttackAgain()
    {
        attack = true;
        moveBack = false;
    }
}
