using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBehaviour : BaseBehaviour
{

    #region Serialized

    [SerializeField] MonoBehaviour interactionAgent;
    [SerializeField] bool interactOnCollision;
    [SerializeField] bool interactOnTrigger;
    [SerializeField] bool cycleInteractions;
    [SerializeField] bool resetOnAreaExit;
    [SerializeField] List<Interaction> Interactions;

    #endregion

    bool agentFound = false;
    GameObject agent;
    Interaction currentInteraction;
    int currentInteractionIndex;
    float timer = 0;
    bool countTime = false;

    #region Overrides

    protected override void CustomSetup()
    {
        if (Interactions.Count > 0)
        {
            currentInteractionIndex = 0;
            currentInteraction = Interactions[currentInteractionIndex];

            for (int i = 0; i < Interactions.Count; i++)
            {
                Interactions[i].OnInteraction.AddListener(GoToNextInteraction);
            }
        }
    }

    public override void OnUpdate()
    {
        if (agentFound)
        {
            if (!currentInteraction.RequiresTimer && !currentInteraction.RequiresInput)
            {
                currentInteraction.OnInteraction.Invoke();
                return;
            }
            else
            if (currentInteraction.RequiresTimer && currentInteraction.RequiresInput)
            {
                if (countTime)
                    timer += Time.deltaTime;

                if (Input.GetButtonDown(InputManager.CurrentInputDevice + currentInteraction.Input))
                    countTime = true;

                if (Input.GetButtonUp(InputManager.CurrentInputDevice + currentInteraction.Input))
                {
                    countTime = false;
                    timer = 0;
                }

                if (timer >= currentInteraction.Time)
                {
                    currentInteraction.OnInteraction.Invoke();
                }
            }
            else
            if (currentInteraction.RequiresTimer && !currentInteraction.RequiresInput)
            {
                timer += Time.deltaTime;

                if (timer >= currentInteraction.Time)
                {
                    currentInteraction.OnInteraction.Invoke();
                }
            }
            else
            {
                if (Input.GetButtonDown(InputManager.CurrentInputDevice + currentInteraction.Input))
                {
                    currentInteraction.OnInteraction.Invoke();
                }
            }
        }
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
            timer = 0;
            if (resetOnAreaExit)
                currentInteraction = Interactions[0];
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
            timer = 0;
            if (resetOnAreaExit)
                currentInteraction = Interactions[0];
        }
    }

    #endregion

    #region Internals

    void GoToNextInteraction()
    {
        if (currentInteractionIndex >= Interactions.Count - 1)
        {
            if (!cycleInteractions)
            {
                OnInteractionsEnd();
                return;
            }
            else
            {
                currentInteractionIndex = 0;
            }
        }
        else
        {
            currentInteractionIndex++;
        }

        currentInteraction = Interactions[currentInteractionIndex];
        timer = 0;
    }

    void OnInteractionsEnd()
    {

    }

    #endregion

}

[System.Serializable]
public class Interaction
{
    public bool RequiresInput;
    public bool RequiresTimer;
    public string Input;
    public float Time;
    public UnityVoidEvent OnInteraction;
}