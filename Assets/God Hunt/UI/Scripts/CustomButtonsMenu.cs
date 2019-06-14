using UnityEngine;
using GodHunt.Inputs;

public class CustomButtonsMenu : MonoBehaviour
{
    [SerializeField] CustomButton[] buttons;
    CustomButton currentlySelectedButton;
    int currentlySelectedButtonIndex;

    public void Setup()
    {
        foreach (CustomButton customButton in buttons)
        {
            customButton.Setup();
        }

        currentlySelectedButtonIndex = 0;
        currentlySelectedButton = buttons[currentlySelectedButtonIndex];
        currentlySelectedButton.Select();

        ToggleEventsSubscriptions(true);
    }

    private void OnDisable()
    {
        ToggleEventsSubscriptions(false);
    }

    private void ToggleEventsSubscriptions(bool _value)
    {
        if (_value)
        {
            InputManager.OnSelectPressed += () => currentlySelectedButton.Click();
            InputManager.OnSelectionUpPressed += () => ChangeSelection(1);
            InputManager.OnSelectionDownPressed += () => ChangeSelection(-1);
        }
        else
        {
            InputManager.OnSelectPressed -= () => currentlySelectedButton.Click();
            InputManager.OnSelectionUpPressed -= () => ChangeSelection(1);
            InputManager.OnSelectionDownPressed -= () => ChangeSelection(-1);
        }
    }

    //1 = up, -1 = down
    private void ChangeSelection(int _dir)
    {
        switch (_dir)
        {
            case 1:
                if (currentlySelectedButtonIndex == 0)
                    currentlySelectedButtonIndex = buttons.Length - 1;
                else
                    currentlySelectedButtonIndex--;

                currentlySelectedButton.Deselect();
                currentlySelectedButton = buttons[currentlySelectedButtonIndex];
                currentlySelectedButton.Select();
                break;
            case -1:
                if (currentlySelectedButtonIndex == buttons.Length - 1)
                    currentlySelectedButtonIndex = 0;
                else
                    currentlySelectedButtonIndex++;

                currentlySelectedButton.Deselect();
                currentlySelectedButton = buttons[currentlySelectedButtonIndex];
                currentlySelectedButton.Select();
                break;
        }
    }
}