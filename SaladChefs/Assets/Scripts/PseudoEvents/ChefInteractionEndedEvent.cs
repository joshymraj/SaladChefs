public class ChefInteractionEndedEvent
{
    public GameManager gameManager;

    public void Dispatch(Interactable interactable, int chefIndex)
    {
        if (gameManager != null)
        {
            gameManager.HandleChefInteractionEnded(interactable, chefIndex);
        }
    }
}
