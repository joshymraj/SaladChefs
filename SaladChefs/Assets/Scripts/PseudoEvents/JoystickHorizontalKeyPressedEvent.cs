public class JoystickHorizontalKeyPressedEvent
{
    public PlayerMotionController PlayerMotionController;

    public void Dispatch(float pressAmount)
    {
        if(PlayerMotionController != null)
        {
            PlayerMotionController.HandleJoystickTurnButtonPressed(pressAmount);
        }
    }
}
