using UnityEngine;
using GodHunt.Inputs;

public class CustomButtonsMenu : MonoBehaviour
{
    [SerializeField] CustomButton[] buttons;
    CustomButton currentlySelectedButton;
    int currentlySelectedButtonIndex;
    bool subscribed;

    #region API
    public void Setup()
    {
        foreach (CustomButton customButton in buttons)
        {
            customButton.Setup(this);
        }

        currentlySelectedButtonIndex = 0;
        currentlySelectedButton = buttons[currentlySelectedButtonIndex];
        currentlySelectedButton.Select();

        ToggleEventsSubscriptions(true);
    }

    public void DeselectAll()
    {
        foreach (CustomButton button in buttons)
        {
            button.Deselect();
        }
    }
    #endregion

    #region Monos
    private void OnDisable()
    {
        ToggleEventsSubscriptions(false);
    }

    private void OnEnable()
    {
        ToggleEventsSubscriptions(true);

        currentlySelectedButtonIndex = 0;
        currentlySelectedButton = buttons[currentlySelectedButtonIndex];
        currentlySelectedButton.Select();
    } 
    #endregion

    private void ToggleEventsSubscriptions(bool _value)
    {
        if (_value && subscribed == false)
        {
            InputManager.OnSelectPressed += () => currentlySelectedButton.Click();
            InputManager.OnSelectionUpPressed += () => ChangeSelection(1);
            InputManager.OnSelectionDownPressed += () => ChangeSelection(-1);
            subscribed = true;
        }
        else if(subscribed == true)
        {
            InputManager.OnSelectPressed -= () => currentlySelectedButton.Click();
            InputManager.OnSelectionUpPressed -= () => ChangeSelection(1);
            InputManager.OnSelectionDownPressed -= () => ChangeSelection(-1);
            subscribed = false;
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