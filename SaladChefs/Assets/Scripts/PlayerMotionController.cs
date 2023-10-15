using UnityEngine;

public class PlayerMotionController : MonoBehaviour
{
    //public int playerNumber = 1;
    public float turnSpeed = 180;
    public float moveSpeed = 5;

    //[HideInInspector]
    //public bool hasSpeedBoost;

    public float speedBoost;

    public Animator playerAnimator;

    //string moveAxis;
    //string turnAxis;

    //float turnFactor;
    //float moveFactor;

    //Rigidbody rigidbody;
    public CharacterController characterController;
    public JoystickController joystickController;

    // Start is called before the first frame update
    void Start()
    {
        //moveAxis = string.Concat("Vertical", playerNumber.ToString());
        //turnAxis = string.Concat("Horizontal", playerNumber.ToString());

        //rigidbody = GetComponent<Rigidbody>();
        joystickController.HorizontalKeyPressedEvent.PlayerMotionController = this;
        joystickController.VerticalKeyPressedEvent.PlayerMotionController = this;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    turnFactor = Input.GetAxis(turnAxis);
    //    moveFactor = Input.GetAxis(moveAxis);
    //}

    //void FixedUpdate()
    //{
    //    Move();
    //    Turn();
    //}

    public void UpdateSpeedBoost(bool hasSpeed)
    {
        playerAnimator.SetBool("SpeedBoost", hasSpeed);
        if (hasSpeed)
        {
            moveSpeed += speedBoost;
        }
        else
        {
            moveSpeed -= speedBoost;
        }
    }

    public void HandleJoystickMoveButtonPressed(float moveFactor)
    {
        Vector3 movement = transform.forward * moveSpeed * moveFactor * Time.deltaTime;
        characterController.Move(movement);
        playerAnimator.SetFloat("Move", Mathf.Abs(moveFactor));
    }

    public void HandleJoystickTurnButtonPressed(float turnFactor)
    {
        float rotationAmount = turnSpeed * turnFactor * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(0, rotationAmount, 0);
        transform.Rotate(0, rotationAmount, 0);
    }
}
