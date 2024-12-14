
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance { get; private set; }

    [Header("Game Data")]
    [SerializeField]
    public static bool isPaused = false;
    [SerializeField]
    public static bool isGameOver = false;

    [Header("Player Data")]
    public GameObject player;
    public PlayerInventory playerInventory;
    public ObjectThrowing playerObjectThrowing;

    [Header("Enemy Data")]
    public GameObject enemy;
    public EnemyController enemyController;

    [Header("Canvas")]
    public ControladorCanvas controladorCanvas;
    public GameObject pantallaPausa, pantallaPerdiste, pantallaGanaste;
    

    void Awake()
    {
        // Implementacion del Singleton
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

    public void HideAndLockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowAndUnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PauseGame()
    {
        Debug.Log("Game Paused");

        isPaused = true;

        ShowAndUnlockCursor();

        // show pause screen
        pantallaPausa.SetActive(true);

        // stop enemy animations
        enemyController.PauseAnimations();
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");


        isPaused = false;
        // hide and lock cursor
        HideAndLockCursor();

        // hide pause screen
        pantallaPausa.SetActive(false);

        // resume enemy animations
        enemyController.ResumeAnimations();
    }

    public void GameOver()
    {
        Debug.Log("Game Over");

        isGameOver = true;
        isPaused = true;

        ShowAndUnlockCursor();

        pantallaPerdiste.SetActive(true);
    }

    public void WinGame()
    {
        Debug.Log("You win!");


        isGameOver = true;

        ShowAndUnlockCursor();

        pantallaGanaste.SetActive(true);
    }

    public void RestartGame()
    {
        Debug.Log("Game Restarted");

        isGameOver = false;
        isPaused = false;

        ShowAndUnlockCursor();

        pantallaPerdiste.SetActive(false);
        pantallaGanaste.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Debug.Log("Next Level");

        isGameOver = false;
        isPaused = false;

        pantallaGanaste.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        Debug.Log("Main Menu");

        isGameOver = false;
        isPaused = false;

        pantallaPerdiste.SetActive(false);
        pantallaGanaste.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
