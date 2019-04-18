using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionSystem
{
    [System.Serializable]
    public class Action
    {
        #region Data

        public string Name;

        public bool Input;
        public KeyCode InputKey;
        public bool Hold;
        public float HoldTime;

        public float Duration;

        public bool InterruptOtherActions;
        public List<Action> ActionsToInterrupt;
        public List<Action> InterruptableActions;

        #endregion

        #region Status

        public bool enabled;

        #endregion

        #region API

        public void AddActionToInterrupt(Action _action)
        {
            if (ActionsToInterrupt == null)
                ActionsToInterrupt = new List<Action>();

            ActionsToInterrupt.Add(_action);
        }

        public void RemoveActionToInterrupt(int _index)
        {
            ActionsToInterrupt.RemoveAt(_index);
        }

        public void RemoveActionToInterrupt(Action _action)
        {
            ActionsToInterrupt.Remove(_action);
        }

        #endregion

        public Action()
        {
            Name = "New Action";
            ActionsToInterrupt = new List<Action>();
            InterruptableActions = new List<Action>();
        }
    } 
}