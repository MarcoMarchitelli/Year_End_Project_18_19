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
    EventSystem eventSystem;
    StandaloneInputModule inputModule;

    #endregion

    bool isPaused = false;
    bool isMapOpen = false;
    bool isInInventory = false;

    string _currentInputDevice;
    public string CurrentInputDevice
    {
        get { return _currentInputDevice; }
        private set { _currentInputDevice = value; }
    }
    const string keyboard = "Keyboard ";
    const string xboxController = "XBOX ";

    #region MonoBehaviour Methods

    private void Awake()
    {
        Setup();
    }

    private void Update()
    {
        CheckInputDevices();

        if (!isMapOpen && !isInInventory && Input.GetButtonDown(CurrentInputDevice + "Pause"))
            TogglePause();

        if (!isPaused && !isInInventory && Input.GetButtonDown(CurrentInputDevice + "Map"))
            ToggleMap();

        if (!isPaused && !isMapOpen && Input.GetButtonDown(CurrentInputDevice + "Inventory"))
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

    #region Internals

    void Setup()
    {
        if (!Instance)
            Instance = this;

        roomSystem.Setup();

        eventSystem = FindObjectOfType<EventSystem>();
        inputModule = FindObjectOfType<StandaloneInputModule>();

        ChangeCurrentInputDevice(keyboard);
        string[] joyNames = Input.GetJoystickNames();
        if (joyNames.Length > 0)
            for (int i = 0; i < joyNames.Length; i++)
            {
                if (joyNames[i].Length == 33)
                    ChangeCurrentInputDevice(xboxController);
            }
    }

    string[] joyNames;
    void CheckInputDevices()
    {
        joyNames = Input.GetJoystickNames();

        //if current device is keyboard and i detect a controller --> change to controller
        if (CurrentInputDevice == keyboard && joyNames.Length > 0)
            for (int i = 0; i < joyNames.Length; i++)
            {
                if (joyNames[i].Length == 33)
                    ChangeCurrentInputDevice(xboxController);
            }
        //If current device is controller but i do not detect it anymore --> change to keyboard
        else if(CurrentInputDevice == xboxController && joyNames.Length == 0)
        {
            ChangeCurrentInputDevice(keyboard);
        }
    }

    void ChangeCurrentInputDevice(string _newInputDeviceName)
    {
        CurrentInputDevice = _newInputDeviceName;

        switch (CurrentInputDevice)
        {
            case keyboard:
                inputModule.horizontalAxis = keyboard + "Horizontal";
                inputModule.verticalAxis = keyboard + "Vertical";
                inputModule.submitButton = keyboard + "Submit";
                inputModule.cancelButton = keyboard + "Cancel";
                break;
            case xboxController:
                inputModule.horizontalAxis = xboxController + "DPAD Horizontal";
                inputModule.verticalAxis = xboxController + "DPAD Vertical";
                inputModule.submitButton = xboxController + "Submit";
                inputModule.cancelButton = xboxController + "Cancel";
                break;
        }

        print(CurrentInputDevice);
    }

    #endregion

}