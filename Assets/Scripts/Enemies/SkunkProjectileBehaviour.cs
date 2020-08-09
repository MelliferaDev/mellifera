using System;
using UI;
using UnityEngine;

namespace Enemies
{
    public class SkunkProjectileBehaviour : MonoBehaviour
    {
        [SerializeField] private float turnSpeed = 30f;
        
        private Transform player;
        private EnemyAttack damage;
        
        private void Start()
        {
            //player = GameObject.FindGameObjectWithTag("Player").transform;
            damage = GetComponent<EnemyAttack>();
            RearviewCameraBehaviour.RequestRearviewOn();
            Debug.Log(player);
        }

        private void Update()
        {
            Vector3 target = player.position;
            Vector3 dirTarget = (target - transform.position).normalized;
            
            Quaternion lookRotation = Quaternion.LookRotation(dirTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        }

        private void OnDestroy()
        {
            RearviewCameraBehaviour.RequestRearviewOff();
        }

        public void SetTarget(Transform target)
        {
            Debug.Log(target);
            player = target;
            Debug.Log("Target aquired");
            RearviewCameraBehaviour.RequestRearviewOff();
        }
    }
}