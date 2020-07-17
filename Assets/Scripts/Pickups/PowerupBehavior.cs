using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBehavior : MonoBehaviour
{
    public PlayerPowerup powerupType;

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
        if (other.CompareTag("Player"))
        {
            PlayerPowerupBehavior powerupReceiver = other.GetComponent<PlayerPowerupBehavior>();

            if (powerupReceiver.CanPickUp())
            {
                powerupReceiver.GivePowerup(powerupType);
                Destroy(gameObject);
            }
        }
    }
}
