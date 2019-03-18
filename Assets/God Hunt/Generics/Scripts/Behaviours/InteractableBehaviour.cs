using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBehaviour : BaseBehaviour
{

    #region Serialized

    [SerializeField] MonoBehaviour interactionAgent;
    [SerializeField] bool interactOnTrigger;
    [SerializeField] bool interactOnCollision;
    [SerializeField] bool requireInput;
    [SerializeField] KeyCode interactionInput;
    [SerializeField] UnityVoidEvent OnInteraction;

    #endregion

    bool agentFound = false;
    GameObject agent;

    private void OnTriggerEnter(Collider other)
    {
        if (!interactOnTrigger)
            return;

        if (other.GetComponent(interactionAgent.GetType()))
        {
            agent = other.gameObject;
            if (!requireInput)
            {
                OnInteraction.Invoke();
                return;
            }
            agentFound = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!interactOnTrigger)
            return;

        if (agent == other.gameObject)
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
            if (!requireInput)
            {
                OnInteraction.Invoke();
                return;
            }
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

    public override void OnUpdate()
    {
        if(requireInput && agentFound)
        {
            if (Input.GetKeyDown(interactionInput))
                OnInteraction.Invoke();
        }
    }

}