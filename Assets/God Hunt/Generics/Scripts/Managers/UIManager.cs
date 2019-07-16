using UnityEngine;
using UnityEngine.UI;

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
    [Header("Options")]
    [SerializeField] GameObject optionsPanel;
    [Header("Map")]
    [SerializeField] GameObject mapPanel;
    [Header("Inventory")]
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject firstSelectedItem;
    [Header("????")]
    [SerializeField] GameObject collectablesScreen;

    Options options;
    bool controllerSchemeActive = true;
    Button _firstSelectedPauseButton;
    public Button firstSelectedPauseButton
    {
        get
        {
            if (!_firstSelectedPauseButton)
                _firstSelectedPauseButton = pausePanel.GetComponentInChildren<Button>();

            return _firstSelectedPauseButton;
        }
    }

    #region API

    public void Setup()
    {
        PlayerEntityData playerData = GameManager.Instance.player.Data as PlayerEntityData;

        playerHPUI.damageReceiver = playerData.damageReceiverBehaviour;
        playerHPUI.Setup();

        options = optionsPanel?.GetComponentInChildren<Options>();

        pausePanel.SetActive(false);
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
        if (pausePanel != null)
        {
            pausePanel.SetActive(_value);
            if (_value)
                firstSelectedPauseButton.Select();
        }
    }

    public void ToggleoptionsPanel(bool _value)
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(_value);
            if (_value)
                options.Open();
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