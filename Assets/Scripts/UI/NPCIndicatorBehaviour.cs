using System;
using UnityEngine;

namespace UI
{
    public class NPCIndicatorBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject uiIndicator;
        private static int npcRequests;
        
        void Start()
        {
            npcRequests = 0;
        }

        void Update()
        {
            uiIndicator.SetActive(npcRequests > 0);
        }

        public static void RequestNPCIndicateOn()
        {
            npcRequests++;
            npcRequests = Math.Max(npcRequests, 1);
        }

        public static void RequestNPCIndicateOff()
        {
            npcRequests--;
        }

    }
}