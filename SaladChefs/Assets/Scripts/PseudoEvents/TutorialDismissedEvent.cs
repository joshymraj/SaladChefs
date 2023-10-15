public class TutorialDismissedEvent
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
            GameManager.HandleTutorialCompleted();
        }
    }
}
