using UnityEngine;

namespace GodHunt.Inputs
{
    [RequireComponent(typeof(InputChecker))]
    public class InputManager : MonoBehaviour
    {
        [SerializeField] GamePadBindings gamePadBindings;
        [SerializeField] KeyboardBindings keyboardBindings;

        public static InputDevice CurrentInputDevice;
        public static System.Action<InputDevice> OnInputDeviceChange;

        #region Buttons Events
        public static System.Action OnJumpPressed;
        public static System.Action OnJumpReleased;
        public static System.Action OnRunPressed;
        public static System.Action OnRunReleased;
        public static System.Action OnAttackPressed;
        public static System.Action OnAttackReleased;
        public static System.Action OnDashPressed;
        public static System.Action OnDashReleased;
        public static System.Action OnMapPressed;
        public static System.Action OnMapReleased;
        public static System.Action OnInventoryPressed;
        public static System.Action OnInventoryReleased;
        public static System.Action OnPausePressed;
        public static System.Action OnPauseReleased;
        #endregion

        #region Axes events
        public static System.Action<Vector2> OnMovementInput;
        #endregion

        public void Setup()
        {
            InputChecker.OnGamepadConnected += HandleControllerConnection;
            InputChecker.OnGamepadDisconnected += HandleControllerDisconnection;

            CurrentInputDevice = InputDevice.keyboard;
        }

        private void Update()
        {
            if(CurrentInputDevice == InputDevice.keyboard)
            {
                if (Input.GetKeyDown(keyboardBindings.Attack))
                    OnAttackPressed?.Invoke();
                if (Input.GetKeyUp(keyboardBindings.Attack))
                    OnAttackReleased?.Invoke();

                if (Input.GetKeyDown(keyboardBindings.Dash))
                    OnDashPressed?.Invoke();        
                if (Input.GetKeyUp(keyboardBindings.Dash))
                    OnDashReleased?.Invoke();

                if (Input.GetKeyDown(keyboardBindings.Run))
                    OnRunPressed?.Invoke();        
                if (Input.GetKeyUp(keyboardBindings.Run))
                    OnRunReleased?.Invoke();

                if (Input.GetKeyDown(keyboardBindings.Jump))
                    OnJumpPressed?.Invoke();        
                if (Input.GetKeyUp(keyboardBindings.Jump))
                    OnJumpReleased?.Invoke();

                if (Input.GetKeyDown(keyboardBindings.Pause))
                    OnPausePressed?.Invoke();
                if (Input.GetKeyUp(keyboardBindings.Pause))
                    OnPauseReleased?.Invoke();

                if (Input.GetKeyDown(keyboardBindings.Map))
                    OnMapPressed?.Invoke();
                if (Input.GetKeyUp(keyboardBindings.Map))
                    OnMapReleased?.Invoke();

                if (Input.GetKeyDown(keyboardBindings.Inventory))
                    OnInventoryPressed?.Invoke();
                if (Input.GetKeyUp(keyboardBindings.Inventory))
                    OnInventoryReleased?.Invoke();

                OnMovementInput?.Invoke(new Vector2(Input.GetAxisRaw(keyboardBindings.HorizontalAxis), Input.GetAxisRaw(keyboardBindings.VerticalAxis)));
            }
        }

        #region GamePad events handlers
        void HandleControllerConnection(IntellGamePad _gamePad)
        {
            if (CurrentInputDevice == InputDevice.keyboard)
            {
                CurrentInputDevice = InputDevice.gamePad;
                OnInputDeviceChange?.Invoke(CurrentInputDevice);
            }

            _gamePad.OnButtonPressed += HandleButtonPress;
            _gamePad.OnButtonReleased += HandleButtonRelease;
            _gamePad.OnLeftStickAxesChange += HandleLeftStickAxes;
        }

        void HandleControllerDisconnection(IntellGamePad _gamePad)
        {
            if (CurrentInputDevice == InputDevice.gamePad)
            {
                CurrentInputDevice = InputDevice.keyboard;
                OnInputDeviceChange?.Invoke(CurrentInputDevice);
            }

            _gamePad.OnButtonPressed -= HandleButtonPress;
            _gamePad.OnButtonReleased -= HandleButtonRelease;
            _gamePad.OnLeftStickAxesChange -= HandleLeftStickAxes;
        }
        #endregion

        #region Buttons/Axes check
        void HandleButtonPress(IntellGamePad _pad, Buttons _button)
        {
            if (_button == gamePadBindings.Attack)
            {
                OnAttackPressed?.Invoke();
                return;
            }

            if (_button == gamePadBindings.Jump)
            {
                OnJumpPressed?.Invoke();
                return;
            }

            if (_button == gamePadBindings.Dash)
            {
                OnDashPressed?.Invoke();
                return;
            }

            if (_button == gamePadBindings.Run)
            {
                OnRunPressed?.Invoke();
                return;
            }

            if (_button == gamePadBindings.Pause)
            {
                OnPausePressed?.Invoke();
                return;
            }

            if (_button == gamePadBindings.Map)
            {
                OnMapPressed?.Invoke();
                return;
            }

            if (_button == gamePadBindings.Inventory)
            {
                OnInventoryPressed?.Invoke();
                return;
            }
        }

        void HandleButtonRelease(IntellGamePad _pad, Buttons _button)
        {
            if (_button == gamePadBindings.Attack)
            {
                OnAttackReleased?.Invoke();
                return;
            }

            if (_button == gamePadBindings.Jump)
            {
                OnJumpReleased?.Invoke();
                return;
            }

            if (_button == gamePadBindings.Dash)
            {
                OnDashReleased?.Invoke();
                return;
            }

            if (_button == gamePadBindings.Run)
            {
                OnRunReleased?.Invoke();
                return;
            }

            if (_button == gamePadBindings.Pause)
            {
                OnPauseReleased?.Invoke();
                return;
            }

            if (_button == gamePadBindings.Map)
            {
                OnMapReleased?.Invoke();
                return;
            }

            if (_button == gamePadBindings.Inventory)
            {
                OnInventoryReleased?.Invoke();
                return;
            }
        }

        void HandleLeftStickAxes(IntellGamePad _pad, Vector2 _direction)
        {
            OnMovementInput?.Invoke(_direction);
        }
        #endregion

        [System.Serializable]
        public class GamePadBindings
        {
            public enum InputType { button, dpad, trigger, stick }

            [Header("Game Pad")]
            public Buttons Jump;
            public Buttons Attack;
            public Buttons Dash;
            public Buttons Run;
            public Buttons Pause;
            public Buttons Map;
            public Buttons Inventory;
        }

        [System.Serializable]
        public class KeyboardBindings
        {
            [Header("Keyboard")]
            public KeyCode Jump;
            public KeyCode Attack;
            public KeyCode Dash;
            public KeyCode Run;
            public KeyCode Pause;
            public KeyCode Map;
            public KeyCode Inventory;
            public string HorizontalAxis;
            public string VerticalAxis;
        }
    }

    public enum InputDevice { gamePad, keyboard }
}