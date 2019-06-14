﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using GodHunt.Inputs;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool initPlayer;
    [SerializeField] bool initUIManager;
    [SerializeField] bool initRoomSystem;
    [SerializeField] bool initCameraManager;
    [SerializeField] bool initInputManager;

    private UIManager uiManager;
    [HideInInspector] public PlayerEntity player;
    [HideInInspector] public RoomSystem roomSystem;
    private InputManager inputManager;

    bool isPaused = false;
    bool isMapOpen = false;
    bool isInInventory = false;
    bool isInCollectablesScreen = false;

    #region API

    public void ToggleInventory()
    {
        if (!isPaused && !isMapOpen && !isInCollectablesScreen)
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
        if (!isPaused && !isInInventory && !isInCollectablesScreen)
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
        if (!isMapOpen && !isInInventory && !isInCollectablesScreen)
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

    public void ToggleCollectablesScreen()
    {
        if (!isMapOpen && !isInInventory && !isPaused)
        {
            isInCollectablesScreen = !isInCollectablesScreen;

            if (isInCollectablesScreen)
            {
                Time.timeScale = 0;
                player.Enable(false);
                uiManager.ToggleCollectablesScreen(isInCollectablesScreen);
            }
            else
            {
                Time.timeScale = 1;
                player.Enable(true);
                uiManager.ToggleCollectablesScreen(isInCollectablesScreen);
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

    public void FreezeFrames(float _duration)
    {
        StartCoroutine(CountFreezeFrames(_duration));
    }

    #endregion

    #region Internals

    public void Init()
    {
        Singleton();

        if (initInputManager)
        {
            inputManager = FindObjectOfType<InputManager>();
        }

        if (initPlayer)
        {
            player = FindObjectOfType<PlayerEntity>();
        }

        if (initUIManager)
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        if (initCameraManager)
        {
            FindObjectOfType<CameraManager>().Init();
        }

        if (initRoomSystem)
        {
            roomSystem = FindObjectOfType<RoomSystem>();
        }
    }

    public void Setup()
    {
        if (initInputManager)
        {
            inputManager.Setup();
        }

        if (initPlayer)
        {
            player.SetUpEntity();
        }

        if (initUIManager)
        {
            uiManager.Setup();
        }

        if (initRoomSystem)
        {
            roomSystem.Setup();
        }
    }

    public static GameManager Instance;
    void Singleton()
    {
        if (!Instance)
            Instance = this;
    }

    IEnumerator CountFreezeFrames(float _time)
    {
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(_time);

        Time.timeScale = 1;
    }

    #endregion
}