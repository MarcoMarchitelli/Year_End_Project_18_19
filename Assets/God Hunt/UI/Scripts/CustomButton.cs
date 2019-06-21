using UnityEngine;
using UnityEngine.UI;
using GodHunt.Inputs;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public enum State { mouse, controller }

    public Image image;
    public bool ChangeImageColor;
    public Color SelectedColor, UnselectedColor;

    public UnityVoidEvent OnSelect, OnDeselect, OnClick;

    private State currentState;
    private bool mouseDowned;
    private CustomButtonsMenu menu;

    #region API
    public void Setup(CustomButtonsMenu _menu = null)
    {
        menu = _menu;

        InputChecker.OnGamepadConnected += HandleControllerConnection;
        InputChecker.OnGamepadDisconnected += HandleControllerDisonnection;

        if (InputManager.CurrentInputDevice == InputDevice.gamePad)
            currentState = State.controller;
        else
            currentState = State.mouse;

        print(name + " setup");
    }

    public void Select()
    {
        print(name + " selected!");
        if (ChangeImageColor)
        {
            image.color = SelectedColor;
            image.SetAllDirty();
        }
        menu?.DeselectAll();
        OnSelect.Invoke();
    }

    public void Deselect()
    {
        print(name + " deselected!");
        if (ChangeImageColor)
        {
            image.color = UnselectedColor;
            image.SetAllDirty();
        }
        OnDeselect.Invoke();
    }

    public void Click()
    {
        OnClick.Invoke();
    } 
    #endregion

    private void HandleControllerConnection(IntellGamePad _pad)
    {
        currentState = State.controller;
    }

    private void HandleControllerDisonnection(IntellGamePad _pad)
    {
        currentState = State.mouse;
    }

    #region Pointer Events
    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == State.mouse)
            mouseDowned = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == State.mouse)
            Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == State.mouse)
        {
            Deselect();
            mouseDowned = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentState == State.mouse && mouseDowned)
        {
            mouseDowned = false;
            Click();
        }
    } 
    #endregion
}