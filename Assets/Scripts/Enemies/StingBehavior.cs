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

        // Start is called before the first frame update
        void Start()
        {
            wasps = GameObject.FindGameObjectsWithTag("Wasp");
        }

        // Update is called once per frame
        void Update()
        {
            if (WaspInRange() && Input.GetKeyDown(KeyCode.Q) && !LevelManager.gamePaused)
            {
                StingEnemy();
            }
        }

        bool WaspInRange()
        {
            foreach (GameObject wasp in wasps)
            {
                if (wasp != null && Vector3.Distance(transform.position, wasp.transform.position) < stingDistance)
                {
                    Debug.Log("Target in my sights");
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
            FindObjectOfType<LevelManager>().StartDDR(targetWasp);
        }

        // This can be more fleshed out in future iterations of our game
        public void FinishSting(int score, int maxScore, GameObject target)
        {
            LevelManager lm = FindObjectOfType<LevelManager>();
            if (score > maxScore * .9)
            {
                Debug.Log("Option1");
                lm.IncrementHealth(-minDamage);
                Destroy(target);
            }
            else if (score > maxScore * .6)
            {
                Debug.Log("Option2");
                lm.IncrementHealth(-averageDamage);
                Destroy(target);
            }
            else if (score > maxScore * .4)
            {
                Debug.Log("Option3");
                lm.IncrementHealth(-maxDamage);
                Destroy(target);
            }
            else
            {
                Debug.Log("Option4");
                lm.IncrementHealth(-maxDamage);
            }
            stingIndicator.SetActive(false);
            RearviewCameraBehaviour.RequestRearviewOff();
        }
    }
}
