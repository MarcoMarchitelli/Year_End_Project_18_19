using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionSystem
{
    public class PlayerActionsBehaviour : MonoBehaviour
    {
        public List<Action> Actions;

        public void RemoveAction(int _index)
        {
            if (Actions == null)
                Actions = new List<Action>();

            Action actionToRemove = Actions[_index];
            Actions.RemoveAt(_index);
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].InterruptableActions.Contains(actionToRemove))
                    Actions[i].InterruptableActions.Remove(actionToRemove);
            }
        }

        public void AddAction()
        {
            if (Actions == null)
                Actions = new List<Action>();

            Actions.Add(new Action());

            if (Actions.Count > 1)
                for (int i = 0; i < Actions.Count; i++)
                {
                    for (int j = 0; j < Actions.Count; j++)
                    {
                        if (Actions[i] != Actions[j] && !Actions[i].InterruptableActions.Contains(Actions[j]))
                            Actions[i].InterruptableActions.Add(Actions[j]);
                    }
                }
        }
    } 
}