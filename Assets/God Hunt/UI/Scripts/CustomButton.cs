using UnityEngine;
using UnityEngine.UI;
using GodHunt.Inputs;

[RequireComponent(typeof(Image))]
public class CustomButton : MonoBehaviour
{
    public enum State { mouse, controller }

    public UnityVoidEvent OnSelect, OnDeselect, OnClick;

    private State currentState;
    private bool mouseDowned;

    public void Setup()
    {
        InputChecker.OnGamepadConnected += HandleControllerConnection;
        InputChecker.OnGamepadDisconnected += HandleControllerDisonnection;

        if (InputManager.CurrentInputDevice == InputDevice.gamePad)
            currentState = State.controller;
        else
            currentState = State.mouse;

        print(name + " setup");
    }

    private void HandleControllerConnection(IntellGamePad _pad)
    {
        currentState = State.controller;
    }

    private void HandleControllerDisonnection(IntellGamePad _pad)
    {
        currentState = State.mouse;
    }

    public void Select()
    {
        OnSelect.Invoke();
        print(name + " selected");
    }

    public void Deselect()
    {
        OnDeselect.Invoke();
    }

    public void Click()
    {
        OnClick.Invoke();
    }

    private void OnMouseEnter()
    {
        if (currentState == State.mouse)
            Select();
    }

    private void OnMouseExit()
    {
        if (currentState == State.mouse)
        {
            Deselect();
            mouseDowned = false;
        }
    }

    private void OnMouseDown()
    {
        if (currentState == State.mouse)
            mouseDowned = true;
    }

    private void OnMouseUp()
    {
        if (currentState == State.mouse && mouseDowned)
        {
            mouseDowned = false;
            Click();
        }
    }
}