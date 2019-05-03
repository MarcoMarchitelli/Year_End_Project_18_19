using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject gameplayPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject firstSelectedObject;
    [SerializeField] GameObject mapPanel;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject controllerCommandsScheme;
    [SerializeField] GameObject keyboardCommandsScheme;
    [SerializeField] GameObject collectablesScreen;
    public PlayerHPUI playerHPUI;

    bool controllerSchemeActive = true;
    Button _firstSelectedObjectButton;
    Button FirstSelectedObjectButton
    {
        get
        {
            if (_firstSelectedObjectButton == null)
            {
                _firstSelectedObjectButton = firstSelectedObject.GetComponent<Button>();
            }
            return _firstSelectedObjectButton;
        }
    }

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
            FirstSelectedObjectButton.Select();
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