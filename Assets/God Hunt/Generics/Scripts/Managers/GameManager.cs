﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] bool initPlayer;
    [SerializeField] bool initUIManager;
    [SerializeField] bool initRoomSystem;
    [SerializeField] bool initInputManager;

    private UIManager uiManager;
    [HideInInspector] public PlayerEntity player;
    [HideInInspector] public RoomSystem roomSystem;
    private InputManager inputManager;

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
        Singleton();

        if (initInputManager)
        {
            inputManager = FindObjectOfType<InputManager>();
            inputManager.Setup();
        }

        if (initUIManager)
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        if (initPlayer)
        {
            player = FindObjectOfType<PlayerEntity>();
            player.SetUpEntity();
        }

        if (initRoomSystem)
        {
            roomSystem = FindObjectOfType<RoomSystem>();
            roomSystem.Setup();
        }
    }

    public static GameManager Instance;
    void Singleton()
    {
        if (!Instance)
            Instance = this;
    }

    #endregion
}