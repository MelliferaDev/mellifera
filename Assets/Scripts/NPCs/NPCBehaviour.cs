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
        [SerializeField] private float pollinateDuration = 10f;
        
        private GameObject[] flowers;
        private int currFlowerIdx;
        private Vector3 currFlowerPos;
        private Transform player;
        private float currDist;
        
        private float pollinateTimer;
        private float lastPatrolTimer;
        
        private float flySpeedSave;

        private CharacterController ctrl;
        private void Start()
        {
            flowers = GameObject.FindGameObjectsWithTag("FlowerGroup");
            player = GameObject.FindGameObjectWithTag("Player").transform;

            ctrl = GetComponent<CharacterController>();
            
            currFlowerIdx = -1;
            ChooseNextFlower();
            currDist = 10f;

            flySpeedSave = flySpeed;
            lastPatrolTimer = Time.time;
        }

        private void Update()
        {
            if (LevelManager.gamePaused)
            {
                ctrl.Move(Vector3.zero);
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
                lastPatrolTimer = Time.time;
            } else if (Time.time - lastPatrolTimer > 30f)
            {
                // taking too long to get to the flower, probably stuck
                ChooseNextFlower();
                lastPatrolTimer = Time.time;
            }
            
            // fly to target
            Vector3 toTarget = (currFlowerPos - transform.position);
            
            Vector3 move = toTarget.normalized * (flySpeed * Time.deltaTime);
            float hoverOffset = hoverDist * Mathf.Sin(hoverSpeed * Time.time);
            move.y = hoverOffset;
            ctrl.Move(move);

            Vector3 ctrlVel = ctrl.velocity;
            ctrlVel.y = transform.forward.y;
            FaceDirection(ctrlVel);
        }

        private void UpdatePollinating()
        {
            if (Time.time - pollinateTimer > pollinateDuration)
            {
                ChooseNextFlower();
                currState = NPCState.Flying;
                lastPatrolTimer = Time.time;
            }
            
            // hover above the flower
            Vector3 move = Vector3.zero;
            move.y = hoverDist * Mathf.Sin(hoverSpeed * Time.time);
            
            ctrl.Move(move);
            FaceTarget(currFlowerPos);
        }
        
        private void UpdateInteracting()
        {
            transform.LookAt(player.transform.position);
        }
        
        
        private void FaceTarget(Vector3 target)
        {
            FaceDirection((target - transform.position).normalized);
        }
        
        private void FaceDirection(Vector3 dirTarget)
        {
            if (Mathf.Abs(dirTarget.magnitude) > Mathf.Epsilon)
            {
                Quaternion lookRotation = Quaternion.LookRotation(dirTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);  
            }
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

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(currFlowerPos, 1f);
            Gizmos.DrawLine(transform.position, currFlowerPos);
        }
    }

    public enum NPCState
    {
        Flying, Pollinating, Interacting
    }
}