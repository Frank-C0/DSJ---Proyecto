using UnityEngine;
using System.Collections;

public class DoorInteractive : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    //[SerializeField] private string requiredKeyTag = "Door1";
    [SerializeField] private bool isOpen = false;
    [SerializeField] private float openAngle = -90f; // Ángulo final en el eje Y para abrir la puerta
    [SerializeField] private float openSpeed = 2f;   // Velocidad de apertura

    public string GetInteractionMessage()
    {
        if (isOpen)
        {
            return "";
        }
        if (!isOpen && PlayerSingleton.Instance.playerInventory.HasKey())
        {
            return "Press E to open the door";
        }
        else
        {
            return "You need a key to open this door";
        }
    }

    public void Interact()
    {
        if (!isOpen && PlayerSingleton.Instance.playerInventory.HasKey())
        {
            Debug.Log("Door opened!");
            isOpen = true;
            // Aquí podrías añadir una animación de apertura
            StartCoroutine(OpenDoor());

            // disable the collider
            GetComponent<Collider>().enabled = false;
        }
    }

    private IEnumerator OpenDoor()
    {
        isOpen = true; // Marcar la puerta como abierta
        Transform parent = transform.parent;

        Quaternion startRotation = parent.rotation; // Rotación inicial
        Quaternion targetRotation = Quaternion.Euler(0f, openAngle, 0f); // Rotación final

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            parent.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * openSpeed;
            yield return null;
        }

        parent.rotation = targetRotation; // Asegurar que la rotación final sea precisa
    }
}
