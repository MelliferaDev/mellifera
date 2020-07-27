using UnityEngine;

namespace Wasps
{
    public class WaspAttack : MonoBehaviour
    {
        public int playerDamage = -5;
        public float recoilDamage = 0;
        // Start is called before the first frame update
    
        private LevelManager lm;

        void Start()
        {
            lm = FindObjectOfType<LevelManager>();

        }

        private void OnTriggerEnter(Collider hit)
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                GetComponent<WaspBehavior>().ApplyAttackRecoil(recoilDamage);
                lm.IncrementHealth(playerDamage);
            }
        }
    }
}
