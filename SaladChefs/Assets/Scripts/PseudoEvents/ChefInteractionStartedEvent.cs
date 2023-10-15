public class ChefInteractionStartedEvent
{
    public GameManager gameManager;

    public void Dispatch(Interactable interactable, string actionHint, int chefIndex)
    {
        if (gameManager != null)
        {
            gameManager.HandleChefInteractionStarted(interactable, actionHint, chefIndex);
        }
    }
}
