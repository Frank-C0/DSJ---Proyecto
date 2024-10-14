using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float detectionRange = 60f; // Distancia a la que el rayo detectará al jugador
    public LayerMask detectionLayer;    // Capa para raycast, asignar al jugador y evitar obstáculos
    public float stopDistance = 0.5f;
    public Transform target;
    public NavigationEnemy navigationEnemy;
    
    public Transform player;
    private Animator animator;
    private Rigidbody rb;
    private bool isFollowing = false;
    private Vector3 wanderingDirection;

    void Start()
    {
        // Asigna el Animator y Rigidbody del objeto
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Generar una dirección inicial para deambular
        wanderingDirection = Random.insideUnitSphere;
        wanderingDirection.y = 0; // Para que solo deambule en el plano horizontal
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
            Wandering();
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
                Debug.Log("Objeto detectado: " + hit.collider.name);
                isFollowing = true;
                navigationEnemy.isActive= false;
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            // Si no detecta al jugador, dejar de seguirlo
            isFollowing = false;
            navigationEnemy.isActive= true;
        }
    }

    // Método para seguir al jugador
    void FollowTarget()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            MoveTowards(directionToPlayer, 2*runSpeed);
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

    // Método para mover el objeto en una dirección con cierta velocidad
    void MoveTowards(Vector3 direction, float speed)
    {
        Vector3 newPosition = rb.position + direction * speed * Time.deltaTime;
        rb.MovePosition(newPosition);
        this.transform.LookAt(target.position);

        // Rotar para mirar en la dirección de movimiento
        //Quaternion lookRotation = Quaternion.LookRotation(direction);
        //rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f));
    }

    // Cambiar de dirección si colisiona con una pared u obstáculo
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            // Generar una nueva dirección aleatoria al chocar con algo
            wanderingDirection = Random.insideUnitSphere;
            wanderingDirection.y = 0;
        }
        else
        {
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