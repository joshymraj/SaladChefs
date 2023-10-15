public class GameOverShowLeaderboardEvent
{
    public GameManager GameManager
    {
        get;
        set;
    }

    public void Dispatch()
    {
        if(GameManager != null)
        {
            GameManager.HandleShowLeaderboard();
        }
    }
}

