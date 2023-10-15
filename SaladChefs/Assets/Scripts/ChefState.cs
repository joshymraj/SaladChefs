using System;

public enum ChefState
{
    Idle,
    InteractingWithChoppingBoard,
    InteractingWithIngredient,
    InteractingWithSaladPlate,
    InteractingWithOrderBook,
    InteractingWithTrashCan,
    CarryingIngredient,
    CarryingSalad,
    ReadyToTakeOrder,
    ReadyToServe,
    ReceivingAccolade, // pause chef timer at this state
    Chopping,
    TimedOut // stop chef timer at this state
}

