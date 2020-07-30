using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Pickups
{
    public class PowerupGUI : MonoBehaviour
    {
        [SerializeField] private Image vortexImg;
        [SerializeField] private Text vortexText;
        [SerializeField] private Image freeStingImg;
        [SerializeField] private Text freeStingText;
        [SerializeField] private Image waggleDanceImg;
        [SerializeField] private Text waggleDanceText;


        public void UpdateGUI(PlayerPowerup type, int val)
        {
            Text text;
            Image img;
            switch (type)
            {
                case PlayerPowerup.Vortex:
                    text = vortexText;
                    img = vortexImg;
                    break;
                case PlayerPowerup.FreeSting:
                    text = freeStingText;
                    img = freeStingImg;
                    break;
                case PlayerPowerup.WaggleDance:
                    text = waggleDanceText;
                    img = waggleDanceImg;
                    break;
                default: return;
            }
            
            text.text = val.ToString("00");
            img.gameObject.SetActive(val > 0);
        }
    }
}