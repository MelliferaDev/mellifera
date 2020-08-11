using System.Dynamic;
using Pickups;
using UnityEngine;
using UnityEngine.UI;
using Enemies;

namespace Player
{
    
    // Subject to change for future implementation
    // "powerupDuration" should be set on a per-powerup basis, and it means different things for different powerups:
    // Vortex: Duration of the speed boost
    // Free sting: time you have to hit the target before you lose it
    // Waggle dance: Not sure, may not be applicable
    public class PlayerPowerupBehavior : MonoBehaviour
    {
        [SerializeField] private int vortexDuration = 3; // seconds
        [SerializeField] private int waggleDanceDuration = 3; // seconds
        [SerializeField] private PowerupGUI gui;
        [SerializeField] private GameObject swarmVfx;

        [Header("Waggle Target Reticle Settings")] 
        [SerializeField] private Transform reticleEyes;
        [SerializeField] Image reticleImage;
        [SerializeField] float reticleChangeSpeed = 3;
        [SerializeField] Color reticleEnemyColor;
        [SerializeField] Color reticleCollectibleColor;
        [SerializeField] Color reticleHiveColor;
        [SerializeField] bool canWagglePollen = true;
        [SerializeField] bool canWaggleEnemies = true;
        [SerializeField] bool canWaggleHive = true;
        Color originalReticleColor;
        GameObject curTarget;

        public static int vortexSpeedBoost = 100; // This will go in the Vortex specific class once it exists.

        public int vortexCollected;
        public int stingCollected;
        public int waggleCollected;

        private float powerupTimeout;
        private static PlayerPowerup curPowerup;

        // Start is called before the first frame update
        void Start()
        {
            curPowerup = PlayerPowerup.None;
            originalReticleColor = reticleImage.color;
            reticleImage.gameObject.SetActive(waggleCollected > 0);
            ReticleEffect();
        }

        // Update is called once per frame
        void Update()
        {
            if (curPowerup != PlayerPowerup.None)
            {
                powerupTimeout -= Time.deltaTime;
            
                if (powerupTimeout < 0)
                {
                    curPowerup = PlayerPowerup.None;
                }
            }
        }

        private void FixedUpdate()
        {
            ReticleEffect();
        }

        public bool CanPickUp()
        {
            // This used to check if you've already picked up a powerup. 
            // Since you can now have multiple of each powerup, the restriction is gone. 
            // However, we may want to introduce a future mechanic where you can only pick up X number of powerups at once, or something.
            return true;
        }

        public void GivePowerup(PlayerPowerup powerupType)
        {
            switch (powerupType)
            {
                case PlayerPowerup.Vortex: 
                    vortexCollected++;
                    gui.UpdateGUI(powerupType, vortexCollected);
                    break;
                case PlayerPowerup.FreeSting: 
                    stingCollected++;
                    gui.UpdateGUI(powerupType, stingCollected);
                    break;
                case PlayerPowerup.WaggleDance: 
                    waggleCollected++;
                    reticleImage.gameObject.SetActive(true);
                    gui.UpdateGUI(powerupType, waggleCollected);
                    break;
            }
        }

        public void Activate(PlayerPowerup powerupType)
        {
            switch (powerupType)
            {
                case PlayerPowerup.Vortex:
                    ActivateVortex();
                    break;
                case PlayerPowerup.FreeSting:
                    ActivateSting();
                    break;
                case PlayerPowerup.WaggleDance:
                    ActivateWaggleDance();
                    break;
            }
        }

        private void ActivateVortex()
        {
            if (vortexCollected <= 0) return;
            
            curPowerup = PlayerPowerup.Vortex;
            powerupTimeout = vortexDuration;
            vortexCollected--;
            gui.UpdateGUI(PlayerPowerup.Vortex, vortexCollected);

        }
        
        private void ActivateSting()
        {
            if (stingCollected <= 0) return;
            
            curPowerup = PlayerPowerup.FreeSting;
            powerupTimeout = 0; // Sting doesn't have an active duration, it's one-and-done.
            stingCollected--;
            gui.UpdateGUI(PlayerPowerup.FreeSting, stingCollected);

        }
        
