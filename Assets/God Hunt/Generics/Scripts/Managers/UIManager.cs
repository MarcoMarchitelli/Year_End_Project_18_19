using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] GameObject gameplayPanel;
    public PlayerHPUI playerHPUI;
    [Header("Pause")]
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject firstSelectedObject;
    [SerializeField] GameObject controllerCommandsScheme;
    [SerializeField] GameObject keyboardCommandsScheme;
    [Header("Map")]
    [SerializeField] GameObject mapPanel;
    [Header("Inventory")]
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject firstSelectedItem;
    [Header("????")]
    [SerializeField] GameObject collectablesScreen;

    bool controllerSchemeActive = true;

    #region API

    public void Setup()
    {
        PlayerEntityData playerData = GameManager.Instance.player.Data as PlayerEntityData;

        playerHPUI.damageReceiver = playerData.damageReceiverBehaviour;
        playerHPUI.Setup();
    }

    public void ToggleInventoryPanel(bool _value)
    {
        if (inventoryPanel)
            inventoryPanel.SetActive(_value);

        if (_value == true && firstSelectedItem != null)
        {
            InputManager.Instance.eventSystem.SetSelectedGameObject(firstSelectedItem);
        }
    }

    public void ToggleCollectablesScreen(bool _value)
    {
        if (collectablesScreen)
            collectablesScreen.SetActive(_value);
    }

    public void ToggleGameplayPanel(bool _value)
    {
        if (gameplayPanel)
            gameplayPanel.SetActive(_value);
    }

    public void TogglePausePanel(bool _value)
    {
        if ( pausePanel != null )
            pausePanel.SetActive( _value );

        if ( _value == true && firstSelectedObject != null )
        {
            InputManager.Instance.eventSystem.SetSelectedGameObject( firstSelectedObject );
        }
    }

    public void ToggleMapPanel(bool _value)
    {
        if (mapPanel)
            mapPanel.SetActive(_value);
    }

    public void ToggleCommandsScheme()
    {
        if (controllerSchemeActive)
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