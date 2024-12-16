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
        // Destroy(gameObject);

        // set payer as parent and position on 0,0,0 local
        transform.SetParent(PlayerSingleton.Instance.player.transform);
        transform.localPosition = Vector3.zero;

        // disable the rigidbody, gravity and collider
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Collider>().enabled = false;
        
    }
}
