using UnityEngine;
using Refactoring;

public class GameManager : MonoBehaviour
{

    #region References

    [SerializeField] UIManager uiManager;
    [SerializeField] PlayerEntity player;

    #endregion

    bool isPaused = false;

    #region MonoBehaviour Methods

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    #endregion

    #region API

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

    #endregion
}