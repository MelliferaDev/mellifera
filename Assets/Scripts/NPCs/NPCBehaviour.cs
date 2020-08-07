using System;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.ProBuilder;
using Random = UnityEngine.Random;

namespace NPCs
{
    public class NPCBehaviour : MonoBehaviour
    {
        public NPCState currState = NPCState.Flying;
        [SerializeField] private float flySpeed = 10f;
        [SerializeField] private float hoverDist = 3f;
        [SerializeField] private float hoverSpeed = 1f;
        [SerializeField] private float interactHoverDist = 1f;
        [SerializeField] private float pollinateDuration = 10f;

        private GameObject[] flowers;
        private int currFlowerIdx;
        private Vector3 currFlowerPos;
        private Transform player;
        private float currDist;
        
        private float pollinateTimer;

        private float flySpeedSave;

        private void Start()
        {
            flowers = GameObject.FindGameObjectsWithTag("FlowerGroup");
            player = GameObject.FindGameObjectWithTag("Player").transform;
            
            currFlowerIdx = -1;
            ChooseNextFlower();
            currDist = 10f;

            flySpeedSave = flySpeed;
        }

        private void Update()
        {
            if (LevelManager.gamePaused)
            {
                flySpeed = 0;
                return;
            }
            
            flySpeed = flySpeedSave;

            Vector2 posXZ = new Vector2(transform.position.x, transform.position.z);
            Vector2 flowerXZ = new Vector2(currFlowerPos.x, currFlowerPos.z);

            currDist = Vector2.Distance(posXZ, flowerXZ);

            switch (currState)
            {
                case NPCState.Flying: UpdateFlying();
                    break;
                case NPCState.Pollinating: UpdatePollinating();
                    break;
                case NPCState.Interacting : UpdateInteracting();
                    break;
            }
            
        }

        private void UpdateFlying()
        {
            if (currDist < 5f)
            {
                currState = NPCState.Pollinating;
                pollinateTimer = Time.time;
            }
            
            // fly to target
            Vector3 target =  Vector3.MoveTowards(transform.position, currFlowerPos, Time.deltaTime * flySpeed);
            FaceTarget(target);
            target.y = transform.position.y;
            
            // hover as you go
            float hoverOffset = hoverDist * Mathf.Sin(hoverSpeed * Time.time);
            target.y += hoverOffset;

            transform.position = target;
        }

        private void UpdatePollinating()
        {
            if (Time.time - pollinateTimer > pollinateDuration)
            {
                ChooseNextFlower();
                currState = NPCState.Flying;
            }
            
            // hover above the flower
            Vector3 target = transform.position;
            target.y += hoverDist * Mathf.Sin(hoverSpeed * Time.time);
            transform.position =  Vector3.MoveTowards(transform.position, target, Time.deltaTime * flySpeed);
        }
        
        private void UpdateInteracting()
        {
            transform.LookAt(player.transform.position);
        }
        
        
        private void FaceTarget(Vector3 target)
        {
            Vector3 dirTarget = (target - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dirTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
        }

        private void ChooseNextFlower()
        {

            int newFlower =  Random.Range(0, flowers.Length - 1);
            if (newFlower == currFlowerIdx)
            {
                newFlower = (newFlower + 1) % flowers.Length;
            }

            currFlowerPos = flowers[newFlower].transform.position;
        }


    }

    public enum NPCState
    {
        Flying, Pollinating, Interacting
    }
}