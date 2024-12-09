using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform playerCamera; // C�mara del jugador.
    [SerializeField] private Transform handPosition; // Posici�n para sostener el objeto.
    [SerializeField] private LineRenderer lineRenderer; // L�nea para dibujar la trayectoria.
    [SerializeField] private int trajectoryPoints = 30; // Cantidad de puntos en la trayectoria.
    [SerializeField] private float maxThrowForce = 20f; // Fuerza m�xima de lanzamiento.
    [SerializeField] private float forceIncrement = 10f; // Incremento de fuerza por segundo.
    [SerializeField] private float timeBetweenPoints = 0.1f; // Tiempo entre puntos de la trayectoria.

    private GameObject heldObject = null; // Objeto actualmente sostenido.
    private bool hasKey = false; // Indica si el jugador tiene una llave.
    private float currentThrowForce = 0f; // Fuerza actual cargada.
    private bool isCharging = false; // Si se est� cargando la fuerza.
    private List<Vector3> trajectoryPositions = new List<Vector3>(); // Puntos de la trayectoria.

    void Update()
    {
        HandleObjectInteraction();
    }

    private void HandleObjectInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Recoger objeto con 'E'.
        {
            if (heldObject == null)
            {
                TryPickUpObject();
            }
        }

        if (heldObject != null)
        {
            HandleThrowing();
        }
    }

    private void TryPickUpObject()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            // Si es un objeto lanzable.
            if (hit.collider.CompareTag("Throwable"))
            {
                PickUpObject(hit.collider.gameObject);
            }
            // Si es una llave.
            else if (hit.collider.CompareTag("Key"))
            {
                CollectKey(hit.collider.gameObject);
            }
        }
    }

    private void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true; // Desactivar f�sica.
        }

        obj.transform.SetParent(handPosition); // Vincular a la mano.
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    private void CollectKey(GameObject keyObject)
    {
        hasKey = true; // Actualizar el estado del jugador.
        Destroy(keyObject); // Eliminar la llave del escenario.
        Debug.Log("�Llave recogida! Ahora puedes usarla.");
    }

    private void HandleThrowing()
    {
        // Iniciar carga de fuerza.
        if (Input.GetMouseButtonDown(1)) // Bot�n derecho.
        {
            isCharging = true;
            currentThrowForce = 0f; // Reiniciar la fuerza.
            lineRenderer.enabled = true; // Mostrar la l�nea de trayectoria.
        }

        // Cargar fuerza y dibujar la trayectoria.
        if (Input.GetMouseButton(1) && isCharging)
        {
            currentThrowForce += forceIncrement * Time.deltaTime;
            currentThrowForce = Mathf.Clamp(currentThrowForce, 0, maxThrowForce);
            ShowTrajectory();
        }

        // Lanzar al soltar el bot�n derecho.
        if (Input.GetMouseButtonUp(1) && isCharging)
        {
            isCharging = false;
            LaunchHeldObject();
            lineRenderer.positionCount = 0; // Limpiar la l�nea.
            lineRenderer.enabled = false; // Ocultar la l�nea.
        }
    }

    private void ShowTrajectory()
    {
        trajectoryPositions.Clear();
        Vector3 startPoint = handPosition.position; // Posici�n inicial del lanzamiento.
        Vector3 startVelocity = playerCamera.forward * currentThrowForce; // Direcci�n y fuerza inicial.

        // Calcular los puntos de la trayectoria.
        for (int i = 0; i < trajectoryPoints; i++)
        {
            float time = i * timeBetweenPoints;
            Vector3 point = CalculatePositionAtTime(startPoint, startVelocity, time);
            trajectoryPositions.Add(point);

            // Detener el c�lculo si se predice una colisi�n con el suelo.
            if (Physics.Raycast(point, Vector3.down, out RaycastHit hit, 0.1f))
            {
                break;
            }
        }

        // Actualizar el LineRenderer.
        lineRenderer.positionCount = trajectoryPositions.Count;
        lineRenderer.SetPositions(trajectoryPositions.ToArray());
    }

    private Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 initialVelocity, float time)
    {
        // F�rmula del movimiento parab�lico.
        Vector3 gravity = Physics.gravity;
        return startPosition + initialVelocity * time + 0.5f * gravity * time * time;
    }

    private void LaunchHeldObject()
    {
        if (heldObject != null)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false; // Activar f�sica.
                rb.velocity = playerCamera.forward * currentThrowForce; // Aplicar la fuerza.
            }

            heldObject.transform.SetParent(null); // Soltar el objeto.
            heldObject = null;
            currentThrowForce = 0f; // Reiniciar la fuerza.
        }
    }

    public bool HasKey()
    {
        return hasKey; // Permite consultar si el jugador tiene una llave.
    }
}
