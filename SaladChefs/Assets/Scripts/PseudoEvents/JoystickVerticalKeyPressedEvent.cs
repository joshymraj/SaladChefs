public class JoystickVerticalKeyPressedEvent
{
    public PlayerMotionController PlayerMotionController;

    public void Dispatch(float pressAmount)
    {
        if(PlayerMotionController != null)
        {
            PlayerMotionController.HandleJoystickMoveButtonPressed(pressAmount);
        }
    }
}
