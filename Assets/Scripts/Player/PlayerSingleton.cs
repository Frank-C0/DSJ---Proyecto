using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance { get; private set; }

    [Header("Player Data")]
    public GameObject player;
    public PlayerInventory playerInventory;
    public ObjectThrowing playerObjectThrowing;

    void Awake()
    {
        // Implementación del Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene el GameManager entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
