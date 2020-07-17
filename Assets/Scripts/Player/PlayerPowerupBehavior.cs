using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerupBehavior : MonoBehaviour
{
    // Subject to change for future implementation
    // Duration should be set on a per-powerup basis, and it means different things for different powerups:
    // Vortex: Duration of the speed boost
    // Free sting: time you have to hit the target before you lose it
    // Waggle dance: Not sure, may not be applicable
    [SerializeField] private int powerupDuration = 3; // seconds
    // This will go in the Vortex specific class once it exists.
    public static int speedBoost = 100;

    private float powerupTimeout;
    private PlayerPowerup curPowerup;
    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        curPowerup = PlayerPowerup.None;
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            powerupTimeout -= Time.deltaTime;
            
            if (powerupTimeout < 0)
            {
                curPowerup = PlayerPowerup.None;
                isActive = false;
            }
        }
    }

    public bool CanPickUp()
    {
        return curPowerup == PlayerPowerup.None;
    }

    public void GivePowerup(PlayerPowerup powerupType)
    {
        curPowerup = powerupType;
    }

    public void Activate()
    {
        isActive = true;
        // TODO This is temp code for how the powerup is be fired. In the future we'd probably want some sort of modular or polymorphic approach.
        // Also since for the prototype, we currently only support the "Vortex"
        if (curPowerup == PlayerPowerup.Vortex)
        {
            powerupTimeout = powerupDuration;
        }
    }

    public PlayerPowerup GetActiveCurrentPowerup()
    {
        return isActive ? curPowerup : PlayerPowerup.None;
    }
}

public enum PlayerPowerup
{
    None = 0, Vortex = 1, FreeSting = 2, WaggleDance = 3
}
