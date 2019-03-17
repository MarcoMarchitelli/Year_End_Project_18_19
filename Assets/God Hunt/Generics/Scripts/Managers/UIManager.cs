using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject gameplayPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject mapPanel;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject controllerCommandsScheme;
    [SerializeField] GameObject keyboardCommandsScheme;

    bool controllerSchemeActive = true;

    #region API

    public void ToggleInventoryPanel(bool _value)
    {
        if(inventoryPanel)
            inventoryPanel.SetActive(_value);
    }

    public void ToggleGameplayPanel(bool _value)
    {
        if(gameplayPanel)
            gameplayPanel.SetActive(_value);
    }

    public void TogglePausePanel(bool _value)
    {
        if(pausePanel)
            pausePanel.SetActive(_value);
    }

    public void ToggleMapPanel(bool _value)
    {
        if(mapPanel)
            mapPanel.SetActive(_value);
    }

    public void ToggleCommandsScheme()
    {
        if(controllerSchemeActive)
        {
            controllerCommandsScheme.SetActive(false);
            keyboardCommandsScheme.SetActive(true);
            controllerSchemeActive = false;
        }
        else
        {
            controllerCommandsScheme.SetActive(true);
            keyboardCommandsScheme.SetActive(false);
            controllerSchemeActive = true;
        }
    }

    #endregion

}
