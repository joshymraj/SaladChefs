public class ChefSaladTrashedEvent
{
    public GameManager gameManager;

    public void Dispatch(int noOfIngredients, int chefIndex)
    {
        if(gameManager != null)
        {
            gameManager.HandleChefSaladTrashed(noOfIngredients, chefIndex);
        }
    }
}

