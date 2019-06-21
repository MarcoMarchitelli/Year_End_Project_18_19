using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestInputManager : MonoBehaviour
{
    [HideInInspector] public List<InputKey> InputKeys;
    [SerializeField] EventSystem eventSystemPrefab;
    [SerializeField] Button firstSelected;

    #region Vars

    public static TestInputManager Instance;

    [HideInInspector] public EventSystem eventSystem;
    StandaloneInputModule inputModule;

    bool setupped;
    bool _padConnected;
    bool PadConnected
    {
        get { return _padConnected; }
        set
        {
            if (value != _padConnected)
            {
                _padConnected = value;
                ChangeCurrentInputDevice(_padConnected ? XBOXCONTROLLER : KEYBOARD);
            }
        }
    }
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

    const string KEYBOARD = "Keyboard ";
    const string XBOXCONTROLLER = "XBOX ";
    const float DEVICE_CHECK_INTERVAL = .5f;

    string[] joyNames;

    #endregion

    #region Monos

    private void Update()
    {
        if (!setupped)
            return;

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

        if (joyNames.Length > 0)
            foreach (string joyName in joyNames)
            {
                if (joyName.Contains("XBOX"))
                {
                    PadConnected = true;
                    break;
                }
                else if (joyName.Length == 0)
                {
                    PadConnected = false;
                }
            }
    }

    void ChangeCurrentInputDevice(string _newInputDeviceName)
    {
        CurrentInputDevice = _newInputDeviceName;

        //For menu navigation.
        switch (CurrentInputDevice)
        {
            case KEYBOARD:
                inputModule.horizontalAxis = KEYBOARD + "Horizontal";
                inputModule.verticalAxis = KEYBOARD + "Vertical";
                inputModule.submitButton = KEYBOARD + "Submit";
                inputModule.cancelButton = KEYBOARD + "Cancel";
                break;
            case XBOXCONTROLLER:
                inputModule.horizontalAxis = XBOXCONTROLLER + "DPAD Horizontal";
                inputModule.verticalAxis = XBOXCONTROLLER + "DPAD Vertical";
                inputModule.submitButton = XBOXCONTROLLER + "Submit";
                inputModule.cancelButton = XBOXCONTROLLER + "Cancel";
                break;
        }

        print(CurrentInputDevice);
    }

    IEnumerator DeviceCheckRoutine()
    {
        while (true)
        {
            CheckInputDevices();
            yield return new WaitForSecondsRealtime(DEVICE_CHECK_INTERVAL);
        }
    }

    #endregion

    #region API

    public void Setup()
    {
        Singleton();

        eventSystem = Instantiate(eventSystemPrefab);
        inputModule = eventSystem.GetComponent<StandaloneInputModule>();

        if (firstSelected)
            eventSystem.firstSelectedGameObject = firstSelected.gameObject;

        ChangeCurrentInputDevice(KEYBOARD);
        //string[] joyNames = Input.GetJoystickNames();
        //if (joyNames.Length > 0)
        //    for (int i = 0; i < joyNames.Length; i++)
        //    {
        //        if (joyNames[i].Length == 33)
        //            ChangeCurrentInputDevice(xboxController);
        //    }

        StartCoroutine("DeviceCheckRoutine");

        setupped = true;
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