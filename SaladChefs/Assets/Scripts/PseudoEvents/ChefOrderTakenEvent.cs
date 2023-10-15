using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefOrderTakenEvent
{
    public GameManager gameManager;

    public void Dispatch(SaladData orderedSalad, int chefIndex)
    {
        if(gameManager != null)
        {
            gameManager.HandleChefOrderTaken(orderedSalad, chefIndex);
        }
    }
}
