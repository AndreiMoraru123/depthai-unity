using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Default,
    HandGesture
}

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;

    [SerializeField]
    public string promptMessage;

    [SerializeField]
    public InteractionType interactionType = InteractionType.Default;

    public virtual bool ValidateInteraction()
    {
        return true;
    }

    public void BaseInteract()
    {
        if (!ValidateInteraction())
        {
            Debug.Log("invalid interaction");
            return;
        }

        if (useEvents)
        {

            GetComponent<InteractionEvent>().onInteract.Invoke();
        }

        Interact();
    }
    protected virtual void Interact()
    {
    }
}
