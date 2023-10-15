public class ChefPowerUpUsedEvent
{
    public GameManager gameManager;

    public void Dispatch(PowerUp powerUp, int chefIndex)
    {
        if(gameManager != null)
        {
            gameManager.HandleChefPowerUpUsed(powerUp, chefIndex);
        }
    }
}
