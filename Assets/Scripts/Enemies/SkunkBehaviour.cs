using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    /// <summary>
    /// Skunks patrol just like wasps, but they do not follow the player, instead they will shoot projectiles at them.
    /// Because they are very big, they won't shoot if the player is really close to them.
    /// (maybe touching the skunk does damage to the wasp as well?)
    /// </summary>
    public class SkunkBehaviour : EnemyBehaviour
    {
        public SkunkState currState;
        [Header("Settings")]
        [SerializeField] private float enemySpeed = 1.0f;
        [SerializeField] private float maxDistToAttack = 10f;
        [SerializeField] private float minDistToAttack = 5f;
        [SerializeField] private Transform[] patrolPoints;
        [Header("Projectile")]
        [SerializeField] private float shootRate = 2; // shoot every x seconds
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private GameObject projectilesParent;
        [SerializeField] private GameObject projectileSpawn;
        [Space(20)] 
        [SerializeField] private GameObject guiObject;
    
        private Animator anim;
        private NavMeshAgent agent;
        
        private float lastTimeShot;
        private float disengageTimer;
        private static readonly int SkunkMovement = Animator.StringToHash("skunkMovement");

        private EnemySight sight;

        protected override void Start()
        {
            base.Start();
            Initalize();

            if (patrolPoints != null && patrolPoints.Length >= 2)
            {
                pointA = patrolPoints[0].position;
                pointB = patrolPoints[1].position;
                nextPoint = pointA;
                agent.SetDestination(nextPoint);

            }
            
            
            guiObject.SetActive(false);


            lastTimeShot = Time.time;
            disengageTimer = Time.time;
        }

        void Initalize()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            sight = GetComponent<EnemySight>();
            
            agent.speed = enemySpeed;
            agent.angularSpeed = rotationSpeed;
            
            if (projectilesParent == null)
            {
                projectilesParent = GameObject.FindGameObjectWithTag("ProjectilesParent");
            }
        }


        protected override void Update()
        {
            base.Update();

            if (LevelManager.gamePaused)
            {
                agent.speed = 0;
                FaceTarget(player.transform.position);
            }
            else
            {
                switch(currState) {
                    case SkunkState.Patrolling: UpdatePatrolState(); break;
                    case SkunkState.Attacking: UpdateAttackState(); break;
                    case SkunkState.Disengaging: UpdateDisengageState(); break;
                }
            }
        }
        
        public override void ApplySelfDamage(float damage) {return;}

        //Defeated
        public override void EnemyDefeated()
        {
            //will probably call update disengage here
        }

        ///////////////////////////////////////////////
        //// Basic Movement States ////////////////////

        public override void UpdatePatrolState()
        {
            agent.speed = enemySpeed;
            agent.stoppingDistance = minDistToAttack + 5;
            bool x = agent.isStopped;
            if (Vector3.Distance(transform.position, nextPoint) <= minDistToAttack - Mathf.Epsilon)
            {
                FindNextPoint();
                agent.SetDestination(nextPoint);
            }

            if (distToPlayer >= minDistToAttack && distToPlayer <= maxDistToAttack && sight.InFOV(player.transform))
            {
                lastTimeShot = Time.time - shootRate - 0.1f;
                currState = SkunkState.Attacking;
            }
            
            guiObject.SetActive(false);
            
            FaceTarget(nextPoint);
            anim.SetInteger(SkunkMovement, 4); // walking
        }



        ///////////////////////////////////////////////
        //// Attacking State //////////////////////////

        private void UpdateAttackState()
        {
            if (distToPlayer < minDistToAttack || distToPlayer > maxDistToAttack)
            {
                currState = SkunkState.Disengaging;
                disengageTimer = Time.time;
            }

            agent.speed = 0;

            guiObject.SetActive(true);
    
            FaceTargetReverse(player.transform.position);
            anim.SetInteger(SkunkMovement, 2); // attack
            EnemyAttack();
        }

        private void UpdateDisengageState()
        {

            anim.SetInteger(SkunkMovement, 3);
            float animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
            
            if (Time.time - disengageTimer >= animDuration)
            {
                currState = SkunkState.Patrolling;
            } 
        
            if (distToPlayer >= minDistToAttack && distToPlayer <= maxDistToAttack)
            {
                currState = SkunkState.Attacking;
            }
            
            guiObject.SetActive(false);
        }
    
        
        private void EnemyAttack()
        {
            if (Time.time - lastTimeShot > shootRate)
            {
                GameObject proj = Instantiate(projectilePrefab,
                    projectileSpawn.transform.position, projectileSpawn.transform.rotation);
                proj.transform.parent = projectilesParent.transform;
                
                lastTimeShot = Time.time;
            }
        }
    
        void ProjectileInstantiate()
        {
            GameObject proj = Instantiate(projectilePrefab,
                projectileSpawn.transform.position, projectileSpawn.transform.rotation);
            proj.transform.parent = projectilesParent.transform;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 position = transform.position;
            Gizmos.DrawWireSphere(position, minDistToAttack);
        
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, maxDistToAttack);
            
            if (patrolPoints.Length >= 2)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawSphere(patrolPoints[0].position, 2.5f);
                Gizmos.DrawSphere(patrolPoints[1].position, 2.5f);
            }

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(pointA, 3f);
            Gizmos.DrawSphere(pointB, 3f);

            

        }
    

    }

    public enum SkunkState
    {
        Patrolling, Attacking, Disengaging
    }
}