        private void ActivateWaggleDance()
        {
            if (waggleCollected <= 0) return;

            bool didDance = false;
            
            if (curTarget != null)
            {
                if (curTarget.CompareTag("Enemy"))
                {
                    // This currently just feels like a different version of the free sting...
                    Debug.Log("here: " + curTarget.name);
                    FindObjectOfType<StingBehavior>().FinishSting(1, 1, curTarget);
                    didDance = true;
                }
                else if (curTarget.CompareTag("Collectible"))
                {
                    CollectibleBehavior cb = curTarget.GetComponent<CollectibleBehavior>();
                    if (cb == null) return;

                    if (cb.collectibleType == CollectibleType.Pollen)
                    {
                        cb.SendMessage("ControllerCollisionListener", new object[] { GetComponent<PlayerControl>(), 2 });
                        didDance = true;
                    }
                }
                else if (curTarget.CompareTag("Hive"))
                {
                    HiveManager hm = FindObjectOfType<HiveManager>();
                    hm.ActivateHiveDefence();
                    didDance = true;
                }
            }
            else
            {

                // Our third dance, Shock, isn't needed till level 3. I'm thinking this is some sort of AoE attack?
                // However, I don't know what the best way to trigger it would be, and if this `else` case is enough.

                // temp
                didDance = false;
            }

            if (didDance)
            {
                Instantiate(swarmVfx, curTarget.transform.position, Quaternion.identity);

                curPowerup = PlayerPowerup.WaggleDance;
                powerupTimeout = waggleDanceDuration;
                waggleCollected--;
                gui.UpdateGUI(PlayerPowerup.WaggleDance, waggleCollected);
            }
        }

        public static PlayerPowerup GetActiveCurrentPowerup()
        {
            return curPowerup;
        }

        private void ReticleEffect()
        {
            if (waggleCollected > 0)
            {
                RaycastHit hit;

                if (Physics.Raycast(reticleEyes.position, reticleEyes.forward, out hit))
                {
                    
                    if (hit.collider.CompareTag("Enemy") && canWaggleEnemies)
                    {
                        reticleImage.color = Color.Lerp(reticleImage.color, reticleEnemyColor, Time.deltaTime * reticleChangeSpeed);

                        Quaternion targetRotation = Quaternion.AngleAxis(0f, Vector3.forward);
                        reticleImage.transform.rotation = Quaternion.Lerp(reticleImage.transform.rotation, targetRotation, Time.deltaTime * reticleChangeSpeed);

                        reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, new Vector3(.7f, .7f, 1), Time.deltaTime * reticleChangeSpeed);

                        curTarget = hit.collider.gameObject;
                        return;
                    }
                    else if (hit.collider.CompareTag("Collectible") && canWagglePollen)
                    {
                        if(GetCBFromCollider(hit.collider, out CollectibleBehavior cb))
                        {
                            if (cb.collectibleType == CollectibleType.Health) return;
                        }
                        
                        reticleImage.color = Color.Lerp(reticleImage.color, reticleCollectibleColor, Time.deltaTime * reticleChangeSpeed);

                        Quaternion targetRotation = Quaternion.AngleAxis(45f, Vector3.forward);
                        reticleImage.transform.rotation = Quaternion.Lerp(reticleImage.transform.rotation, targetRotation, Time.deltaTime * reticleChangeSpeed);

                        reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, Vector3.one, Time.deltaTime * reticleChangeSpeed);

                        curTarget = hit.collider.gameObject;
                        return;
                    }
                    else if (hit.collider.CompareTag("Hive") && canWaggleHive)
                    {
                        reticleImage.color = Color.Lerp(reticleImage.color, reticleHiveColor, Time.deltaTime * reticleChangeSpeed);

                        Quaternion targetRotation = Quaternion.AngleAxis(45f, Vector3.forward);
                        reticleImage.transform.rotation = Quaternion.Lerp(reticleImage.transform.rotation, targetRotation, Time.deltaTime * reticleChangeSpeed);

                        reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, Vector3.one, Time.deltaTime * reticleChangeSpeed);

                        curTarget = hit.collider.gameObject;
                        return;
                    }
                }
                ReturnToOriginalReticle();
            } 
            else
            {
                reticleImage.gameObject.SetActive(false);
            }
        }

        private void ReturnToOriginalReticle()
        {
            reticleImage.color = Color.Lerp(reticleImage.color, originalReticleColor, Time.deltaTime * reticleChangeSpeed);
            reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, Vector3.one, Time.deltaTime * reticleChangeSpeed);
            reticleImage.transform.rotation = Quaternion.Lerp(reticleImage.transform.rotation, Quaternion.AngleAxis(0f, Vector3.forward), Time.deltaTime * reticleChangeSpeed);
            curTarget = null;
        }

        private bool GetCBFromCollider(Collider col, out CollectibleBehavior cb)
        {
            cb = col.GetComponent<CollectibleBehavior>();
            if (cb == null)
                cb = col.GetComponentInChildren<CollectibleBehavior>();
            else 
                return true;

            if (cb == null)
                cb = col.GetComponentInParent<CollectibleBehavior>();
            else 
                return true;
            
            if (cb != null) 
                return true;
            
            Debug.LogWarning("Objected tagged as Collectible w/o CollectibleBehaviour attached");
            return false;

        }
    }

    public enum PlayerPowerup
    {
        None, Vortex, FreeSting, WaggleDance
    }
}