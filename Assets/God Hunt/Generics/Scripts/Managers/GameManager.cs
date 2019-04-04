using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region References

    [SerializeField] UIManager uiManager;
    [SerializeField] PlayerEntity player;
    public RoomSystem roomSystem;
    [SerializeField] InputManager inputManager;


    #endregion

    bool isPaused = false;
    bool isMapOpen = false;
    bool isInInventory = false;

    #region MonoBehaviour Methods

    private void Awake()
    {
        Setup();
    }

    #endregion

    #region API

    public void ToggleInventory()
    {
        if (!isPaused && !isMapOpen)
        {
            isInInventory = !isInInventory;

            if (isInInventory)
            {
                Time.timeScale = 0;
                player.Enable(false);
                uiManager.ToggleInventoryPanel(true);
            }
            else
            {
                Time.timeScale = 1;
                player.Enable(true);
                uiManager.ToggleInventoryPanel(false);
            }
        }
    }

    public void ToggleMap()
    {
        if (!isPaused && !isInInventory)
        {
            isMapOpen = !isMapOpen;

            if (isMapOpen)
            {
                Time.timeScale = 0;
                player.Enable(false);
                uiManager.ToggleMapPanel(true);
            }
            else
            {
                Time.timeScale = 1;
                player.Enable(true);
                uiManager.ToggleMapPanel(false);
            }
        }
    }

    public void TogglePause()
    {
        if (!isMapOpen && !isInInventory)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Time.timeScale = 0;
                player.Enable(false);
                uiManager.TogglePausePanel(true);
            }
            else
            {
                Time.timeScale = 1;
                player.Enable(true);
                uiManager.TogglePausePanel(false);
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void LoadScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
        Time.timeScale = 1;
    }

    #endregion

    #region Internals

    void Setup()
    {
        if (!Instance)
            Instance = this;

        if (roomSystem)
            roomSystem.Setup();

        if (inputManager)
            inputManager.Setup();
    }

    #endregion
}