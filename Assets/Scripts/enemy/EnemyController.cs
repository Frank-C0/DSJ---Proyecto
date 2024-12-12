using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float WalkSpeed = 2f;
    public float RunSpeed = 5f;
    public Transform[] Waypoints;
    public float StopDistance = 0.5f;

    [Header("Detection Settings")]
    public float LoseSightTime = 10f;

    [Header("References")]
    public Transform Target;
    public NavMeshAgent Agent;
    public Animator Animator;
    public EnemyVision Vision;

    private IEnemyState currentState;
    private PatrolState patrolState;
    private ChaseState chaseState;
    private AttackState attackState;

    private float timeSinceLastSeen;

    void Start()
    {
        if (Agent == null) Agent = GetComponent<NavMeshAgent>();
        if (Animator == null) Animator = GetComponent<Animator>();
        if (Vision == null) Vision = GetComponent<EnemyVision>();

        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);


        SwitchState(patrolState);
    }

    void Update()
    {
        currentState.UpdateState();

        if (Vision.IsPlayerInSight(Target))
        {
            timeSinceLastSeen = 0f;
            SwitchState(chaseState);
        }
        else if (Vector3.Distance(Target.position, transform.position) < 1.5f)
        {
            SwitchState(attackState);
        }
        else if (currentState == chaseState)
        {
            timeSinceLastSeen += Time.deltaTime;

            if (timeSinceLastSeen >= LoseSightTime)
            {
                SwitchState(patrolState);
            }
        }

    }

    public void SwitchState(IEnemyState newState)
    {
        currentState = newState;
        currentState.EnterState();
    }



}
