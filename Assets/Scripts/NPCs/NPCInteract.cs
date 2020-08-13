using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace NPCs
{
    [RequireComponent(typeof(NPCBehaviour))]
    public class NPCInteract : MonoBehaviour
    {
        public static bool interacting = false;
        
        [SerializeField] private float interactDist;
        [SerializeField] private float interactYOffset;
        [SerializeField] private GameObject guiSpeak;
        
        private Transform player;
        private NPCBehaviour fsm;
        private InputManager input;

        private bool requested;

        private float currInteractDist;

        private static int linesRead;
        private List<string> levelTips;
        
        private void Start()
        {
            fsm = GetComponent<NPCBehaviour>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            input = FindObjectOfType<InputManager>();

            if (guiSpeak == null)
            {
                guiSpeak = GameObject.FindGameObjectWithTag("Interact Speech");
            }

            currInteractDist = interactDist;
            requested = false;

            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName.EndsWith("1"))
            {
                levelTips = NPCLines.INTRO_LINES_LVL1;
            } else if (sceneName.EndsWith("2"))
            {
                levelTips = NPCLines.INTRO_LINES_LVL2;
            } else if (sceneName.EndsWith("3"))
            {
                levelTips = NPCLines.INTRO_LINES_LVL3;
            }

            linesRead = 0;
        }

        private void Update()
        {
            if (LevelManager.gamePaused)
            {
                // probably some redundancy here but ¯\_(ツ)_/¯
                if (interacting && requested && input.GetNPCTalkBtnClicked())
                { 
                    DeactivateTalk();
                }

                return;
            }

            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist <= currInteractDist)
            {
                if (!requested)
                {
                    NPCIndicatorBehaviour.RequestNPCIndicateOn();
                    requested = true;
                }
                if (input.GetNPCTalkBtnClicked())
                {
                    ActivateTalk();
                }
            }
            else
            {
                if (requested && !interacting)
                {
                    NPCIndicatorBehaviour.RequestNPCIndicateOff();
                    requested = false;
                }
            }
        }

        private void ActivateTalk()
        {
            fsm.currState = NPCState.Interacting;
            // player.transform.LookAt(transform);
            transform.position = (player.position 
                                  + player.forward * (2 * interactDist)
                                  + player.up * (2 * interactYOffset));
            currInteractDist = 2 * interactDist;
            transform.LookAt(player.transform);
            
            guiSpeak.SetActive(true);
            TMP_Text text = guiSpeak.GetComponentInChildren<TMP_Text>();

            text.text = ChooseLine();
            
            interacting = true;

            Invoke(nameof(Pause), 0.5f);
        }

        private string ChooseLine()
        {
            if (linesRead < levelTips.Count)
            {
                string tip = levelTips[linesRead];
                linesRead++;
                return tip;
            }
            
            int rIdx = Random.Range(0, NPCLines.NPC_LINES.Count - 1);
            return NPCLines.NPC_LINES[rIdx];
        }

        private void Pause()
        {
            LevelManager.gamePaused = true;
        }

        private void DeactivateTalk()
        {
            currInteractDist = interactDist;
            fsm.currState = NPCState.Flying;
            
            LevelManager.gamePaused = false;
            interacting = false;

            guiSpeak.SetActive(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactDist);
        }
    }
}