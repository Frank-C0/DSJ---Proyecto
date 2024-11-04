using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float walkSpeed = 2f;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public float runSpeed = 5f;
    public float detectionRange = 60f; // Distancia a la que el rayo detectará al jugador
    public float stopDistance = 0.5f;
    public Transform target;
    private Animator animator;
    public TMPro.TextMeshProUGUI gameOverText;
    public UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        // Asigna el Animator y Rigidbody del objeto
        animator = GetComponent<Animator>();

        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 position = waypoints[i].position;
            position.y = transform.position.y;    // Asigna el valor de 'y' fijo
            waypoints[i].position = position; // Actualiza la posición del waypoint
        }
    }

    void Update()
    {
        // Usamos un Raycast para detectar al jugador en frente del objeto
        DetectPlayerWithRaycast();
    }

    // Detectar al jugador mediante Raycast
    void DetectPlayerWithRaycast()
    {
        RaycastHit hit;
        Vector3 rayDirection =  (target.position - transform.position).normalized;

        
        // Lanzar el raycast desde la posición del objeto hacia adelante
        if (Physics.Raycast(transform.position, rayDirection, out hit, detectionRange))
        {
            // Si el objeto detectado es el jugador
            if (hit.collider.CompareTag("Player")){
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);           
                agent.speed= runSpeed;
                agent.SetDestination(target.position);
                this.transform.LookAt(target.position);
            }
            else
            {
                agent.speed= walkSpeed;
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                MoveTowardsWaypoint();
            }
        }
    }  

    void MoveTowardsWaypoint()
    {
        if (waypoints.Length == 0) return; // Si no hay waypoints, salir

        agent.SetDestination(waypoints[currentWaypointIndex].position);
        this.transform.LookAt(waypoints[currentWaypointIndex].position);
        // Comprobar si ha llegado al waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.8f)
        {
            float randomValue = Random.value; // Genera un valor aleatorio entre 0 y 1

            if (randomValue < 0.7f) // 70% de probabilidad de continuar
            {
                // Mover al siguiente waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Cíclico
            }
            else // 30% de probabilidad de volver al anterior
            {
                // Mover al waypoint anterior
                currentWaypointIndex = (currentWaypointIndex - 1 + waypoints.Length) % waypoints.Length; // Cíclico
            }
            Debug.Log(currentWaypointIndex);
        }
    }

    // Cambiar de dirección si colisiona con una pared u obstáculo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameOverText.gameObject.SetActive(true);
            animator.SetBool("isAtacking", false);
            animator.SetBool("isWalking", true);
            Time.timeScale = 0; 
            Debug.Log("Juego Terminado");

        }
    }

    void LateUpdate()
    {
        // Obtén la rotación actual del objeto
        Vector3 rotation = transform.rotation.eulerAngles;

        // Solo permite la rotación en Y
        rotation.x = 0f; // Bloquear rotación en X
        rotation.z = 0f; // Bloquear rotación en Z

        // Asigna la rotación solo en Y
        transform.rotation = Quaternion.Euler(rotation);
    }
}