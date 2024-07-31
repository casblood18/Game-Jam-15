using UnityEngine;


public class PlayerInteractionController : MonoBehaviour
{
    private Interactable interactingItem;

    private void OnEnable()
    {
        InputManager.Instance.OnInteractInput += PlayerTryInteract;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnInteractInput -= PlayerTryInteract;
    }

    private void PlayerTryInteract()
    {
        if (interactingItem != null) interactingItem.Interact();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.TryGetComponent<Interactable>(out interactingItem);
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        interactingItem = null;
    }
}
