using System;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class UpgradeMenu : MonoBehaviour
    {
        public static int totalPoints; // the amount of points available to spend
        [SerializeField] private int startingPoints;
        [Space(10)] 
        [SerializeField] private int speedCost = 1; // num pts per increment
        [SerializeField] private int speedIncr = 5; // amount of speed increase per point
        [SerializeField] private Button upSpeed;
        [SerializeField] private Button downSpeed;
        [Space(10)] 
        [SerializeField] private int healthCost = 1;
        [SerializeField] private int healthIncr = 5; // per point
        [SerializeField] private Button upHealth;
        [SerializeField] private Button downHealth;
        [Space(10)] 
        [SerializeField] private int attackCost = 1;
        [SerializeField] private float attackIncr = 0.25f; // per point
        [SerializeField] private Button upAttack;
        [SerializeField] private Button downAttack;
        [Space(10)] 
        [SerializeField] private Button doneBtn;
        [SerializeField] private Color doneColor1;
        [SerializeField] private Color doneColor2;

        internal int pointsToSpeed;
        internal int pointsToHealth;
        internal int pointsToAttack;

        private void Start()
        {
            pointsToSpeed = 0;
            pointsToAttack = 0;
            pointsToHealth = 0;
            totalPoints += startingPoints;
            
            upSpeed.onClick.AddListener(IncrSpeed);
            downSpeed.onClick.AddListener(DecrSpeed);
            
            upHealth.onClick.AddListener(IncrHealth);
            downHealth.onClick.AddListener(DecrHealth);
            
            upAttack.onClick.AddListener(IncrAttack);
            downAttack.onClick.AddListener(DecrAttack);

            doneBtn.image.color = doneColor1;
        }

        private void Update()
        {
            if (totalPoints < Math.Min(Math.Min(speedCost, attackCost), healthCost))
            {
                float step = (Mathf.Sin(Time.time) + 1f) / 1f;
                doneBtn.image.color = Color.Lerp(doneColor1, doneColor2, step);
            }
            else
            {
                doneBtn.image.color = doneColor1;
            }

            upSpeed.interactable = (totalPoints >= speedCost);
            upHealth.interactable = (totalPoints >= healthCost);
            upAttack.interactable = (totalPoints >= attackCost);

            downSpeed.interactable = (pointsToSpeed > 0);
            downHealth.interactable = (pointsToHealth > 0);
            downAttack.interactable = (pointsToAttack > 0);
        }

        /////////////////////////////////////////////////////
        //// Speed ///////////////////////////////////////////

        void IncrSpeed()
        {
            if (totalPoints < speedCost) return;
            PlayerUpgrades.maxSpeedAdd += speedIncr * speedCost;
            totalPoints -= speedCost;
            pointsToSpeed += speedCost;
        }

        void DecrSpeed()
        {
            if (pointsToSpeed < speedCost) return;
            PlayerUpgrades.maxSpeedAdd -= speedIncr * speedCost;
            totalPoints += speedCost;
            pointsToSpeed -= speedCost;
        }
        
        /////////////////////////////////////////////////////
        //// Health /////////////////////////////////////////
        
        void IncrHealth()
        {
            if (totalPoints < healthCost) return;
            PlayerUpgrades.maxHealthAdd += healthIncr * healthCost;
            totalPoints -= healthCost;
            pointsToHealth += healthCost;

        }

        void DecrHealth()
        {
            if (pointsToHealth < healthCost) return;
            PlayerUpgrades.maxHealthAdd -= healthIncr * healthCost;
            totalPoints += healthCost;
            pointsToHealth -= healthCost;
        }
        
        /////////////////////////////////////////////////////
        //// Attack /////////////////////////////////////////
        
        void IncrAttack()
        {
            if (totalPoints < attackCost) return;
            PlayerUpgrades.attackMult += attackIncr * attackCost;
            totalPoints -= attackCost;
            pointsToAttack += attackCost;

        }

        void DecrAttack()
        {
            if (pointsToAttack < attackCost) return;
            PlayerUpgrades.attackMult -= attackIncr * attackCost;
            totalPoints += attackCost;
            pointsToAttack -= attackCost;
        }

    }
}