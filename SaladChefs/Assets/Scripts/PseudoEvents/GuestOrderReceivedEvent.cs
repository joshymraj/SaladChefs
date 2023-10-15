public class GuestOrderReceivedEvent
{
    public GameManager gameManager;

    public void Dispatch(int servedChefIndex, bool isSatisfied, PowerUp tip, int tipValue, int seatIndex)
    {
        if(gameManager != null)
        {
            gameManager.HandleGuestOrderReceived(servedChefIndex, isSatisfied, tip, tipValue, seatIndex);
        }
    }
}
