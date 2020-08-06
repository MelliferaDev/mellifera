using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class PesticideBehavior : MonoBehaviour
{
    public GameObject pesticideHit;
    public int pesticideAmount = -5;

    private LevelManager lm;
    int damageApplied;

    private void Start()
    {
        lm = FindObjectOfType<LevelManager>();
    }

   
    void Update()
    {
    }

    /// <summary>
    /// A listener method designed to recieve messages from the PlayerCollection
    /// OnControllerColliderHit
    /// </summary>
    /// <param name="player">The PlayerControl from the OnControllerColliderHit</param>
    public void ControllerCollisionListener(PlayerControl player)
    {
        if (pesticideAmount < 0)
        {
            lm.IncrementHealth(pesticideAmount);
            Instantiate(pesticideHit, transform.position, transform.rotation);
            // don't continuously apply pesticide gamage
            pesticideAmount = 0;
            gameObject.GetComponent<Renderer>().materials[1].DisableKeyword("_EMISSION");
        }
    }
    
    
}
