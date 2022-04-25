using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    [SerializeField] UnityEvent OnSwitchPressed;
    [SerializeField] bool isActive = false;
    [SerializeField] Light lt;
    [SerializeField] Color inactiveColor = Color.red;
    [SerializeField] Color standbyColor = Color.blue;
    [SerializeField] Color activatedColor = Color.green;
    [SerializeField] Color activatingColor = Color.yellow;

    Elevator elevator;
    Door door;
    bool isCallElevatorBottom = false;
    bool isCallElevatorTop = false;
    Elevator elevatorToCall;

    public enum SwitchState
    {
        Inactive,
        Standby,
        Activated,
        Activating
    }

    [SerializeField] bool blinkWhenActivating = false;
    bool blinking = false;
    [SerializeField] float blinkSpeed = 0.1f;
    float blinkTime = Mathf.Infinity;

    SwitchState state = SwitchState.Inactive;

    private void Start()
    {
        if (isActive) SetState(SwitchState.Standby);

        door = GetComponentInParent<Door>();
        elevator = GetComponentInParent<Elevator>();
    }

    private void Update()
    {
        Blinking();
    }

    private void Blinking()
    {
        if (!blinking) return;

        blinkTime += Time.deltaTime;

        if (blinkTime > blinkSpeed)
        {
            if (lt.enabled)
            {
                lt.enabled = false;
            }
            else
            {
                lt.enabled = true;
            }

            blinkTime = 0;
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state == SwitchState.Inactive) return;

        PlayerSwitchInteraction player = other.GetComponent<PlayerSwitchInteraction>();
        if (player == null) return;

        player.ActivePlayerRaytracing(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (state == SwitchState.Inactive) return;

        PlayerSwitchInteraction player = other.GetComponent<PlayerSwitchInteraction>();
        if (player == null) return;

        player.ActivePlayerRaytracing(false);
    }

    public void SwitchPressed()
    {
        if (state == SwitchState.Inactive || state == SwitchState.Activating) return;

        if (isCallElevatorTop) elevatorToCall.CallElevator(false);
        else if (isCallElevatorBottom) elevatorToCall.CallElevator(true);
        else if (elevator != null) elevator.ElevatorToggle();
        else if (door != null) door.ToggleDoor();
        else OnSwitchPressed.Invoke();
    }

    public void SetState(SwitchState newState)
    {
        state = newState;
        
        switch (newState)
        {
            case SwitchState.Inactive:
                lt.enabled = true;
                lt.color = inactiveColor;
                break;

            case SwitchState.Standby:
                lt.enabled = true;
                lt.color = standbyColor;
                blinking = false;
                break;

            case SwitchState.Activated:
                lt.enabled = true;
                lt.color = activatedColor;
                blinking = false;
                break;

            case SwitchState.Activating:
                lt.color = activatingColor;
                if (blinkWhenActivating)
                {
                    blinking = true;
                    blinkTime = 0;
                }
                break;
        }
    }

    public void SetStateStandby()
    {
        SetState(SwitchState.Standby);
    }

    public void SetStateActivated()
    {
        SetState(SwitchState.Activated);
    }

    public void SetStateActivating()
    {
        SetState(SwitchState.Activating);
    }

    public void SetStateInactive()
    {
        SetState(SwitchState.Inactive);
    }

    public void SetElevatorCallSwitch(Elevator callingElevator, bool topSwitch)
    {
        elevatorToCall = callingElevator;
        if (topSwitch)
        {
            isCallElevatorTop = true;
        }
        else
        {
            isCallElevatorBottom = true;
        }
    }
}
