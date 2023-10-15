public class ScoreboardTimedOutEvent
{
    public GameManager gameManager;

    public void Dispatch(PlayerScore finalScore)
    {
        if(gameManager != null)
        {
            gameManager.HandleScoreboardTimedOut(finalScore);
        }
    }
}
