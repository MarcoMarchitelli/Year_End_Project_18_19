using System.Collections.Generic;
using UnityEngine;
using GodHunt.Inputs;

public class InteractableBehaviour : BaseBehaviour
{
    #region Serialized

    [SerializeField] MonoBehaviour interactionAgent;
    [SerializeField] bool interactOnlyOnce = true;
    [SerializeField] bool interactOnCollision;
    [SerializeField] bool interactOnTrigger;
    [SerializeField] GameInputActions inputAction;
    [SerializeField] UnityVoidEvent OnInteraction;

    #endregion

    bool agentFound = false;
    bool interactable = false;
    GameObject agent;

    private System.Action OnAgentFound;

    #region Overrides

    protected override void CustomSetup()
    {
        switch (inputAction)
        {
            case GameInputActions.OnJumpPressed:
                InputManager.OnJumpPressed += HandleInput;
                break;
            case GameInputActions.OnJumpReleased:
                InputManager.OnJumpReleased += HandleInput;
                break;
            case GameInputActions.OnRunPressed:
                InputManager.OnRunPressed += HandleInput;
                break;
            case GameInputActions.OnRunReleased:
                InputManager.OnRunReleased += HandleInput;
                break;
            case GameInputActions.OnAttackPressed:
                InputManager.OnAttackPressed += HandleInput;
                break;
            case GameInputActions.OnAttackReleased:
                InputManager.OnAttackReleased += HandleInput;
                break;
            case GameInputActions.OnDashPressed:
                InputManager.OnDashPressed += HandleInput;
                break;
            case GameInputActions.OnDashReleased:
                InputManager.OnDashReleased += HandleInput;
                break;
            case GameInputActions.OnMapPressed:
                InputManager.OnMapPressed += HandleInput;
                break;
            case GameInputActions.OnMapReleased:
                InputManager.OnMapReleased += HandleInput;
                break;
            case GameInputActions.OnInventoryPressed:
                InputManager.OnInventoryPressed += HandleInput;
                break;
            case GameInputActions.OnInventoryReleased:
                InputManager.OnInventoryReleased += HandleInput;
                break;
            case GameInputActions.OnPausePressed:
                InputManager.OnPausePressed += HandleInput;
                break;
            case GameInputActions.OnPauseReleased:
                InputManager.OnPauseReleased += HandleInput;
                break;
            case GameInputActions.OnSelectPressed:
                InputManager.OnSelectPressed += HandleInput;
                break;
            case GameInputActions.OnSelectionUpPressed:
                InputManager.OnSelectionUpPressed += HandleInput;
                break;
            case GameInputActions.OnSelectionDownPressed:
                InputManager.OnSelectionDownPressed += HandleInput;
                break;
        }
    }

    #endregion

    #region API

    public void ToggleInteractability(bool _value)
    {
        interactable = _value;
    }

    #endregion

    #region Monos

    private void OnTriggerEnter(Collider other)
    {
        if (!interactOnTrigger)
            return;

        if (other.GetComponent(interactionAgent.GetType()))
        {
            agent = other.gameObject;
            agentFound = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!interactOnTrigger)
            return;

        if (agentFound && agent == other.gameObject)
        {
            agentFound = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!interactOnCollision)
            return;

        if (collision.collider.GetComponent(interactionAgent.GetType()))
        {
            agent = collision.collider.gameObject;
            agentFound = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!interactOnCollision)
            return;

        if (agent == collision.collider.gameObject)
        {
            agentFound = false;
        }
    }

    #endregion

    #region Internals

    void HandleInput()
    {
        if (agentFound && interactable)
        {
            if (interactOnlyOnce)
                ToggleInteractability(false);
            OnInteraction.Invoke();
        }
    }

    #endregion
}

public enum GameInputActions
{
    OnJumpPressed,
    OnJumpReleased,
    OnRunPressed,
    OnRunReleased,
    OnAttackPressed,
    OnAttackReleased,
    OnDashPressed,
    OnDashReleased,
    OnMapPressed,
    OnMapReleased,
    OnInventoryPressed,
    OnInventoryReleased,
    OnPausePressed,
    OnPauseReleased,   
    OnSelectPressed,
    OnSelectionUpPressed,
    OnSelectionDownPressed
}