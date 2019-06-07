namespace GodHunt.Inputs
{
    using UnityEngine;
    using System.Collections;
    using XInputDotNetPure;
    using System;

    /// <summary>
    /// Classe che contiene la struttura dati dei gamepad
    /// </summary>
    public class IntellGamePad
    {
        #region Delegates
        public Action<IntellGamePad, Buttons> OnButtonPressed;
        public Action<IntellGamePad, Buttons> OnButtonHold;
        public Action<IntellGamePad, Buttons> OnButtonReleased;
        public Action<IntellGamePad, Vector2> OnLeftStickAxesChange;
        public Action<IntellGamePad, Vector2> OnRightStickAxesChange;
        public Action<IntellGamePad, float> OnRightTriggerChange;
        public Action<IntellGamePad, float> OnLeftTriggerChange;
        #endregion

        #region Struct
        [Serializable]
        public struct Settings
        {
            [Range(0, 1)]
            public float rightStickDeadzone;
            [Range(0, 1)]
            public float leftStickDeadzone;

            public Settings(float _rightStickDeadzone, float _leftStickDeadzone)
            {
                rightStickDeadzone = Mathf.Abs(_rightStickDeadzone);
                leftStickDeadzone = Mathf.Abs(_leftStickDeadzone);
            }
        }

        public enum Buttons
        {
            None = 0,
            A = 1,
            B = 2,
            X = 3,
            Y = 4,
            RB = 5,
            LB = 6,
            Start = 7,
            Back = 8,
            Guide = 9,
            LeftStickButton = 10,
            RightStickButton = 11,
            DPadDown = 12,
            DPadUp = 13,
            DPadRight = 14,
            DPadLeft = 15,
            LeftStick = 16,
            RightStick = 17,
            LT = 18,
            RT = 19,
        }
        #endregion

        public Settings currentSettings;
        public int ID;
        public GamePadState OldGamePadState;

        public GamePadState CurrentGamePadState
        {
            get
            {
                return _currentGamePadState;
            }

            set
            {
                OldGamePadState = _currentGamePadState;
                _currentGamePadState = value;
                CheckButtons();
            }
        }
        private GamePadState _currentGamePadState;

        public IntellGamePad(GamePadState gamePadState, int id)
        {
            CurrentGamePadState = gamePadState;
            ID = id;
        }

        private void CheckButtons()
        {
            #region Buttons
            #region X
            //X Button
            if (OldGamePadState.Buttons.X == ButtonState.Released && CurrentGamePadState.Buttons.X == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.X);
            }
            else if (OldGamePadState.Buttons.X == ButtonState.Pressed && CurrentGamePadState.Buttons.X == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.X);
            }
            else if (OldGamePadState.Buttons.X == ButtonState.Pressed && CurrentGamePadState.Buttons.X == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.X);
            }
            #endregion

            #region A
            //A Button
            if (OldGamePadState.Buttons.A == ButtonState.Released && CurrentGamePadState.Buttons.A == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.A);
            }
            else if (OldGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState.Buttons.A == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.A);
            }
            else if (OldGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState.Buttons.A == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.A);
            }
            #endregion

            #region B
            //B Button
            if (OldGamePadState.Buttons.B == ButtonState.Released && CurrentGamePadState.Buttons.B == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.B);
            }
            else if (OldGamePadState.Buttons.B == ButtonState.Pressed && CurrentGamePadState.Buttons.B == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.B);
            }
            else if (OldGamePadState.Buttons.B == ButtonState.Pressed && CurrentGamePadState.Buttons.B == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.B);
            }
            #endregion

            #region Y
            //Y Button
            if (OldGamePadState.Buttons.Y == ButtonState.Released && CurrentGamePadState.Buttons.Y == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.Y);
            }
            else if (OldGamePadState.Buttons.Y == ButtonState.Pressed && CurrentGamePadState.Buttons.Y == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.Y);
            }
            else if (OldGamePadState.Buttons.Y == ButtonState.Pressed && CurrentGamePadState.Buttons.Y == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.Y);
            }
            #endregion

            #region Start
            //Start Button
            if (OldGamePadState.Buttons.Start == ButtonState.Released && CurrentGamePadState.Buttons.Start == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.Start);
            }
            else if (OldGamePadState.Buttons.Start == ButtonState.Pressed && CurrentGamePadState.Buttons.Start == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.Start);
            }
            else if (OldGamePadState.Buttons.Start == ButtonState.Pressed && CurrentGamePadState.Buttons.Start == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.Start);
            }
            #endregion

            #region Back
            //Back Button
            if (OldGamePadState.Buttons.Back == ButtonState.Released && CurrentGamePadState.Buttons.Back == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.Back);
            }
            else if (OldGamePadState.Buttons.Back == ButtonState.Pressed && CurrentGamePadState.Buttons.Back == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.Back);
            }
            else if (OldGamePadState.Buttons.Back == ButtonState.Pressed && CurrentGamePadState.Buttons.Back == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.Back);
            }
            #endregion

            #region Guide
            //Guide Button
            if (OldGamePadState.Buttons.Guide == ButtonState.Released && CurrentGamePadState.Buttons.Guide == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.Guide);
            }
            else if (OldGamePadState.Buttons.Guide == ButtonState.Pressed && CurrentGamePadState.Buttons.Guide == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.Guide);
            }
            else if (OldGamePadState.Buttons.Guide == ButtonState.Pressed && CurrentGamePadState.Buttons.Guide == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.Guide);
            }
            #endregion

            #region RB
            //RB Button
            if (OldGamePadState.Buttons.RightShoulder == ButtonState.Released && CurrentGamePadState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.RB);
            }
            else if (OldGamePadState.Buttons.RightShoulder == ButtonState.Pressed && CurrentGamePadState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.RB);
            }
            else if (OldGamePadState.Buttons.RightShoulder == ButtonState.Pressed && CurrentGamePadState.Buttons.RightShoulder == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.RB);
            }
            #endregion

            #region LB
            //LB Button
            if (OldGamePadState.Buttons.LeftShoulder == ButtonState.Released && CurrentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.LB);
            }
            else if (OldGamePadState.Buttons.LeftShoulder == ButtonState.Pressed && CurrentGamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.LB);
            }
            else if (OldGamePadState.Buttons.LeftShoulder == ButtonState.Pressed && CurrentGamePadState.Buttons.LeftShoulder == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.LB);
            }
            #endregion

            #region Left Stick Press
            //Left Stick Press Button
            if (OldGamePadState.Buttons.LeftStick == ButtonState.Released && CurrentGamePadState.Buttons.LeftStick == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.LeftStickButton);
            }
            else if (OldGamePadState.Buttons.LeftStick == ButtonState.Pressed && CurrentGamePadState.Buttons.LeftStick == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.LeftStickButton);
            }
            else if (OldGamePadState.Buttons.LeftStick == ButtonState.Pressed && CurrentGamePadState.Buttons.LeftStick == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.LeftStickButton);
            }
            #endregion

            #region Right Stick Press
            //Right Stick Press Button
            if (OldGamePadState.Buttons.RightStick == ButtonState.Released && CurrentGamePadState.Buttons.RightStick == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.RightStickButton);
            }
            else if (OldGamePadState.Buttons.RightStick == ButtonState.Pressed && CurrentGamePadState.Buttons.RightStick == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.RightStickButton);
            }
            else if (OldGamePadState.Buttons.RightStick == ButtonState.Pressed && CurrentGamePadState.Buttons.RightStick == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.RightStickButton);
            }
            #endregion
            #endregion

            #region DPAD

            #region Up
            if (OldGamePadState.DPad.Up == ButtonState.Released && CurrentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.DPadUp);
            }
            else if (OldGamePadState.DPad.Up == ButtonState.Pressed && CurrentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.DPadUp);
            }
            else if (OldGamePadState.DPad.Up == ButtonState.Pressed && CurrentGamePadState.DPad.Up == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.DPadUp);
            }
            #endregion

            #region Down
            if (OldGamePadState.DPad.Down == ButtonState.Released && CurrentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.DPadDown);
            }
            else if (OldGamePadState.DPad.Down == ButtonState.Pressed && CurrentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.DPadDown);
            }
            else if (OldGamePadState.DPad.Down == ButtonState.Pressed && CurrentGamePadState.DPad.Down == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.DPadDown);
            }
            #endregion

            #region Right
            if (OldGamePadState.DPad.Right == ButtonState.Released && CurrentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.DPadRight);
            }
            else if (OldGamePadState.DPad.Right == ButtonState.Pressed && CurrentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.DPadRight);
            }
            else if (OldGamePadState.DPad.Right == ButtonState.Pressed && CurrentGamePadState.DPad.Right == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.DPadRight);
            }
            #endregion

            #region Left
            if (OldGamePadState.DPad.Left == ButtonState.Released && CurrentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                OnButtonPressed?.Invoke(this, Buttons.DPadLeft);
            }
            else if (OldGamePadState.DPad.Left == ButtonState.Pressed && CurrentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                OnButtonHold?.Invoke(this, Buttons.DPadLeft);
            }
            else if (OldGamePadState.DPad.Left == ButtonState.Pressed && CurrentGamePadState.DPad.Left == ButtonState.Released)
            {
                OnButtonReleased?.Invoke(this, Buttons.DPadLeft);
            }
            #endregion

            #endregion

            #region Axes
            #region Left Stick Axes
            //Left stick Axes
            float lsX = CurrentGamePadState.ThumbSticks.Left.X;
            float lsY = CurrentGamePadState.ThumbSticks.Left.Y;

            Vector2 leftStickInput = new Vector2(Math.Abs(lsX) > currentSettings.leftStickDeadzone ? lsX : 0, Math.Abs(lsY) > currentSettings.leftStickDeadzone ? lsY : 0);

            OnLeftStickAxesChange?.Invoke(this, leftStickInput);
            #endregion

            #region Right Stick Axes
            //Right stick Axes
            float rsX = CurrentGamePadState.ThumbSticks.Right.X;
            float rsY = CurrentGamePadState.ThumbSticks.Right.Y;

            Vector2 rightStickInput = new Vector2(Math.Abs(rsX) > currentSettings.rightStickDeadzone ? rsX : 0, Math.Abs(rsY) > currentSettings.rightStickDeadzone ? rsY : 0);

            OnRightStickAxesChange?.Invoke(this, leftStickInput);
            #endregion

            #region Right Trigger Axis
            OnRightTriggerChange?.Invoke(this, CurrentGamePadState.Triggers.Right);
            #endregion

            #region Left Trigger Axis
            OnLeftTriggerChange?.Invoke(this, CurrentGamePadState.Triggers.Left);
            #endregion
            #endregion
        }

        public void SetSettings(Settings _settings)
        {
            currentSettings = _settings;
        }
    }
}