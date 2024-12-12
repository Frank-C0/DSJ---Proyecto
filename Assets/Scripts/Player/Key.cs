using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public string GetInteractionMessage()
    {
        return "Press E to collect the key";
    }

    public void Interact()
    {
        PlayerSingleton.Instance.playerInventory.CollectKey();
        Destroy(gameObject);
    }
}
