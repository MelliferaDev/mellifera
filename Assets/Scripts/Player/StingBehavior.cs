using Player;
using UI;
using UnityEngine;

namespace Enemies
{
    public class StingBehavior : MonoBehaviour
    {

        public float stingDistance = 10f;

        public int minDamage = 5;
        public int averageDamage = 10;
        public int maxDamage = 20;
        public GameObject stingIndicator;

        GameObject[] wasps;
        GameObject targetWasp;

        private PlayerPowerupBehavior powerup;
        private InputManager input;

        private bool stinging = false;

        // Start is called before the first frame update
        void Start()
        {
            wasps = GameObject.FindGameObjectsWithTag("Enemy");
            input = FindObjectOfType<InputManager>();
            powerup = GetComponent<PlayerPowerupBehavior>();
        }

        // Update is called once per frame
        void Update()
        {
            if (WaspInRange() && input.GetStingKeyClicked() && !LevelManager.gamePaused)
            {
                StingEnemy();
            } 
            else if (stinging)
            {
                gameObject.transform.LookAt(targetWasp.transform);
            }
        }

        bool WaspInRange()
        {
            foreach (GameObject wasp in wasps)
            {
                if (wasp != null && Vector3.Distance(transform.position, wasp.transform.position) < stingDistance)
                {
                    targetWasp = wasp;
                    stingIndicator.SetActive(true);
                    return true;
                }
            }
            stingIndicator.SetActive(false);
            return false;
        }

        void StingEnemy()
        {
            //SceneManager.LoadScene("BrockDDR", LoadSceneMode.Additive);
            FindObjectOfType<PlayerControl>().StopBuzzSFX();
            gameObject.transform.LookAt(targetWasp.transform);
            stinging = true;
            powerup.Activate(PlayerPowerup.FreeSting);
            if (PlayerPowerupBehavior.GetActiveCurrentPowerup() == PlayerPowerup.FreeSting)
            {
                FinishSting(1, 1, targetWasp);
            }
            else
            {
                FindObjectOfType<LevelManager>().StartDDR(targetWasp);
            }
        }

        // This can be more fleshed out in future iterations of our game
        public void FinishSting(int score, int maxScore, GameObject target)
        {
            LevelManager lm = FindObjectOfType<LevelManager>();
            EnemyBehaviour targetEB = target.GetComponent<EnemyBehaviour>();
            if (targetEB == null)
            {
                targetEB = target.GetComponentInParent<EnemyBehaviour>();
            }

            stinging = false;
            // killing the wasps can definitely be improved upon
            if (score > maxScore * .9)
            {
                Debug.Log("Option1");
                lm.IncrementHealth(-minDamage);
                targetEB.EnemyDefeated();
            }
            else if (score > maxScore * .6)
            {
                Debug.Log("Option2");
                lm.IncrementHealth(-averageDamage);
                targetEB.EnemyDefeated();
            }
            else if (score > maxScore * .4)
            {
                Debug.Log("Option3");
                lm.IncrementHealth(-maxDamage);
                targetEB.EnemyDefeated();
            }
            else
            {
                Debug.Log("Option4");
                lm.IncrementHealth(-maxDamage);
            }
            stingIndicator.SetActive(false);
            RearviewCameraBehaviour.RequestRearviewOff();
            FindObjectOfType<PlayerControl>().StartBuzzSFX();
        }
    }
}
