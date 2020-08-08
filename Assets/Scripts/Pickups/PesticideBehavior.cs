using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class PesticideBehavior : MonoBehaviour
{
    public GameObject pesticideHit;
    public int pesticideAmount = -5;

    private LevelManager lm;
    [SerializeField] private AudioClip collectSfx;

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
    public void ControllerCollisionListener(object[] args)
    {
        PlayerControl player = args[0] as PlayerControl;
        
        if (pesticideAmount < 0)
        {
            lm.IncrementHealth(pesticideAmount);
            Instantiate(pesticideHit, transform.position, transform.rotation);
            // don't continuously apply pesticide gamage
            pesticideAmount = 0;
            if (collectSfx != null) AudioSource.PlayClipAtPoint(collectSfx, player.transform.position);

            for (int i = 0; i < gameObject.GetComponent<Renderer>().materials.Length; i++)
            {
                gameObject.GetComponent<Renderer>().materials[i].DisableKeyword("_EMISSION");
            }
        }
    }
    
    
}
