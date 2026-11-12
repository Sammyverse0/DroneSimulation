using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float waitAtWaypoint = 1.5f;
    [SerializeField] Animator animator;

    NavMeshAgent agent;
    int waypointIndex;
    float waitTimer;

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
        Patrol();

        if (animator != null)
            animator.SetFloat("Speed", agent.velocity.magnitude);
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
}