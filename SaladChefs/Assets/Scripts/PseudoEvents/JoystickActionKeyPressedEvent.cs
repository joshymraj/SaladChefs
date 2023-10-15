using UnityEngine;

public class JoystickActionKeyPressedEvent
{
    public ChefController chefController;

    public void Dispatch(KeyCode keyCode)
    {
        chefController.HandleJoystickActionButtonPressed(keyCode);
    }
}
