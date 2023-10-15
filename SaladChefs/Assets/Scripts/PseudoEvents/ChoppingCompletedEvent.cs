public class ChoppingCompletedEvent
{
    public ChefController chefController;

    public void Dispatch(Ingredient ingredient)
    {
        if(chefController != null)
        {
            chefController.HandleChoppingCompleted(ingredient);
        }
    }
}

