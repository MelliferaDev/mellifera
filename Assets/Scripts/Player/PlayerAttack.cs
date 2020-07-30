using UnityEngine;

namespace Player
{
    // TODO: Implement
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float damage;


        public float GetAttackDamage()
        {
            return damage * PlayerUpgrades.attackMult;
        }
    }
}