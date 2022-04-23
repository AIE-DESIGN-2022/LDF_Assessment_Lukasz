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
        SetState(SwitchState.Inactive);

        if (isActive) SetState(SwitchState.Standby);

        Door door = GetComponentInParent<Door>();
        if (door != null && door.IsActive()) SetState(SwitchState.Standby);
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
        OnSwitchPressed.Invoke();
    }

    public void SetActive(bool active)
    {
        isActive = active;
        if (isActive) lt.color = Color.green;
        else lt.color = Color.red;
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
}
