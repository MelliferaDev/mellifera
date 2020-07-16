using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspAttack : MonoBehaviour
{
    public float recoilDamage = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.parent.GetComponent<WaspBehavior>().AttackRecoil(recoilDamage);

            // call to decrease player health          

        }
    }
}
