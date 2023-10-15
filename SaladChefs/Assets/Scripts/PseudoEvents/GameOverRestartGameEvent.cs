public class GameOverRestartGameEvent
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
            GameManager.HandleRestartGame();
        }
    }
}
