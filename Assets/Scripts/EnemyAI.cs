using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    enum State { Patrol, Attack }

    [SerializeField] Transform[] waypoints;
    [SerializeField] float waitAtWaypoint = 1.5f;
    [SerializeField] Animator animator;

    [SerializeField] float detectionRange = 20f;
    [SerializeField] float loseRange = 26f;
    [SerializeField] float eyeHeight = 1.6f;

    [SerializeField] Transform muzzle;
    [SerializeField] Projectile bulletPrefab;
    [SerializeField] float fireInterval = 1f;
    [SerializeField] float aimSpread = 1.5f;
    [SerializeField] float turnSpeed = 6f;
    [SerializeField] AudioSource shootAudio;

    NavMeshAgent agent;
    Transform player;
    State state = State.Patrol;
    int waypointIndex;
    float waitTimer;
    float nextFireTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (waypoints.Length > 0)
            agent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        if (player == null) FindPlayer();

        bool canSee = CanSeePlayer();

        if (state == State.Patrol)
        {
            Patrol();
            if (canSee) EnterAttack();
        }
        else
        {
            Attack(canSee);
        }

        if (animator != null)
            animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void FindPlayer()
    {
        GameObject found = GameObject.FindGameObjectWithTag("Player");
        if (found != null) player = found.transform;
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 origin = transform.position + Vector3.up * eyeHeight;
        Vector3 direction = player.position - origin;
        float distance = direction.magnitude;
        float limit = state == State.Attack ? loseRange : detectionRange;

        if (distance > limit) return false;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, ~0, QueryTriggerInteraction.Ignore))
            return hit.collider.transform.root == player;

        return false;
    }

    void Patrol()
    {
        if (waypoints.Length == 0 || agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitAtWaypoint)
            {
                waitTimer = 0f;
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[waypointIndex].position);
            }
        }
    }

    void EnterAttack()
    {
        state = State.Attack;
        agent.isStopped = true;
    }

    void ExitAttack()
    {
        state = State.Patrol;
        waitTimer = 0f;
        agent.isStopped = false;

        if (waypoints.Length > 0)
            agent.SetDestination(waypoints[waypointIndex].position);
    }

    void Attack(bool canSee)
    {
        if (!canSee)
        {
            ExitAttack();
            return;
        }

        Vector3 flat = player.position - transform.position;
        flat.y = 0f;

        if (flat.sqrMagnitude > 0.01f)
        {
            Quaternion look = Quaternion.LookRotation(flat);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * Time.deltaTime);
        }

        if (Time.time >= nextFireTime)
            Shoot();
    }

    void Shoot()
    {
        nextFireTime = Time.time + fireInterval;

        Vector3 aimPoint = player.position + Random.insideUnitSphere * aimSpread;
        Quaternion rotation = Quaternion.LookRotation(aimPoint - muzzle.position);
        Instantiate(bulletPrefab, muzzle.position, rotation);

        if (shootAudio != null) shootAudio.Play();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}