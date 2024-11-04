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
    public NavigationEnemy navigationEnemy;
    
    public Transform player;
    private Animator animator;
    private Rigidbody rb;
    private bool isFollowing = false;
    private float factor = 1.0f;

    private float minDistanceToTarget = 2.5f;
    private Vector3 wanderingDirection;

    // text mesh pro for game over
    public TMPro.TextMeshProUGUI gameOverText;
    public UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        // Asigna el Animator y Rigidbody del objeto
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Generar una dirección inicial para deambular
        wanderingDirection = Random.insideUnitSphere;
        wanderingDirection.y = 0; // Para que solo deambule en el plano horizontal
        navigationEnemy.SetTarget(player);

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

        if (isFollowing)
        {
            FollowTarget();
        }
        else
        {
            MoveTowardsWaypoint();
        }
    }

    // Detectar al jugador mediante Raycast
    void DetectPlayerWithRaycast()
    {
        RaycastHit hit;
        //Vector3 rayDirection = transform.forward; // Rayo hacia adelante desde el objeto
        Vector3 rayDirection =  (player.position - transform.position).normalized;

        
        // Lanzar el raycast desde la posición del objeto hacia adelante
        if (Physics.Raycast(transform.position, rayDirection, out hit, detectionRange))
        {
            // Si el objeto detectado es el jugador
            if (hit.collider.CompareTag("Player"))
            {
                //Debug.Log("Objeto detectado: " + hit.collider.name);
                isFollowing = true;
                //navigationEnemy.isActive= false;
                navigationEnemy.SetBoolIsActive(true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            // Si no detecta al jugador, dejar de seguirlo
            // Activar la animación de caminar mientras deambula
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            navigationEnemy.SetBoolIsActive(false);
            float distanceToPath = Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position);
            if (distanceToPath > 0.1f){
                agent.enabled= true;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            } else{
                agent.enabled= false;
                isFollowing = false;
            }
        }
    }

    // Método para seguir al jugador
    void FollowTarget()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if ((distanceToTarget < minDistanceToTarget)){
                factor= 0.8f;
            }else{
                factor= 1.0f;
            }
            
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            MoveTowards(directionToPlayer, 2*runSpeed*factor);
        }
        else
        {
            // Detener el seguimiento si está demasiado cerca del jugador
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAtacking", true);
        }
    }

    // Movimiento de deambulación
    void Wandering()
    {
        // Activar la animación de caminar mientras deambula
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);

        // Moverse en la dirección actual
        MoveTowards(wanderingDirection, walkSpeed);
    }

    void MoveTowardsWaypoint()
    {
        if (waypoints.Length == 0) return; // Si no hay waypoints, salir

        // Moverse hacia el waypoint actual
        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        MoveTowards(direction, walkSpeed);
        /*Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * walkSpeed * Time.deltaTime;*/

        // Comprobar si ha llegado al waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.8f)
        {
            // Decidir si continuar al siguiente waypoint o volver al anterior
            DecideNextWaypoint();
        }
    }

    void DecideNextWaypoint()
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

    // Método para mover el objeto en una dirección con cierta velocidad
    void MoveTowards(Vector3 direction, float speed)
    {
        Vector3 newPosition = rb.position + direction * speed * Time.deltaTime;
        rb.MovePosition(newPosition);
        this.transform.LookAt(transform.position + direction);
    }

    // Cambiar de dirección si colisiona con una pared u obstáculo
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            // Generar una nueva dirección aleatoria al chocar con algo
            //wanderingDirection = Random.insideUnitSphere;
            //wanderingDirection.y = 0;
            // Obtener la normal de la colisión (dirección perpendicular a la pared)
            Vector3 normalColision = collision.contacts[0].normal;

            // Generar una dirección opuesta a la pared usando la normal
            wanderingDirection = Vector3.Reflect(transform.forward, normalColision) + Random.insideUnitSphere;
            
            // Asegurarse de que la dirección esté en el plano horizontal
            wanderingDirection.y = 0;

        }
        else
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