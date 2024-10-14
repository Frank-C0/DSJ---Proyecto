using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationEnemy : MonoBehaviour
{
    public Transform target;
    public Transform main;
    public NavMeshAgent agent;
    public bool isActive= true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive){
            agent.SetDestination(target.position);
        } else{
            agent.SetDestination(main.position);
        }
    }
}
