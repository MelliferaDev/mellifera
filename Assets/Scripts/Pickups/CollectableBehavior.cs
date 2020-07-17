using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehavior : MonoBehaviour
{
    [SerializeField] private int pollenAmount = 25;

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
        Debug.Log("z");

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("a");
            if (collision.gameObject.GetComponent<PlayerControl>().currState == PlayerFlightState.Landed)
            {
                Debug.Log("b");
                // TODO right now we just give all the pollen as soon as they land on it, but we might want to give it over a short amount of time (2-3 seconds?)
                FindObjectOfType<LevelManager>().CollectPollen(pollenAmount);
                // No more pollen left on this flower
                pollenAmount = 0;
            }
        }
    }
}
