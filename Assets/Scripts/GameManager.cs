using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject PanelPauseDebug;

    public bool isGamePaused;

	private void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGameDebug();
        }
    }

    public void PauseGameDebug()
    {
        isGamePaused = !isGamePaused;
        PanelPauseDebug.SetActive((isGamePaused) ? true : false);
        Time.timeScale = (isGamePaused) ? 0f : 1f;
    }
}
