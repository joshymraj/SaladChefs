using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public string horizontalAxisName;
    public string verticalAxisName;
    public KeyCode actionKey1;
    public KeyCode actionKey2;

    float horizontalKeyPressFactor;
    float verticalKeyPressFactor;

    public bool Disabled
    {
        get;
        set;
    }

    public JoystickHorizontalKeyPressedEvent HorizontalKeyPressedEvent
    {
        get;
        private set;
    }

    public JoystickVerticalKeyPressedEvent VerticalKeyPressedEvent
    {
        get;
        private set;
    }

    public JoystickActionKeyPressedEvent ActionKeyPressedEvent
    {
        get;
        private set;
    }

    void Awake()
    {
        HorizontalKeyPressedEvent = new JoystickHorizontalKeyPressedEvent();
        VerticalKeyPressedEvent = new JoystickVerticalKeyPressedEvent();
        ActionKeyPressedEvent = new JoystickActionKeyPressedEvent();
    }

    void Update()
    {
        horizontalKeyPressFactor = Input.GetAxis(horizontalAxisName);
        verticalKeyPressFactor = Input.GetAxis(verticalAxisName);
    }

    void FixedUpdate()
    {
        OnHorizontalKeyPressed(horizontalKeyPressFactor);
        OnVerticalKeyPressed(verticalKeyPressFactor);

        if (Input.GetKeyDown(actionKey1))
        {
            OnActionKeyPressed(actionKey1);
        }

        if (Input.GetKeyDown(actionKey2))
        {
            OnActionKeyPressed(actionKey2);
        }
    }

    void OnHorizontalKeyPressed(float pressAmount)
    {
        if (!Disabled)
        {
            HorizontalKeyPressedEvent.Dispatch(pressAmount);
        }
    }

    void OnVerticalKeyPressed(float pressAmount)
    {
        if (!Disabled)
        {
            VerticalKeyPressedEvent.Dispatch(pressAmount);
        }
    }

    void OnActionKeyPressed(KeyCode key)
    {
        if (!Disabled)
        {
            ActionKeyPressedEvent.Dispatch(key);
        }
    }
}
