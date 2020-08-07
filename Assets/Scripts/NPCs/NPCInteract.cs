using System;
using Player;
using TMPro;
using UI;
using UnityEngine;
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
        private void Start()
        {
            fsm = GetComponent<NPCBehaviour>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            input = FindObjectOfType<InputManager>();

            if (guiSpeak == null)
            {
                guiSpeak = GameObject.FindGameObjectWithTag("Interact Speech");
            }

            requested = false;
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
            if (dist <= interactDist)
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
                if (requested)
                {
                    NPCIndicatorBehaviour.RequestNPCIndicateOff();
                    requested = false;
                }
            }
        }

        private void ActivateTalk()
        {
            fsm.currState = NPCState.Interacting;
            player.transform.LookAt(transform);
            transform.position = (player.position 
                                  + (player.forward * interactDist)
                                  + (player.up * interactYOffset)) ; 
            transform.LookAt(player.transform);
            
            LevelManager.gamePaused = true;
            interacting = true;
            
            guiSpeak.SetActive(true);
            TMP_Text text = guiSpeak.GetComponentInChildren<TMP_Text>();
            
            int rIdx = Random.Range(0, NPCLines.NPC_LINES.Count - 1);
            text.text = NPCLines.NPC_LINES[rIdx];
        }

        private void DeactivateTalk()
        {
            fsm.currState = NPCState.Flying;
            
            LevelManager.gamePaused = false;
            interacting = false;

            guiSpeak.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, interactDist);
        }
    }
}