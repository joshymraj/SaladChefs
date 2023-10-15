using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardDismissedEvent
{
    public GameManager gameManager;

    public void Dispatch()
    {
        if(gameManager != null)
        {
            gameManager.HandleLeaderboardDismissed();
        }
    }
}
