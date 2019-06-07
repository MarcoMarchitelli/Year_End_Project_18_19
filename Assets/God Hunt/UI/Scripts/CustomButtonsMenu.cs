using UnityEngine;

public class CustomButtonsMenu : MonoBehaviour
{
    CustomButton[] buttons;
    CustomButton currentlySelectedButton;

    public void Init()
    {
        buttons = GetComponentsInChildren<CustomButton>();
    }

    public void Setup()
    {
        currentlySelectedButton = buttons[0];
    }
}