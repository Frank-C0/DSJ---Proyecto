using UnityEngine;

public class ChaseState : IEnemyState
{
    private EnemyController controller;

    public ChaseState(EnemyController controller)
    {
        this.controller = controller;
    }

    public void EnterState()
    {
        controller.Agent.speed = controller.RunSpeed;
        controller.Animator.SetBool("isWalking", false);
        controller.Animator.SetBool("isRunning", true);
    }

    public void UpdateState()
    {
        if (controller.Target != null)
        {
            controller.Agent.SetDestination(controller.Target.position);
        }
    }
}
