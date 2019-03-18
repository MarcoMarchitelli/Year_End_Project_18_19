using UnityEngine;
using Refactoring;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region References

    [SerializeField] UIManager uiManager;
    [SerializeField] PlayerEntity player;

    #endregion

    bool isPaused = false;
    bool isMapOpen = false;
    bool isInInventory = false;

    #region MonoBehaviour Methods

    private void Update()
    {
        if (!isMapOpen && !isInInventory && Input.GetButtonDown("Pause"))
            TogglePause();

        if (!isPaused && !isInInventory && Input.GetButtonDown("Map"))
            ToggleMap();

        if (!isPaused && !isMapOpen && Input.GetButtonDown("Inventory"))
            ToggleInventory();
    }

    #endregion

    #region API

    public void ToggleInventory()
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

    public void ToggleMap()
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

    public void TogglePause()
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

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    #endregion

}