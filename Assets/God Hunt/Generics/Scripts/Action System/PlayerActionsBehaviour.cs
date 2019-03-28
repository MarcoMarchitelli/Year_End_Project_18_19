using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsBehaviour : MonoBehaviour
{
    public List<Action> Actions;

    public void RemoveAction(int _index)
    {
        Actions.RemoveAt(_index);
    }

    public void AddAction()
    {
        Actions.Add(new Action());
    }
}