using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public List<InputKey> InputKeys;

    #region Vars

    public static InputManager Instance;

    public EventSystem eventSystem;
    StandaloneInputModule inputModule;

    string _currentInputDevice;
    public static string CurrentInputDevice
    {
        get
        {
            return Instance == null ? null : Instance._currentInputDevice;
        }
        private set
        {
            Instance._currentInputDevice = Instance == null ? null : value;
        }
    }
    const string keyboard = "Keyboard ";
    const string xboxController = "XBOX ";

    string[] joyNames;

    #endregion

    #region Monos

    private void Update()
    {
        CheckInputDevices();

        if (Input.GetButtonDown(CurrentInputDevice + "Pause"))
            GameManager.Instance.TogglePause();

        if (Input.GetButtonDown(CurrentInputDevice + "Map"))
            GameManager.Instance.ToggleMap();

        if (Input.GetButtonDown(CurrentInputDevice + "Inventory"))
            GameManager.Instance.ToggleInventory();
    }

    #endregion

    #region Internals

    void Singleton()
    {
        if (!Instance)
            Instance = this;
    }

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
        else if (CurrentInputDevice == xboxController && joyNames.Length == 0)
        {
            ChangeCurrentInputDevice(keyboard);
        }
    }

    void ChangeCurrentInputDevice(string _newInputDeviceName)
    {
        CurrentInputDevice = _newInputDeviceName;

        //For menu navigation.
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

    #region API

    public void Setup()
    {
        Singleton();

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

    public void RemoveInputKey(int _index)
    {
        InputKeys.RemoveAt(_index);
    }

    public void AddInputKey()
    {
        InputKeys.Add(new InputKey());
    }

    #endregion

}

public enum InputKeyButton { Horizontal, Vertical, Pause, Map, Inventory, Jump, Attack, Dash, Run, Submit, Cancel }
public enum InputType { Down, Up, Hold }

[System.Serializable]
public class InputKey
{
    public enum AxisTriggerMode { whileOverTreshold, onceOverTreshold }

    public InputKeyButton Key = InputKeyButton.Submit;
    //based on inputkeybutton settings
    public string keyString;
    public bool isAxis = false;
    //if not axis
    public InputType Interaction = InputType.Down;
    //if inputtype hold
    public bool TriggerWhileHolding;
    //if not triggerWhileHolding
    public float TimeToHold;
    //if axis
    [Range(-1, 1)] public float Treshold;
    public AxisTriggerMode TriggerMode;

}