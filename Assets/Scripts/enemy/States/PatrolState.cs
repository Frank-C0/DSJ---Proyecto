using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IEnemyState
{
    private EnemyController controller;
    private int currentWaypointIndex = 0;

    public PatrolState(EnemyController controller)
    {
        this.controller = controller;
    }

    public void EnterState()
    {
        controller.Agent.speed = controller.WalkSpeed;
        controller.Animator.SetBool("isWalking", true);
        controller.Animator.SetBool("isRunning", false);
    }

    public void UpdateState()
    {
        if (controller.Waypoints.Length == 0) return;

        controller.Agent.SetDestination(controller.Waypoints[currentWaypointIndex].position);

        if (Vector3.Distance(controller.transform.position, controller.Waypoints[currentWaypointIndex].position) < controller.StopDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % controller.Waypoints.Length;
        }
    }
}
