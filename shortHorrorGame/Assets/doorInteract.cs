using UnityEngine;

public class doorInteract : MonoBehaviour, IInteractable
{
    public bool CanInteract()
    {
        return true;
    }
    public bool Interact(playerInteract interactor)
    {
        Debug.Log("interacted");
        return true;
    }
}
