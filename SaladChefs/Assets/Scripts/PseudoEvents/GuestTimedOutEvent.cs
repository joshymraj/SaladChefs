public class GuestTimedOutEvent
{
    public GameManager gameManager;

    public void Dispatch(int seatIndex)
    {
        if(gameManager != null)
        {
            gameManager.HandleGuestTimedOut(seatIndex);
        }
    }
}
