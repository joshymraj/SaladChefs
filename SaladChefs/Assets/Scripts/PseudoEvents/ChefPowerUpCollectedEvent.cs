public class ChefPowerUpCollectedEvent
{
    public GameManager gameManager;

    public void Dispatch(PowerUpController powerUpController)
    {
        if(gameManager != null)
        {
            gameManager.HandleChefPowerUpCollected(powerUpController);
        }
    }
}
