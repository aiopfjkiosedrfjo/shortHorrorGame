using UnityEngine;
using TMPro;
public class playerInteract : MonoBehaviour
{

    //BUGS:
    //Need to update the script disabling to be part of a state machine so its easier to manage and less buggy

    [SerializeField] private float castDistance = 10f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private CanvasGroup interactionText;
    [SerializeField] private KeyCode inputKey = KeyCode.E;


    private void Update()
    {
        if (DoInteractionTest(out IInteractable interactable))
        {
            if (interactable.CanInteract())
            {
                interactionText.alpha = 1f;
                if (Input.GetKeyDown(inputKey))
                    interactable.Interact(this);
            }
        }
        else
        {
            interactionText.alpha = 0f;

        }
    }
    private bool DoInteractionTest(out IInteractable interactable)
    {
        interactable = null;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, castDistance, interactableLayers))
        {
            interactable = hitInfo.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                return true;
            }
            return false;
        }
        return false;
    }
}
