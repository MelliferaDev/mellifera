using System;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    [RequireComponent(typeof(UpgradeMenu))]
    public class UpgradeMenuUI : MonoBehaviour
    {
        [Space(20)] 
        [SerializeField] private Text totalPointText;
        [Space(10)]
        [SerializeField] private Text speedAmtText;
        [SerializeField] private Text speedPtsText;
        [Space(10)]
        [SerializeField] private Text healthAmtText;
        [SerializeField] private Text healthPtsText;
        [Space(10)]
        [SerializeField] private Text attackAmtText;
        [SerializeField] private Text attackPtsText;
        
        private UpgradeMenu menuCtrl;
        private PlayerControl playerCtrl;
        private LevelManager lm;

        private void Start()
        {
            menuCtrl = GetComponent<UpgradeMenu>();
            playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
            lm = FindObjectOfType<LevelManager>();
        }

        private void Update()
        {
            UpdatePointsText();
            UpdateAmountsText();
        }

        void UpdatePointsText()
        {
            totalPointText.text = UpgradeMenu.totalPoints.ToString("00");
            speedPtsText.text = menuCtrl.pointsToSpeed.ToString("00");
            healthPtsText.text = menuCtrl.pointsToHealth.ToString("00");
            attackPtsText.text = menuCtrl.pointsToAttack.ToString("00");
        }

        void UpdateAmountsText()
        {
            float maxSpeedCalc = playerCtrl.maxSpeed + PlayerUpgrades.maxSpeedAdd;
            speedAmtText.text = maxSpeedCalc.ToString("000");
            int maxHealthCalc = lm.startingHealth + PlayerUpgrades.maxHealthAdd;
            healthAmtText.text = maxHealthCalc.ToString("000");
            attackAmtText.text = PlayerUpgrades.attackMult.ToString("0.0x");
        }
    }
}