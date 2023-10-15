using UnityEngine;

public class ChefActionEvent
{
    public GameManager gameManager;

    public void Dispatch(Interactable interactable, int chefIndex, KeyCode key, string feedbackMessage)
    {
        if(gameManager != null)
        {
            gameManager.HandleChefAction(interactable, chefIndex, key, feedbackMessage);
        }
    }
}
