using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardEnemyAI : MonoBehaviour
{
    //Enemy states.
    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Flee,
        Dead
    }

    //Current enemy state.
    public FSMStates currentState;

    //Distance and travel variables.
    public float enemySpeed = 4f;
    public float attackDistance = 3;
    public float chaseDistance = 10;
    float distanceToPlayer;

    //Player and attack information.
    public GameObject player;
    public GameObject AttackHitBox;
    float elapsedTime = 0;
    public float attackDelay = 4.0f;
    private bool shouldBattleIdle;

    //Death visual effects.
    //public GameObject deadVFX;

    //Patrol points of enemy.
    GameObject[] wanderPoints;
    Vector3 nextDestination;

    //Animator controller of enemy.
    Animator m_Animator;

    //Enemy health and death items.
    EnemyHealth enemyHealth;
    int health;
    int currentDestinationIndex = 0;
    Transform deadTransform;
    bool isDead;
    public GameObject[] lootPrefabs;

    //int randInt = 101;

    void Start()
    {
        //Finds possibly null objects.
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (AttackHitBox == null)
        {
            AttackHitBox = GameObject.FindGameObjectWithTag("CatfishAttackHitBox");
        }

        //Finds patrol points.
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");

        //Gets animator component.
        m_Animator = GetComponent<Animator>();

        //Deactivates attack information.
        AttackHitBox.SetActive(false);
        shouldBattleIdle = true;

        //Sets enemy health.
        enemyHealth = GetComponent<EnemyHealth>();
        health = enemyHealth.currentHealth;

        //Confirms enemy isn't dead.
        isDead = false;

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        enemyHealth = GetComponent<EnemyHealth>();
        health = enemyHealth.currentHealth;

        switch (currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
            case FSMStates.Dead:
                UpdateDeadState();
                break;
        }

        elapsedTime += Time.deltaTime;

        if (health <= 0)
        {
            currentState = FSMStates.Dead;
        }
    }

    void Initialize()
    {
        currentState = FSMStates.Patrol;
        FindNextPoint();
    }

    void UpdatePatrolState()
    {
        //print("Patrolling!");

        m_Animator.SetInteger("AnimationInt", 1);

        if (Vector3.Distance(transform.position, nextDestination) < 1)
        {
            FindNextPoint();
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);
    }

    void UpdateChaseState()
    {
        m_Animator.SetInteger("AnimationInt", 2);

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        transform.position = Vector3.MoveTowards(transform.position, nextDestination, (enemySpeed + 2) * Time.deltaTime);
    }

    void UpdateAttackState()
    {
        //print("Attack!");

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        if (shouldBattleIdle)
        {
            m_Animator.SetInteger("AnimationInt", 3);
        }

        ClawAttack();
    }

    void UpdateDeadState()
    {
        if (!isDead)
        {
            LevelManager.enemyKillCount += 1;
            m_Animator.SetInteger("AnimationInt", 6);
            isDead = true;
            deadTransform = gameObject.transform;

            if (lootPrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, lootPrefabs.Length);
                Instantiate(lootPrefabs[randomIndex], transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }

            //Instantiate(deadVFX, deadTransform.position, deadTransform.rotation);
            Destroy(gameObject, 3);
        }
    }

    //
    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;
        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void ClawAttack()
    {
        if (!isDead)
        {
            if (elapsedTime >= attackDelay)
            {
                shouldBattleIdle = false;
                m_Animator.SetInteger("AnimationInt", 4);

                //Activates the hitbox 3/4 into the animation.
                var animDuration = m_Animator.GetCurrentAnimatorStateInfo(0).length;
                //Invoke("ActivateHitBox", animDuration * (3/4));
                ActivateHitBox();

                //Resets attack information.
                Invoke("ResetAttackInfo", animDuration);
                elapsedTime = 0.0f;
            }
        }
    }

    void ActivateHitBox()
    {
        AttackHitBox.SetActive(true);
    }

    void ResetAttackInfo()
    {
        elapsedTime = 0.0f;
        shouldBattleIdle = true;
        AttackHitBox.SetActive(false);
    }

    /*
    //Draws spheres depicting distances for enemy interactions.
    private void OnDrawGizmos()
    {
        //attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        //chase
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
    */
}