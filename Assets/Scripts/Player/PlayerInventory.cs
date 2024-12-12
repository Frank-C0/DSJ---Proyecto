using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasKey = false;

    public void CollectKey()
    {
        hasKey = true;
        Debug.Log("Key collected! You can now open specific doors.");
    }

    public bool HasKey()
    {
        return hasKey;
    }
}
